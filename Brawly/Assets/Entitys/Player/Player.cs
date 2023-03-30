using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : Entity
{
    public float jumpCooldown;
    public bool readyToJump;

    public float moveSpeed = 6f;
    public float sprintSpeed = 10f;

    public float crouchSpeed;

    Jumping jump;
    Crouching cr;
    Sliding sd;

    public enum movementState
    {
        walking,
        sprinting,
        crouching,
        sliding,
        air
    }

    public movementState state = movementState.walking;

    public override void Start()
    {
        jump = new Jumping(transform, rb);
        cr = new Crouching(transform, rb);
        sd = new Sliding(orientation, playerObj, rb, horizontal, vertical);

        health = 100;

        speed = moveSpeed;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        groundDrag = 5f;

        Height = 1;

        cr.startYScale = transform.localScale.y;
        sd.startYScale = transform.localScale.y;
    }


    public override void Update()
    {
        checkGrounded();

        Move();

        checkCommands();

        base.Update();
        
        Debug.Log(state);

    }

    public void Move()
    {
        updateAxis(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        entityMovement();

    }

    public void checkCommands()
    {
        checkMovement();
        checkJump();
        checkCrouch();
        checkSlide();
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
            cr.crouch();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            state = movementState.walking;
            cr.normalize();
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
        else if (state != movementState.crouching)
        {
            state = movementState.air;
        }
    }

    public void checkJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && readyToJump && grounded && state != movementState.crouching)
        {
                ChainVars.exitSlope = true;
            
                readyToJump = false;

                jump.Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
        }

    }
  
    public void ResetJump()
    {
        readyToJump = true;

        ChainVars.exitSlope = false;
    }

    public void checkSlide()
    {
        if (Input.GetKeyDown(KeyCode.F) && (horizontal != 0 || vertical != 0) && rb.velocity.magnitude > 6f && state != movementState.air)
        {
          //   state = movementState.sliding;
            
            sd.startSliding();
        }

        if (Input.GetKeyUp(KeyCode.F) && sd.sliding)
        {
            sd.stopSlide();
        }

        if (sd.sliding)
        {
            sd.slidingMovement();
        }
        
    }
    
}
