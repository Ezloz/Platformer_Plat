using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerActions : MonoBehaviour
{

    [SerializeField] private AudioSource landingSFX;
    [SerializeField] private AudioSource jumpSFX;
    private InputAction move;
    private InputAction look;
    private InputAction attack;
    private InputAction jump;
    private InputAction dash;
    private InputAction reverse_look;


    private Properties properties => GetComponent<Properties>();
    private Rigidbody2D rigid_body => GetComponent<Rigidbody2D>();
    private ControllerDetector user_controller => GetComponent<ControllerDetector>();

    [SerializeField] private GameObject attack_sprite;

    [SerializeField] private GameObject head;
    private GameObject attack_sprite_instance = null;
    private PlayerFX fx => GetComponent<PlayerFX>();


    private enum Direction {Left, Right}
    private Direction currentDirection = Direction.Left;

    private Vector2 lookDirection = Vector2.right;

    private bool wasInAir = false;


    private bool isDashing = false;
    private bool isJumping = false;
    private bool isAttacking = false;


    private float dashTimer = 0f;
    private float attackTimer = 0f;
    private float jumpTimer = 0f;

    private int jumpBufferFrames = 0;

    private int dashCharge = 1;    


    void Start()
    {
        move = InputSystem.actions.FindAction("Move");
        look = InputSystem.actions.FindAction("Look");
        attack = InputSystem.actions.FindAction("Attack");
        jump = InputSystem.actions.FindAction("Jump");
        dash = InputSystem.actions.FindAction("Dash");
        reverse_look = InputSystem.actions.FindAction("ReverseLook");
    }

    void Move()
    {
        float direction = move.ReadValue<Vector2>().x;
        //print(horizontal_direction);

        bool isGrounded = GetIsGrounded();

        if (direction > 0.5) // TBA : make it relative to the sensitivity option IF CONTROLLER 
        {
            if (isGrounded)
            {
                rigid_body.AddForceX(properties.acceleration, ForceMode2D.Impulse);                
            }
            else
            {
                rigid_body.AddForceX(properties.air_acceleration, ForceMode2D.Impulse);
            }
            currentDirection = Direction.Right;
          
        }
        else if (direction < -0.5) // TBA : make it relative to the sensitivity option IF CONTROLLER
        {
            if (isGrounded)
            {
                rigid_body.AddForceX(-properties.acceleration, ForceMode2D.Impulse);                
            }
            else
            {
                rigid_body.AddForceX(-properties.air_acceleration, ForceMode2D.Impulse);
            }
            currentDirection = Direction.Left;
        }
        else {
            rigid_body.linearVelocityX = Mathf.Lerp(rigid_body.linearVelocityX, 0, Time.deltaTime * properties.deccelarition);
        }
        rigid_body.linearVelocityX = Mathf.Clamp(rigid_body.linearVelocityX,-properties.max_velocity,properties.max_velocity);
    }

    void Dash() { 

        if (dash.WasPressedThisFrame() && !isDashing && dashCharge > 0)
        {
            isDashing = true;
            dashCharge -= 1;
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

    private Vector2 GetAttackDirection() // MUST HAVE A MAIN CAMERA
    {
        Vector2 attack_direction = Vector2.left;
        if (user_controller.currentDevice == "Controller")
        {
            attack_direction = move.ReadValue<Vector2>().normalized;
        }
        if (user_controller.currentDevice == "Keyboard")
        {
            if (Camera.main == null)
            {
                print("No main Camera found");
                return Vector2.right;
            }
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
                new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -Camera.main.transform.position.z));
            
            attack_direction = ((Vector2)mousePosition - (Vector2)transform.position).normalized;
        }


        if (reverse_look.IsPressed())
        {
            attack_direction = -attack_direction;
        }

        return attack_direction;
    }

    private void BounceAttack(Vector2 direction)
    {
        direction = direction.normalized; // just in case
        direction = -direction;
        float speed = Mathf.Clamp(Mathf.Sqrt(rigid_body.linearVelocity.sqrMagnitude), properties.bounce_minimum_velocity, properties.bounce_maximum_velocity);
        rigid_body.linearVelocity = direction*speed*properties.bounce_velocity_conversion_coef;

    }


    void Attack() { 

        if (attack.WasPressedThisFrame() && !isAttacking)
        {
            dashTimer = properties.dash_time+1;
            isDashing = false;
            isAttacking = true;
            attackTimer = 0f;
        }

    }

    void Attacking()
    {
        if (attackTimer == 0f)
        {
            Vector2 direction = GetAttackDirection();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // merci Chatgpt
            if (!attack_sprite_instance)
            {
                this.attack_sprite_instance = Instantiate(attack_sprite, transform.position, Quaternion.identity, this.transform);
            }
            this.attack_sprite_instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            this.attack_sprite_instance.GetComponent<SpriteRenderer>().enabled = true;
            this.attack_sprite_instance.GetComponent<BoxCollider2D>().enabled = true;
        }

        attackTimer += Time.deltaTime;
        if (attackTimer > properties.attack_time)
        {
            isAttacking = false;
            this.attack_sprite_instance.GetComponent<SpriteRenderer>().enabled = false;
            this.attack_sprite_instance.GetComponent<BoxCollider2D>().enabled = false;
            return;
        }



    }

    private bool GetIsGrounded()
    {
//        Debug.DrawRay(transform.position + Vector3.right*GetComponent<SpriteRenderer>().bounds.extents.x, Vector2.down * (this.gameObject.GetComponent<SpriteRenderer>().bounds.extents.y+0.01f), Color.red);
        return Physics2D.Raycast(transform.position + Vector3.right*(GetComponent<SpriteRenderer>().bounds.extents.x-0.01f), Vector2.down, GetComponent<SpriteRenderer>().bounds.extents.y+0.01f, LayerMask.GetMask("Platform", "Ground"))
            || Physics2D.Raycast(transform.position + Vector3.left*(GetComponent<SpriteRenderer>().bounds.extents.x-0.01f), Vector2.down, GetComponent<SpriteRenderer>().bounds.extents.y+0.01f, LayerMask.GetMask("Platform", "Ground")); 

    }

    void Jump()
    {
        if((jump.WasPressedThisFrame() || jumpBufferFrames > 0) && GetIsGrounded())
        {
            jumpBufferFrames = 0;
            rigid_body.linearVelocityY = 0;
            rigid_body.AddForceY(this.properties.jump_strength, ForceMode2D.Impulse);
            isJumping = true;
            jumpTimer = properties.jump_time_length;
            jumpSFX.Play();
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

    void Look()
    {
//        float scale = currentDirection == Direction.Left? 1f : -1f;
//        head.transform.Rotate() = new Vector3(scale, 1f, 1f);
        head.transform.rotation = Quaternion.FromToRotation(Vector2.right, GetAttackDirection());
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
        //Debug.DrawRay(this.transform.position, rigid_body.linearVelocity, Color.red);    

        if (jumpBufferFrames > 0)
        {
            jumpBufferFrames -= 1;
            print(jumpBufferFrames);
        }
        if (jump.WasPerformedThisFrame())
        {
            jumpBufferFrames = properties.jump_buffer_nb_frames;
            print("Hello" + jumpBufferFrames);
        }


        if (isDashing)
        {
            Dashing();
            Attack(); 
        }
        else if (isAttacking)
        {
            Jump();
            Attacking();
        }
        else
        {
            Jump();
            Dash();
            Attack();            
        }
        Look();
        if (GetIsGrounded())
        {
            dashCharge = 1;
            if (wasInAir)
            {
                landingSFX.Play();
            }
            wasInAir = false;
        }
        else { wasInAir = true; }
    }

  void FixedUpdate()
  {

        if (isDashing)
        {
        }
        else if (isAttacking)
        {
            Move();
        }
        else
        {
            Move();
        }


    if(isJumping && jump.IsPressed() && jumpTimer > 0f)
    {
        jumpTimer -= Time.deltaTime;
//        Debug.DrawRay(this.transform.position, -Physics2D.gravity * rigid_body.mass, Color.blue);    
        rigid_body.AddForce(-Physics2D.gravity *rigid_body.gravityScale * rigid_body.mass * Mathf.Max(1-properties.jump_damping,0f));
    }

    if (this.attack_sprite_instance != null && this.attack_sprite_instance.GetComponent<BoxCollider2D>().IsTouchingLayers())
    {
        attack_sprite_instance.GetComponent<AudioSource>().Play();
        BounceAttack(this.attack_sprite_instance.transform.right);
        isAttacking = false;
        this.attack_sprite_instance.GetComponent<SpriteRenderer>().enabled = false;
        this.attack_sprite_instance.GetComponent<BoxCollider2D>().enabled = false;
        dashCharge = 1;
        return;
    }


  }

}

