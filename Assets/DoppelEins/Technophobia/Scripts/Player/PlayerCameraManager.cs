using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float mouseSensitivity = 1f;

    private PlayerInputManager inputManager;

    private float xRotation;

    private void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();
    }

    private void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        var lookInput = inputManager.LookValue;

        var mouseX = lookInput.x * mouseSensitivity;
        var mouseY = lookInput.y * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}