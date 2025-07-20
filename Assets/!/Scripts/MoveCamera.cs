using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lookSpeed = 2f;

    public GameObject upperLimit;
    public GameObject lowerLimit;

    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (LevelManager.Instance.isPaused || LevelManager.Instance.isGameover)
            return;

        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        if (Input.GetMouseButton(1)) // Right mouse button
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -75f, 75f);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal"); // A/D
        float moveZ = Input.GetAxis("Vertical");   // W/S
        float moveY = 0f;

        Vector3 move = transform.right * moveX + transform.up * moveY + transform.forward * moveZ;
        transform.position += move * moveSpeed * Time.deltaTime;

        ClampMovement();
    }

    void ClampMovement()
    {
        float clampedx = Mathf.Clamp(transform.position.x, Mathf.Min(upperLimit.transform.position.x, lowerLimit.transform.position.x), Mathf.Max(upperLimit.transform.position.x, lowerLimit.transform.position.x));
        float clampedy = Mathf.Clamp(transform.position.y, Mathf.Min(upperLimit.transform.position.y, lowerLimit.transform.position.y), Mathf.Max(upperLimit.transform.position.y, lowerLimit.transform.position.y));
        float clampedz = Mathf.Clamp(transform.position.z, Mathf.Min(upperLimit.transform.position.z, lowerLimit.transform.position.z), Mathf.Max(upperLimit.transform.position.z, lowerLimit.transform.position.z));

        transform.position = new Vector3(clampedx, clampedy, clampedz);
    }
}
