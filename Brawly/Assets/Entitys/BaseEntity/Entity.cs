﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{ 
   public float health;
   public float speed = 5f;

   public Rigidbody rb;
   
   public float airMultiplier = 0.4f;

   //Entity direction and orientation
   public Vector3 moveDirection;
   
   public float maxSlopeAngle;
   public RaycastHit slopeHit;

  public CharacterController cc;

  public float horizontal;
  public float vertical;

  public Transform orientation;

  public Transform playerObj;
    

    //grounded checks
    public float groundDrag;

    public float Height;
    public LayerMask whatIsGround;
    public bool grounded;

    Vector3 faceDirection;

   
   public virtual void Start()
    {
     
    }

    // Update is called once per frame
   public virtual void Update()
   {
       ChainVars.UpdateOnSlope(onSlope());
   }

   public void updateAxis(float h, float v)
   {
       horizontal = h;
       vertical = v;
       
   }


   public void entityMovement()
   {
        moveDirection = orientation.forward * vertical + orientation.right * horizontal;

        if (onSlope() && !ChainVars.exitSlope)
       {
           Vector3 slopeDir = getSlopeMovementDir(moveDirection);
           
           rb.AddForce(slopeDir * speed * 20f, ForceMode.Force);
           ChainVars.UpdateDir(slopeDir);

           if (rb.velocity.y > 0)
           {
               rb.AddForce(Vector3.down * 80f, ForceMode.Force);
           }
       }
       
       if(grounded)
           rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
       
       if(!grounded)
           rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);
       
       speedControl();

       rb.useGravity = !onSlope();

        faceMoveDirection();
    }

   public void checkGrounded(float distToGround)
   {
       //The height MUST be tuned
       
        //grounded = Physics.Raycast(transform.position, Vector3.down, Height * 0.5f + 0.2f, whatIsGround);

        grounded = !Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f, whatIsGround);

        //grounded = Physics.Raycast(transform.position, Vector3.down, 0.6f, whatIsGround);

        // grounded = Physics.Raycast(transform.position, Vector3.down, 0.6f, whatIsGround);

        if (grounded)
           rb.drag = groundDrag;
       else rb.drag = 0;
   }


   public void speedControl()
   {

       if (onSlope() && !ChainVars.exitSlope)
       {
           if (rb.velocity.magnitude > speed)
               rb.velocity = rb.velocity.normalized * speed;
       }
       else
       {
           Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

           if (flatVel.magnitude > speed)
           {
               Vector3 limitedVel = flatVel.normalized * speed;
               rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
           }
       }
   }
   
   bool onSlope()
   {
       if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, Height * 0.5f + 0.3f))
       {
           float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

           return angle < maxSlopeAngle && angle != 0;
       }

       return false;
   }

   Vector3 getSlopeMovementDir(Vector3 direction)
   {
       return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
   }

    public void faceMoveDirection()
    {
        faceDirection = new Vector3(horizontal, 0, vertical);

        if (faceDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(faceDirection, Vector3.up);
            playerObj.transform.rotation = Quaternion.RotateTowards(playerObj.transform.rotation, toRotation, 720f * Time.deltaTime);
        }
    }

}
