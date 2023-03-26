using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveAroundPlayer : MonoBehaviour
{

    [SerializeField]
    private float mouseSensitivity = 3.0f;

    private float rotationY;
    private float rotationX;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float distanceFromTarget = 5.0f;

    private Vector3 currentRoation;
    private Vector3 smoothVelocity  = Vector3.zero;

    [SerializeField]
    private float maxDistance = 10;
    
    [SerializeField]
    private float minDistance = 3;

    [SerializeField]
    private float smoothTime = 0.2f;

    private MouseCursor mouse;
    
    void Update()
    {
       moveAroundPlayer();
        
    }

    public void moveAroundPlayer()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0 ) // forward
        {
            distanceFromTarget += 2;

            if (distanceFromTarget > maxDistance)
            {
                distanceFromTarget = maxDistance;
            }
        }else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            distanceFromTarget -= 2;

            if (distanceFromTarget < minDistance)
            {
                distanceFromTarget = minDistance;
            }
        }
        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationY += mouseX;
        rotationX += mouseY;

        rotationX = Mathf.Clamp(rotationX, -40, 40);

        Vector3 nextRoation = new Vector3(rotationX, rotationY);
        currentRoation = Vector3.SmoothDamp(currentRoation, nextRoation, ref smoothVelocity,smoothTime);

        transform.localEulerAngles = currentRoation;

        transform.position = target.position - transform.forward * distanceFromTarget;
    }
}
