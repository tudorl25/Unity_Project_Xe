using UnityEngine;

public class FollowToPosition : MonoBehaviour
{
    public Transform start;

    public float x;
    public float y;
    public float z;

    public Rigidbody rb;

    public void setPosition(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    
    /// <summary>
    /// Not final it needs more polishing
    /// </summary>
  public void followPosition()
    {
        Vector3 viewDir = start.position - new Vector3(x, y, z).normalized;

        transform.forward = viewDir;
        
        //shit drivves it to the moon
        rb.AddForce(viewDir * 10f, ForceMode.Force);
            
    }


}
