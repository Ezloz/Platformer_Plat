using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerActions : MonoBehaviour
{
    private InputAction move;
    private InputAction look;
    private InputAction attack;
    private InputAction jump;
    private InputAction dash;

    private Properties properties => GetComponent<Properties>();
    private Rigidbody2D rigid_body => GetComponent<Rigidbody2D>();

    [SerializeField] private GameObject attack_sprite;

    private enum Direction {Left, Right}
    private Direction currentDirection = Direction.Left;

    private bool isGrounded = true;
    private bool isDashing = false;
    private bool isJumping = false;
    private bool isAttacking = false;


    private float dashTimer = 0f;
    private float jumpTimer = 0f;
    private float attackTimer = 0f;
    


    void Start()
    {
        move = InputSystem.actions.FindAction("Move");
        look = InputSystem.actions.FindAction("Look");
        attack = InputSystem.actions.FindAction("Attack");
        jump = InputSystem.actions.FindAction("Jump");
        dash = InputSystem.actions.FindAction("Dash");
        this.attack_sprite.GetComponent<SpriteRenderer>().enabled = false;
    }

    void Move()
    {
        float direction = move.ReadValue<Vector2>().x;
        //print(horizontal_direction);


        if (direction > 0.5) // TBA : make it relative to the sensitivity option IF CONTROLLER 
        {
            rigid_body.AddForceX(properties.acceleration, ForceMode2D.Impulse);
            currentDirection = Direction.Right;
          
        }
        else if (direction < -0.5) // TBA : make it relative to the sensitivity option IF CONTROLLER
        {
            rigid_body.AddForceX(-properties.acceleration, ForceMode2D.Impulse);
            currentDirection = Direction.Left;
        }
        else {
            rigid_body.linearVelocityX = Mathf.Lerp(rigid_body.linearVelocityX, 0, Time.deltaTime * properties.deccelarition);
        }
        rigid_body.linearVelocityX = Mathf.Clamp(rigid_body.linearVelocityX,-properties.max_velocity,properties.max_velocity);
    }

    void Dash() { 

        if (dash.WasPressedThisFrame() && !isDashing)
        {
            isDashing = true;
            dashTimer = 0f;
        }

    }

    void Dashing()
    {
        dashTimer += Time.deltaTime;
        if (dashTimer > properties.dash_time)
        {
            isDashing = false;
            return;
        }
        float dir = currentDirection == Direction.Left ? -1f : 1f;
        rigid_body.linearVelocityX = dir * properties.dash_velocity;
        rigid_body.linearVelocityY = 0f;
        
    }

    private Vector2 GetAttackDirection()
    {
        return (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

    }

    void Attack() { 

        if (attack.WasPressedThisFrame() && !isAttacking)
        {
            print("test");
            isAttacking = true;
            attackTimer = 0f;
        }

    }

    void Attacking()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > properties.attack_time)
        {
            isAttacking = false;
            this.attack_sprite.GetComponent<SpriteRenderer>().enabled = false;
            return;
        }

        Vector2 direction = GetAttackDirection();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // merci Chatgpt
        this.attack_sprite.transform.rotation = Quaternion.Euler(0, 0, angle);
        this.attack_sprite.GetComponent<SpriteRenderer>().enabled = true;
        
    }

    private bool GetIsGrounded()
    {
//        Debug.DrawRay(this.transform.position, Vector2.down*this.gameObject.GetComponent<SpriteRenderer>().bounds.extents.y+0.01f, Color.red);
        return Physics2D.Raycast(transform.position, Vector2.down, GetComponent<SpriteRenderer>().bounds.extents.y+0.01f, LayerMask.GetMask("Platform", "Ground"));

    }

    void Jump()
    {

        if(jump.WasPressedThisFrame() && GetIsGrounded())
        {
            rigid_body.AddForceY(this.properties.jump_strength_min, ForceMode2D.Impulse);
            isGrounded = false;
            isJumping = true;
            jumpTimer = properties.jump_max_time;
        }
        else if (jump.IsPressed() && isJumping && jumpTimer > 0f)
        {
            rigid_body.AddForceY(properties.jump_strength_max/properties.jump_max_time, ForceMode2D.Force);
            jumpTimer -= Time.deltaTime;
        }
        else
        {
            isJumping = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDashing)
        {
            isDashing = false;
        }
    }


    void Update()
    {
        if (!isDashing)
        {
            Move();
            Jump();
            Dash();
        }
        else
        {
            Dashing();
        }
    }
}

