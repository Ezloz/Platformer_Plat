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

    private enum Direction {Left, Right}
    private Direction currentDirection = Direction.Left;

    private bool isGrounded = true;
    private bool isDashing = false;
    private float dashTimer = 0f;


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
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDashing)
        {
            isDashing = false;
        }
    }
    void OnCollisionStay()
    {
        isGrounded = true;

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
        //print(horizontal_direction);
    }
}

