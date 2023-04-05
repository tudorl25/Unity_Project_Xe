using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : Entity
{
    public float jumpCooldown;
    public bool readyToJump;

    public float moveSpeed = 6f;
    public float sprintSpeed = 10f;

    public float crouchSpeed;

    List<string> stateNames= new List<string>() { "isIdle", "isRunning", "isJumping", "isCrouching", "isCrouchWalking", "isWalking", "isSliding" };
    string lastStateName = string.Empty;

    Jumping jump;
    Crouching cr;
    Sliding sd;

    public enum movementState
    {
        idle,
        walking,
        sprinting,
        crouching,
        crouch_walking,
        sliding,
        air
    }

    public movementState state = movementState.idle;

    float distToGround;

    float idleTolerance = 3f;

    public Animator anim;

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

        //get distance to ground
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            distToGround = hit.distance;
        }

        anim = GetComponent<Animator>();

        lastStateName = stateNames[0];
    }


    public override void Update()
    {

        checkGrounded(distToGround);

        Move();

        checkCommands();

        base.Update();

        updateAnimation();
        
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
        if (state != movementState.air)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (rb.velocity.magnitude <= idleTolerance / 2)
                {
                    lastStateName = convertStateToStr(state);
                    state = movementState.crouching;
                }
                else
                {
                    lastStateName = convertStateToStr(state);
                    state = movementState.crouch_walking;
                    speed = crouchSpeed;
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                cr.crouch();
            }

            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                lastStateName = convertStateToStr(state);
                state = movementState.idle;
                cr.normalize();
            }
        }
    }

    public void checkMovement()
    {
        if (grounded && state != movementState.crouching && state != movementState.crouch_walking)
        {
            if (Input.GetKey(KeyCode.LeftShift) && rb.velocity.magnitude > idleTolerance)
            {
                lastStateName = convertStateToStr(state);
                state = movementState.sprinting;
                speed = sprintSpeed;
            }
            else if(rb.velocity.magnitude <= idleTolerance)
            {
                lastStateName = convertStateToStr(state);
                state = movementState.idle;
            }
            else
            {
                lastStateName = convertStateToStr(state);
                state = movementState.walking;
                speed = moveSpeed;
            }
        }
        else if (!grounded)
        {
            lastStateName = convertStateToStr(state);
            state = movementState.air;
        }
    }

    public void checkJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && readyToJump && grounded && state != movementState.crouching && state != movementState.crouch_walking)
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
            lastStateName = convertStateToStr(state);
            state = movementState.sliding;
            
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

    public void updateAnimation()
    {
        if(lastStateName != convertStateToStr(state))
            anim.SetBool(lastStateName, false);
        switch (state)
        {
            case movementState.idle:
                anim.SetBool("isIdle", true);
                break;

            case movementState.sprinting:
                anim.SetBool("isRunning", true);
                break;

            case movementState.air:
                anim.SetBool("isJumping", true);
                break;

            case movementState.crouching:
                anim.SetBool("isCrouching", true);
                break;

            case movementState.crouch_walking:
                anim.SetBool("isCrouchWalking", true);
                break;

            case movementState.walking:
                anim.SetBool("isWalking", true);
                break;

            case movementState.sliding:
                anim.SetBool("isSliding", true);
                break;
        }
    }

    string convertStateToStr(movementState stat)
    {
        if (stat == movementState.idle) return "isIdle";
        else if (stat == movementState.sprinting) return "isRunning";
        else if (stat == movementState.air) return "isJumping";
        else if (stat == movementState.crouching) return "isCrouching";
        else if (stat == movementState.crouch_walking) return "isCrouchWalking";
        else if (stat == movementState.walking) return "isWalking";
        else return "isSliding";
    }
}
