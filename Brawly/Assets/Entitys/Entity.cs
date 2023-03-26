using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{ 
   public float health;
   public float speed = 5f;

   public Rigidbody rb;
   
   public float airMultiplier = 0.4f;

   //Entity direction and orientation
   private Vector3 moveDirection;
   
  public CharacterController cc;

    float horizontal;
    float vertical;

    public Transform orientation;

    //grounded checks
    public float groundDrag;

    public float Height;
    public LayerMask whatIsGround;
    public bool grounded;
   
   public virtual void Start()
    {
        
    }

    // Update is called once per frame
   public virtual void Update()
   {
       
   }

   public void updateAxis(float h, float v)
   {
       horizontal = h;
       vertical = v;
   }


   public void entityMovement()
   {
       moveDirection = orientation.forward * vertical + orientation.right * horizontal;
       
       if(grounded)
       rb.AddForce(moveDirection.normalized * speed * 10f,ForceMode.Force);
       
       if(!grounded)
           rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier,ForceMode.Force);
       
       speedControl();
   }

   public void checkGrounded()
   {
       //The height MUST be tuned
       
       grounded = Physics.Raycast(transform.position, Vector3.down, Height * 0.5f + 0.2f, whatIsGround);
       
      // grounded = Physics.Raycast(transform.position, Vector3.down, 0.6f, whatIsGround);
       
       if (grounded)
           rb.drag = groundDrag;
       else rb.drag = 0;
   }


   public void speedControl()
   {
       Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

       if (flatVel.magnitude > speed)
       {
           Vector3 limitedVel = flatVel.normalized * speed;
           rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
       }
   }

}
