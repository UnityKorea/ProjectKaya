using UnityEngine;
public class Rotate : MonoBehaviour
{
    [Tooltip("Control the object rotate speed each axis")]
    public float Xspeed;
    public float Yspeed;
    public float Zspeed;
    void Update()
    {
        // If relativeTo is not specified or set to Space.Self the rotation is applied around the transform's local axes.
        // If relativeTo is set to Space.World the rotation is applied around the world x, y, z axes.
        transform.Rotate(Time.deltaTime * Xspeed, Time.deltaTime * Yspeed, Time.deltaTime * Zspeed, Space.World);
    }
}