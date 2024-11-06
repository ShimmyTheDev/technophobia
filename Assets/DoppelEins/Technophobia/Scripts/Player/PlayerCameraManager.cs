using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    [SerializeField] Transform cameraHolder;
    [SerializeField] float mouseSensitivity = 1f;

    private float xRotation = 0f;

    private PlayerInputManager inputManager;

    void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();
    }

    void Update()
    {
        RotateCamera();
    }

    void RotateCamera()
    {
        Vector2 lookInput = inputManager.LookValue;

        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
