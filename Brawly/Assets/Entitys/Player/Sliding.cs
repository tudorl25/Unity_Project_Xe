using UnityEngine;

public class Sliding
{
    public Transform orientation;

    public Transform playerObj;

    public Rigidbody rb;

    public Sliding(Transform orientation, Transform playerObj, Rigidbody rb,float horizontal,float vertical)
    {
        this.orientation = orientation;
        this.playerObj = playerObj;
        this.rb = rb;

        this.horizontal = horizontal;
        this.vertical = vertical;
    }

    public float maxSlideTime = 0.75f;
    public float slideForce = 200f;
    public float sliderTimer;

    public float slideYScale = 0.5f;
    public float startYScale;
    
    public bool sliding;

    public float horizontal;
    public float vertical;

    public bool firstDir = false;

    private Vector3 finalDir;

    public void startSliding()
    {
        sliding = true;

        firstDir = true;

        //playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        sliderTimer = maxSlideTime;
    }

    public void slidingMovement()
    {
     
        Vector3 inputDir = orientation.forward;

        if (!ChainVars.onSlope || rb.velocity.y > -0.1f)
        {

            Vector3 direction = getDirection(/*inputDir*/ playerObj.forward);

            playerObj.forward = direction;

            rb.AddForce(direction.normalized * slideForce, ForceMode.Force);

            sliderTimer -= Time.deltaTime;   
        }
        else
        {
            rb.AddForce(ChainVars.slopeMovementDir * slideForce, ForceMode.Force);
        }

        if (sliderTimer <= 0)
            stopSlide();
    }

    public void stopSlide()
    {
        sliding = false;
        
        firstDir = true;

        //playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }

    Vector3 getDirection(Vector3 dir)
    {
        if (firstDir)
        {
            finalDir = dir;
            firstDir = false;
        }

        return finalDir;
    }
}
