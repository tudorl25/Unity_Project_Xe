using UnityEngine;

public class Crouching
{
    public float crouchYScale = 0.5f;
    public float startYScale;

     Transform tf;
     Rigidbody rb;

     public Crouching(Transform tf, Rigidbody rb)
     {
         this.tf = tf;
         this.rb = rb;
     }

    public void crouch()
    {
        tf.localScale = new Vector3(tf.localScale.x, crouchYScale, tf.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    public void normalize()
    {
        tf.localScale = new Vector3(tf.localScale.x, startYScale, tf.localScale.z);
    }
        
}
