using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : Entity
{

    public float jumpForce = 7;
    public float jumpCooldown;
    public bool readyToJump;

   // public int jumpCounter = 1;

    public override void Start()
    {
        health = 100;

        speed = 6f;


        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        groundDrag = 5f;

        Height = 2;
        
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
        

        if (Input.GetKeyDown(KeyCode.Space) && readyToJump )
        {
            readyToJump = false;
            
            Jump();
            
            Invoke(nameof(ResetJump),jumpCooldown);
        }

        //double jump concept, needs more work but for later
        /*
        if (rJ() && Input.GetKeyDown(KeyCode.E))
        {
            jumpCounter--;
            
            Jump();

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
    
    
   /* public bool rJ()
    {
        if (grounded)
        {
            jumpCounter = 2;
        }

        if (jumpCounter == 0)
        {
            return false;
        }
        
         return true;
    }*/
    
}
