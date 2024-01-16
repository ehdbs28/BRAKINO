using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReader : ScriptableObject, InputControl.IPlayerActions
{
    public delegate void InputEventListener();
    public delegate void InputEventListener<in T>(T value);

    public Vector2 movementInput;
    public Vector2 screenPos;

    private InputControl _inputControl;

    private void OnEnable()
    {
        if (_inputControl is null)
        {
            _inputControl = new InputControl();
            _inputControl.Player.SetCallbacks(this);
        }
        
        _inputControl.Player.Enable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnScreenPos(InputAction.CallbackContext context)
    {
        screenPos = context.ReadValue<Vector2>();
    }
}