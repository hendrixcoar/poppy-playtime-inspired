using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Mouse Look")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookAngle = 90f;
    
    private float xRotation = 0f;
    private float mouseX;
    private float mouseY;
    
    [Header("FOV")]
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float sprintFOV = 75f;
    private Camera playerCamera;
    
    private void Start()
    {
        playerCamera = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        GetMouseInput();
        Look();
        UpdateFOV();
    }

    private void GetMouseInput()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
    }

    private void Look()
    {
        transform.parent.Rotate(Vector3.up * mouseX);
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    private void UpdateFOV()
    {
        float targetFOV = Input.GetKey(KeyCode.LeftShift) ? sprintFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * 8f);
    }
}
