using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    public bool CanInteractWithTerminal;
    public bool IsMovingToTerminalPosition;
    public bool CanEnableTerminal;

    public Camera mainCamera;
    private CharacterController characterController;
    private TerminalManager currentTerminal;
    private PlayerInputManager inputManager;


    private void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();
        characterController = GetComponent<CharacterController>();
        inputManager.OnInteractionEvent += OnInteract;
    }

    private void FixedUpdate()
    {
        if (!IsMovingToTerminalPosition || currentTerminal == null) return;
        MovePlayerToTerminalPosition();
    }

    private void OnDestroy()
    {
        inputManager.OnInteractionEvent -= OnInteract;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Terminal") && characterController.isGrounded)
        {
            CanInteractWithTerminal = true;
            currentTerminal = other.GetComponent<TerminalManager>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Terminal") && currentTerminal != null)
        {
            CanInteractWithTerminal = false;
            currentTerminal.EndInteraction();
            currentTerminal = null;
            IsMovingToTerminalPosition = false;
        }
    }

    private void OnInteract()
    {
        if (CanInteractWithTerminal) IsMovingToTerminalPosition = true;
        if (PlayerInputManager.Instance.IsInteracting)
        {
            PlayerInputManager.Instance.IsInteracting = false;
            CanInteractWithTerminal = false;
            currentTerminal.EndInteraction();
            currentTerminal = null;
            IsMovingToTerminalPosition = false;
        }
    }

    private void MovePlayerToTerminalPosition()
    {
        // Get the target position with current Y level to limit movement to X and Z only
        var targetPosition = new Vector3(currentTerminal.playerPosition.position.x, transform.position.y,
            currentTerminal.playerPosition.position.z);

        // Calculate 2D distance and rotation difference
        var distanceToTarget = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
            new Vector3(targetPosition.x, 0, targetPosition.z));
        var rotationDifference = Quaternion.Angle(transform.rotation, currentTerminal.playerPosition.rotation);

        // Adjusted thresholds for snapping
        var positionThreshold = 0.1f;
        var rotationThreshold = 2.0f;

        // Snap to the target if within the thresholds
        if (distanceToTarget <= positionThreshold && rotationDifference <= rotationThreshold)
        {
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
        var moveSpeed = 250f;
        var rotationSpeed = 300f;

        // Use MoveTowards for smooth movement
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Use RotateTowards for smooth rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, currentTerminal.playerPosition.rotation,
            rotationSpeed * Time.deltaTime);
    }
}