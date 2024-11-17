using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")] [SerializeField]
    private float movementSpeed = 5f;

    [SerializeField] private float crouchedSpeed = 2.5f;
    [SerializeField] private float sprintingSpeed = 8f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float weight = 1f;

    [Header("Head Bobbing Settings")] [SerializeField]
    private float bobbingAmount = 0.1f;

    [SerializeField] private float baseBobbingSpeed = 10f;
    private CharacterController controller;
    private float crouchHeight;
    private Vector3 currentMovement;
    private Vector3 initialCameraPosition;
    private PlayerInputManager inputManager;
    private bool isGrounded;
    private float originalHeight;
    private Camera playerCamera;

    private float timeCounter;
    private Vector3 velocity;

    private void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        initialCameraPosition = playerCamera.transform.localPosition;
        originalHeight = controller.height;
        crouchHeight = originalHeight * 0.7f;
    }

    private void Start()
    {
        inputManager.OnJumpEvent += OnJump;
        inputManager.OnCrouchEvent += OnCrouch;
    }

    private void FixedUpdate()
    {
        Move();
        ApplyGravity();
        ApplyHeadBobbing();
    }

    private void OnDestroy()
    {
        inputManager.OnJumpEvent -= OnJump;
        inputManager.OnCrouchEvent -= OnCrouch;
    }

    private void Move()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        var forward = Camera.main.transform.forward;
        var right = Camera.main.transform.right;
        forward.y = 0f;
        right.y = 0f;

        var targetDirection =
            (forward * inputManager.MovementValue.y + right * inputManager.MovementValue.x).normalized;
        var targetSpeed = inputManager.IsSprinting ? sprintingSpeed :
            inputManager.IsCrouching ? crouchedSpeed : movementSpeed;

        var targetMovement = targetDirection * targetSpeed;
        currentMovement = Vector3.Lerp(currentMovement, targetMovement,
            (targetMovement.sqrMagnitude > currentMovement.sqrMagnitude ? acceleration : deceleration) *
            Time.fixedDeltaTime);

        controller.Move(currentMovement * Time.fixedDeltaTime);
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
            velocity.y += gravity * weight * Time.fixedDeltaTime;
        else if (velocity.y < 0) velocity.y = -2f;

        controller.Move(velocity * Time.fixedDeltaTime);
    }

    private void ApplyHeadBobbing()
    {
        if (isGrounded && currentMovement.magnitude > 0)
        {
            var bobbingSpeed = baseBobbingSpeed * (currentMovement.magnitude / movementSpeed);
            timeCounter += Time.fixedDeltaTime * bobbingSpeed;
            var bobbingY = Mathf.Sin(timeCounter) * bobbingAmount;
            playerCamera.transform.localPosition = new Vector3(initialCameraPosition.x,
                initialCameraPosition.y + bobbingY, initialCameraPosition.z);
        }
        else
        {
            playerCamera.transform.localPosition = initialCameraPosition;
        }
    }

    private void OnJump()
    {
        if (isGrounded) velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
    }

    public void OnCrouch()
    {
        if (inputManager.IsCrouching)
        {
            controller.height = crouchHeight;
            playerCamera.transform.localPosition = new Vector3(initialCameraPosition.x,
                initialCameraPosition.y - (originalHeight - crouchHeight) / 2f, initialCameraPosition.z);
        }
        else
        {
            controller.height = originalHeight;
            playerCamera.transform.localPosition = initialCameraPosition;
        }
    }
}