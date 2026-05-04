using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    // Getter & Setter
    public static GameInput Instance { get; private set; }
    public Vector2 MovementInput { get; private set; }
    
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
    }

    // New control mechanics
    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        MovementInput = ctx.ReadValue<Vector2>();
    }
    private void OnMoveCancled(InputAction.CallbackContext ctx)
    {
        MovementInput = Vector2.zero;
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
