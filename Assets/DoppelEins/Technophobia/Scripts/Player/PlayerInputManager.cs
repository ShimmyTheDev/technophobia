using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour, PlayerControls.IPlayerActions
{
    public static PlayerInputManager Instance;
    public Vector2 MovementValue { get; private set; }
    public Vector2 LookValue { get; private set; }
    public bool IsSprinting { get; private set; }
    public bool IsCrouching { get; private set; }
    public bool IsInteracting { get; set; }
    public event Action OnInteractionEvent;
    public event Action OnSprintEvent;
    public event Action OnCrouchEvent;
    public event Action OnJumpEvent;
    public event Action OnInputsEvent;
    public string lastKeystroke = "";

    private PlayerControls controls;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        controls = new PlayerControls();
        controls.Player.SetCallbacks(this);
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsCrouching = true;
            OnCrouchEvent.Invoke();
        }
        else if (context.canceled)
        {
            IsCrouching = false;
            OnCrouchEvent.Invoke();
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        OnInteractionEvent?.Invoke();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (IsInteracting) return;
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsSprinting = true;
        }
        else if (context.canceled)
        {
            IsSprinting = false;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (IsInteracting) return;
        LookValue = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        OnJumpEvent?.Invoke();
    }

    public void OnInputs(InputAction.CallbackContext context)
    {
        if (!context.performed && !IsInteracting) return;
        lastKeystroke = context.control.name;
    }
}
