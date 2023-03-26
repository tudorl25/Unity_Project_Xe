using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : Entity
{

    public float jumpForce = 7;
    public float jumpCooldown;
    public bool readyToJump;

    public int jumpCounter = 1;

    public override void Start()
    {
        health = 100;

        speed = 6f;


        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        groundDrag = 5f;

        Height = 1;
        
    }


    public override void Update()
    {
       Move();
       
       checkCommands();
       
       Debug.Log(grounded);
       
    }

    public void Move()
    { 
        updateAxis(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));

        checkGrounded();
        
        entityMovement();

    }

    public void checkCommands()
    {
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 10f;
        }else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 6f;
        }
        

        if (Input.GetKeyDown(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;
            
            Jump();
            
            Invoke(nameof(ResetJump),jumpCooldown);
        }


        //double jump works flawlessly, just is primitve
        
       /* if (grounded)
        {
            jumpCounter = 1;
        }
       
        if (jumpCounter != 0 && Input.GetKeyDown(KeyCode.E) && readyToJump)
        {
            jumpCounter--;
            
            Jump();

            if (jumpCounter == 1)
            {
                readyToJump = true;
                rb.drag = groundDrag;
            }

            if (jumpCounter == 0)
            {
                Invoke(nameof(ResetJump),jumpCooldown);
            }
        }*/
        
    }

    public void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void ResetJump()
    {
        readyToJump = true;
    }

    
    //Double jump concept, further improvement needed
    
    
    
    
}
