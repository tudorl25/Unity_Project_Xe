using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : Entity
{
    public float jumpCooldown;
    public bool readyToJump;

    public float moveSpeed = 2.5f;
    public float sprintSpeed = 3.5f;

    public float slideSpeed = 7f;

    public float dashSpeed = 10f;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed; 
    
    public float crouchSpeed = 1.5f;

    public float idleTolerance = 0.5f;

    Jumping jump;
    Crouching cr;
    Sliding sd;

    public enum movementState
    {
        walking,
        sprinting,
        crouching,
        crouch_walking,
        sliding,
        dash,
        air,
        idle
    }

    public bool dashing = false;

    public movementState state = movementState.walking;

    public Animator anim;

    string lastStateName;

    public override void Start()
    {
        base.Start();
        
        jump = new Jumping(transform, rb);
        cr = new Crouching(transform, rb);
        sd = new Sliding(orientation, playerObj, rb);

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
        updateAnimation();

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
        StateHandler();
        checkJump();
        checkCrouch();
        checkSlide();
    }

  

    public void checkCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            cr.crouch();
        }
    }
    

    public void checkJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && readyToJump && grounded)
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
        if (Input.GetKeyDown(KeyCode.F) && (horizontal != 0 || vertical != 0) && rb.velocity.magnitude > 3f)
        {
            sd.startSliding();
        }
        
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            cr.normalize();
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
    
    public IEnumerator smoothlyLerpSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed = moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            speed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime;
            yield return null;
        }

        speed = desiredMoveSpeed;
    }

    private void StateHandler()
    {
        if (dashing)
        {
            desiredMoveSpeed = dashSpeed;
        } 
        else if (sd.sliding)
        {
            lastStateName = convertStateToStr(state);
            state = movementState.sliding;

            if (onSlope() && rb.velocity.y < 0.1f)
                desiredMoveSpeed = slideSpeed;
            else
                desiredMoveSpeed = sprintSpeed;
        } 
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            if (rb.velocity.magnitude > idleTolerance)
            {
                lastStateName = convertStateToStr(state);
                state = movementState.crouch_walking;
                desiredMoveSpeed = crouchSpeed;
            }
            else
            {
                lastStateName = convertStateToStr(state);
                state = movementState.crouching;
            }
        } 
        else if (grounded && Input.GetKey(KeyCode.LeftShift) && rb.velocity.magnitude>idleTolerance)
        {
            lastStateName = convertStateToStr(state);
            state = movementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            if (rb.velocity.magnitude < idleTolerance)
            {
                lastStateName = convertStateToStr(state);
                state = movementState.idle;
            }
            else
            {
                lastStateName = convertStateToStr(state);
                state = movementState.walking;
                desiredMoveSpeed = moveSpeed;
            }
        }
        else
        {
            lastStateName = convertStateToStr(state);
            state = movementState.air;
        }

        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && speed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(smoothlyLerpSpeed());
        }
        else
        {
            speed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
        

    }

    public bool addDrag()
    {
        return state == movementState.walking || state == movementState.crouching || state == movementState.sprinting || state == movementState.sliding;
    }


    public void updateAnimation()
    {
        if (lastStateName != convertStateToStr(state))
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
