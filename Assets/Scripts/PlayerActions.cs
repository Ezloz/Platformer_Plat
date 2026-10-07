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
    private GameObject attack_sprite_instance = null;
    private PlayerFX fx => GetComponent<PlayerFX>();


    private enum Direction {Left, Right}
    private Direction currentDirection = Direction.Left;

    private bool isDashing = false;
    private bool isJumping = false;
    private bool isAttacking = false;


    private float dashTimer = 0f;
    private float attackTimer = 0f;
    private float jumpTimer = 0f;
    


    void Start()
    {
        move = InputSystem.actions.FindAction("Move");
        look = InputSystem.actions.FindAction("Look");
        attack = InputSystem.actions.FindAction("Attack");
        jump = InputSystem.actions.FindAction("Jump");
        dash = InputSystem.actions.FindAction("Dash");
    }

    void Move()
    {
        float direction = move.ReadValue<Vector2>().x;
        //print(horizontal_direction);


        if (direction > 0.5) // TBA : make it relative to the sensitivity option IF CONTROLLER 
        {
            if (GetIsGrounded())
            {
                rigid_body.AddForceX(properties.acceleration, ForceMode2D.Impulse);                
            }
            else
            {
                rigid_body.AddForceX(properties.acceleration, ForceMode2D.Force);
            }
            currentDirection = Direction.Right;
          
        }
        else if (direction < -0.5) // TBA : make it relative to the sensitivity option IF CONTROLLER
        {
            if (GetIsGrounded())
            {
                rigid_body.AddForceX(-properties.acceleration, ForceMode2D.Impulse);                
            }
            else
            {
                rigid_body.AddForceX(-properties.acceleration, ForceMode2D.Force);
            }
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
            fx.EmitTrail(0.5f, properties.dash_time);
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
        return (Mouse.current.position.ReadValue() - (Vector2) transform.position).normalized;

    }

    void Attack() { 

        if (attack.WasPressedThisFrame() && !isAttacking)
        {
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
            this.attack_sprite_instance.GetComponent<SpriteRenderer>().enabled = false;
            return;
        }

        Vector2 direction = GetAttackDirection();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // merci Chatgpt
        if (!attack_sprite_instance)
        {
            this.attack_sprite_instance = Instantiate(attack_sprite, transform.position, Quaternion.identity);        
        }
        this.attack_sprite.transform.rotation = Quaternion.Euler(0, 0, angle);
        this.attack_sprite_instance.GetComponent<SpriteRenderer>().enabled = true;
        
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
            rigid_body.AddForceY(this.properties.jump_strength, ForceMode2D.Impulse);
            isJumping = true;
            jumpTimer = properties.jump_time_length;
        }
        else if(isJumping && jump.IsPressed() && jumpTimer > 0f)
        {
            //We keep isJumping to true
            //see corresponding code in FixedUpdate            
        }
        else
        {
            jumpTimer = 0f;
            isJumping = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDashing)
        {
            isDashing = false;
            fx.StopEmiting();
        }
    }


    void Update()
    {
//        Debug.DrawRay(this.transform.position, rigid_body.linearVelocity, Color.red);    
        if (isDashing)
        {
            Dashing();
        }
        else if (isAttacking)
        {
            Attacking();
        }
        else
        {
            Move();
            Jump();
            Dash();
            Attack();            
        }
    }

  void FixedUpdate()
  {
    if(isJumping && jump.IsPressed() && jumpTimer > 0f)
    {
        jumpTimer -= Time.deltaTime;
//        Debug.DrawRay(this.transform.position, -Physics2D.gravity * rigid_body.mass, Color.blue);    
        rigid_body.AddForce(-Physics2D.gravity *rigid_body.gravityScale * rigid_body.mass * Mathf.Max(1-properties.jump_damping,0f));
    }

    
  }

}

