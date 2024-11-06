using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInputManager inputManager;
    private CharacterController controller;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float crouchedSpeed = 2.5f;
    [SerializeField] private float sprintingSpeed = 8f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float weight = 1f;

    [Header("Head Bobbing Settings")]
    [SerializeField] private float bobbingAmount = 0.1f;
    [SerializeField] private float baseBobbingSpeed = 10f;
    private Vector3 velocity;
    private Vector3 currentMovement;
    private bool isGrounded;

    private float timeCounter = 0f;
    private Vector3 initialCameraPosition;
    private Camera playerCamera;
    private float originalHeight;
    private float crouchHeight;
    void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        initialCameraPosition = playerCamera.transform.localPosition;
        originalHeight = controller.height;
        crouchHeight = originalHeight * 0.7f;
    }

    void Start()
    {
        inputManager.OnJumpEvent += OnJump;
        inputManager.OnCrouchEvent += OnCrouch;
    }

    void FixedUpdate()
    {
        Move();
        ApplyGravity();
        ApplyHeadBobbing();
    }

    void Move()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0f;
        right.y = 0f;

        Vector3 targetDirection = (forward * inputManager.MovementValue.y + right * inputManager.MovementValue.x).normalized;
        float targetSpeed = inputManager.IsSprinting ? sprintingSpeed : (inputManager.IsCrouching ? crouchedSpeed : movementSpeed);

        Vector3 targetMovement = targetDirection * targetSpeed;
        currentMovement = Vector3.Lerp(currentMovement, targetMovement, (targetMovement.sqrMagnitude > currentMovement.sqrMagnitude ? acceleration : deceleration) * Time.fixedDeltaTime);

        controller.Move(currentMovement * Time.fixedDeltaTime);
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            velocity.y += gravity * weight * Time.fixedDeltaTime;
        }
        else if (velocity.y < 0)
        {
            velocity.y = -2f;
        }

        controller.Move(velocity * Time.fixedDeltaTime);
    }

    void ApplyHeadBobbing()
    {
        if (isGrounded && currentMovement.magnitude > 0)
        {
            float bobbingSpeed = baseBobbingSpeed * (currentMovement.magnitude / movementSpeed);
            timeCounter += Time.fixedDeltaTime * bobbingSpeed;
            float bobbingY = Mathf.Sin(timeCounter) * bobbingAmount;
            playerCamera.transform.localPosition = new Vector3(initialCameraPosition.x, initialCameraPosition.y + bobbingY, initialCameraPosition.z);
        }
        else
        {
            playerCamera.transform.localPosition = initialCameraPosition;
        }
    }

    void OnJump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }
    public void OnCrouch()
    {
        if (inputManager.IsCrouching)
        {
            controller.height = crouchHeight;
            playerCamera.transform.localPosition = new Vector3(initialCameraPosition.x, initialCameraPosition.y - (originalHeight - crouchHeight) / 2f, initialCameraPosition.z);
        }
        else
        {
            controller.height = originalHeight;
            playerCamera.transform.localPosition = initialCameraPosition;
        }
    }
    void OnDestroy()
    {
        inputManager.OnJumpEvent -= OnJump;
        inputManager.OnCrouchEvent -= OnCrouch;
    }
}
