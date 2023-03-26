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
    
    
    
    public override void Start()
    {
        health = 100;

        speed = moveSpeed;

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
        
       

    }

    public void checkSprint()
    {
        if (grounded)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = sprintSpeed;
            }
            else
            {
                speed = moveSpeed;
            }
        }
    }

    public void checkJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;
            
            Jump();
            
            Invoke(nameof(ResetJump),jumpCooldown);
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
    }
    
    
}
