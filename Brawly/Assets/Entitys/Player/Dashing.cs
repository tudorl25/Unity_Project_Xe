using UnityEngine;


public class Dashing
{

    public Transform playerObj;
    public Rigidbody rb;
    
    public float dashForce = 20f;


    public float dashCd;
    public float dashCdTimer;

    public Dashing(Transform playerObj, Rigidbody rb)
    {
        this.playerObj = playerObj;
        this.rb = rb;
    }
    

    public void dashing()
    {

        Vector3 dashDirection = playerObj.forward;
        
        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);
        
    }
    

    

}
