using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float MouseSensitivity = 100f;
    public Transform playerBody;
    public Transform playerCamera;
    float xRotation = 0f;


    private PlayerInput playerInput;
    private InputAction mouseLook;


    

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        mouseLook = playerInput.actions["Mouse look"];
    }


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 mouse = mouseLook.ReadValue<Vector2>();
        float mouseX = mouse.x * MouseSensitivity * Time.deltaTime;
        float mouseY = mouse.y * MouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotate the camera (this transform) vertically
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player body horizontally
        playerBody.Rotate(Vector3.up * mouseX);

    }
}
