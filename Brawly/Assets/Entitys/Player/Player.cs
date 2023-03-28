using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : Entity
{

    public float jumpForce = 7;
    public float jumpCooldown;
    public bool readyToJump;

    public float moveSpeed = 6f;
    public float sprintSpeed = 10f;

    public float crouchSpeed;
    public float crouchYScale;
    public float startYScale;

    public ChainVars cs = new ChainVars();
    
    
    public enum movementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    public movementState state = movementState.walking;
    
    public override void Start()
    {
        health = 100;

        speed = moveSpeed;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        groundDrag = 5f;

        Height = 1;

        startYScale = transform.localScale.y;

    }


    public override void Update()
    {
        checkGrounded();
        
       Move();
       
       checkCommands();

    }

    public void Move()
    { 
        updateAxis(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));

        entityMovement();

    }

    public void checkCommands()
    {
        checkMovement();
        checkJump();
        checkCrouch();
    }

    public void checkCrouch()
    {
        if (Input.GetKey(KeyCode.LeftControl) && state != movementState.air)
        {
            state = movementState.crouching;
            speed = crouchSpeed;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftControl) && state != movementState.air)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            state = movementState.walking;
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    public void checkMovement()
    {
        if (grounded && state != movementState.crouching)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                state = movementState.sprinting;
                speed = sprintSpeed;
            }
            else
            {
                state = movementState.walking;
                speed = moveSpeed;
            }
        }
        else if(state != movementState.crouching)
        {
            state = movementState.air;
        }
    }

    public void checkJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && readyToJump && grounded && state != movementState.crouching)
        {
               cs.exitSlope = true;
            
                readyToJump = false;

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
        }

    }

    public void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void ResetJump()
    {
        readyToJump = true;

        cs.exitSlope = false;
    }
    
}
