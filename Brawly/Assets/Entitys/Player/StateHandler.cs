using UnityEngine;

public class StateHandler
{
    public enum States
    {
        walking,
        sprinting,
        crouching,
        sliding,
        air
    }

    public States state = States.walking;

    public void updateStates()
    {
        //if(Input.GetKey())
    }


}
