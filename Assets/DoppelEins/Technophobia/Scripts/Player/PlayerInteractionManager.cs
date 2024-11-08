using System;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    private PlayerInputManager inputManager;
    private TerminalManager currentTerminal;
    private CharacterController characterController;
    public bool CanInteractWithTerminal = false;
    public bool IsMovingToTerminalPosition = false;
    public bool CanEnableTerminal = false;

    public Camera mainCamera;


    void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();
        characterController = GetComponent<CharacterController>();
        inputManager.OnInteractionEvent += OnInteract;
    }

    void FixedUpdate()
    {
        if (!IsMovingToTerminalPosition || currentTerminal == null) return;
        MovePlayerToTerminalPosition();
    }

    void OnDestroy()
    {
        inputManager.OnInteractionEvent -= OnInteract;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Terminal") && characterController.isGrounded)
        {
            CanInteractWithTerminal = true;
            currentTerminal = other.GetComponent<TerminalManager>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Terminal"))
        {
            CanInteractWithTerminal = false;
            currentTerminal.EndInteraction();
            currentTerminal = null;
            IsMovingToTerminalPosition = false;
        }
    }

    void OnInteract()
    {
        if (CanInteractWithTerminal)
        {
            IsMovingToTerminalPosition = true;
        }
        if (PlayerInputManager.Instance.IsInteracting)
        {
            PlayerInputManager.Instance.IsInteracting = false;
        }
    }

    void MovePlayerToTerminalPosition()
    {
        // Get the target position with current Y level to limit movement to X and Z only
        Vector3 targetPosition = new Vector3(currentTerminal.playerPosition.position.x, transform.position.y, currentTerminal.playerPosition.position.z);

        // Calculate 2D distance and rotation difference
        float distanceToTarget = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetPosition.x, 0, targetPosition.z));
        float rotationDifference = Quaternion.Angle(transform.rotation, currentTerminal.playerPosition.rotation);

        Debug.Log("DistanceDiff: " + distanceToTarget + " | RotationDiff: " + rotationDifference);

        // Adjusted thresholds for snapping
        float positionThreshold = 0.1f;
        float rotationThreshold = 2.0f;

        // Snap to the target if within the thresholds
        if (distanceToTarget <= positionThreshold && rotationDifference <= rotationThreshold)
        {
            Debug.Log("Reached target. Snapping to exact position and rotation.");
            IsMovingToTerminalPosition = false;

            // Finalize exact position and rotation
            transform.position = targetPosition;
            transform.rotation = currentTerminal.playerPosition.rotation;

            // Ensure camera points at terminal after snapping
            mainCamera.transform.LookAt(currentTerminal.transform);
            currentTerminal.StartInteraction();
            return;
        }

        // Steady movement speed
        float moveSpeed = 250f;
        float rotationSpeed = 300f;

        // Use MoveTowards for smooth movement
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Use RotateTowards for smooth rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, currentTerminal.playerPosition.rotation, rotationSpeed * Time.deltaTime);
    }

}
