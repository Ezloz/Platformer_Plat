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

    private Properties properties => this.GetComponent<Properties>();

    private Rigidbody2D rigid_body => this.gameObject.GetComponent<Rigidbody2D>();

    private bool isGrounded = true;


    void Start()
    {
        move = InputSystem.actions.FindAction("Move");
        look = InputSystem.actions.FindAction("Look");
        attack = InputSystem.actions.FindAction("Attack");
        jump = InputSystem.actions.FindAction("Jump");
        dash = InputSystem.actions.FindAction("Dash");
        
    }

    void Move(float direction)
    {

        if (direction > 0.5) // TBA : make it relative to the sensitivity option IF CONTROLLER 
        {
            rigid_body.AddForceX(this.properties.acceleration, ForceMode2D.Impulse);
            //if (rigid_body.linearVelocityX < 0)
            //{
            //    rigid_body.AddForceX(this.properties.acceleration);
            //}

        }
        else if (direction < -0.5) // TBA : make it relative to the sensitivity option IF CONTROLLER
        {
            rigid_body.AddForceX(-this.properties.acceleration, ForceMode2D.Impulse);
            //if (rigid_body.linearVelocityX > 0)
            //{
            //    rigid_body.AddForceX(-this.properties.acceleration);
            //}
        }
        else {
        
        }
        rigid_body.linearVelocityX = Mathf.Clamp(rigid_body.linearVelocityX,-properties.max_velocity,properties.max_velocity);
        Debug.Log(rigid_body.linearVelocityX);
    }

    private bool GetIsGrounded()
    {
//        Debug.DrawRay(this.transform.position, Vector2.down*this.gameObject.GetComponent<SpriteRenderer>().bounds.extents.y+0.01f, Color.red);
        return Physics2D.Raycast(this.transform.position, Vector2.down, this.gameObject.GetComponent<SpriteRenderer>().bounds.extents.y+0.01f, LayerMask.GetMask("Platform", "Ground"));

    }

    void Jump()
    {

        if(jump.WasPressedThisFrame() && GetIsGrounded())
        {
            rigid_body.AddForceY(this.properties.jump_strength, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }



    void Update()
    {
        float horizontal_direction = move.ReadValue<Vector2>().x;
        Move(horizontal_direction);
        Jump();
        print(horizontal_direction);
        
    }
}
