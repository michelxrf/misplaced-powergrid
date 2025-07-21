using UnityEngine;

/// <summary>
/// Rotates the camera in the tittle screen
/// </summary>
public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed = 45f;
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
