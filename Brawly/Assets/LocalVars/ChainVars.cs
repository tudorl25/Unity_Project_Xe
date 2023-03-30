using UnityEngine;
public class ChainVars
{
    public static bool exitSlope = false;

    public static Vector3 slopeMovementDir;

    public static bool onSlope;

    public static void UpdateDir(Vector3 sl)
    {
        slopeMovementDir = sl;
    }

    public static void UpdateOnSlope(bool on)
    {
        onSlope = on;
    }
    
}
