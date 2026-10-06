using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    private InputAction move;
    private InputAction look;
    private InputAction attack;
    private InputAction jump;
    private InputAction dash;

    private Properties properties => this.GetComponent<Properties>();

    private Rigidbody2D rigid_body => this.gameObject.GetComponent<Rigidbody2D>();

    [SerializeField] float dash_velocity;
    [SerializeField] float dash_time;
    [SerializeField] float bounce_velocity_conversion_coef;




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

    void FixedUpdate()
    {
        float horizontal_direction = move.ReadValue<Vector2>().x;
        Move(horizontal_direction);
        print(horizontal_direction);
        
    }
}
