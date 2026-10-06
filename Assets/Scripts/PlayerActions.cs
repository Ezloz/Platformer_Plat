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
            rigid_body.AddForceX(this.properties.acceleration);
        }
        else if (direction < -0.5) // TBA : make it relative to the sensitivity option IF CONTROLLER
        {
            rigid_body.AddForceX(-this.properties.acceleration);
        }

    }


    void Jump(bool jump_button_pressed)
    {
        if(jump_button_pressed && isGrounded)
        {
            rigid_body.AddForceY(this.properties.jump_strength, ForceMode2D.Impulse);
            isGrounded = false;
        }

    }

    void OnCollisionEnter2D(Collision2D other) {
        isGrounded = true;
        
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    void FixedUpdate()
    {
        float horizontal_direction = move.ReadValue<Vector2>().x;
        Move(horizontal_direction);
        Jump(jump.IsPressed());
        print(horizontal_direction);
        
    }
}
