using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    // Getter & Setter
    public static GameInput Instance { get; private set; }
    public Vector2 MovementInput { get; private set; }

    // Declaring fields
    public event System.Action OnInteract;
    private GameControls controls;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        controls = new GameControls();
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCancled;
        controls.Player.Interact.performed += OnInteractPerfomed;
    }

    // New inpuit system
    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        MovementInput = ctx.ReadValue<Vector2>();
    }
    private void OnMoveCancled(InputAction.CallbackContext ctx)
    {
        MovementInput = Vector2.zero;
    }
    private void OnInteractPerfomed(InputAction.CallbackContext ctx)
    {
        if (OnInteract != null)
            OnInteract();
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}
