using UnityEngine;

public class Jumping
{
    public float jumpForce = 7;

     Transform tf;
     Rigidbody rb;
    
    public Jumping(Transform tf, Rigidbody rb)
    {
        this.tf = tf;
        this.rb = rb;
    }
    
    public void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        rb.AddForce(tf.up * jumpForce, ForceMode.Impulse);
    }
    
}
