using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReader : ScriptableObject, InputControl.IPlayerActions
{
    public delegate void InputEventListener();
    public delegate void InputEventListener<in T>(T value);

    public event InputEventListener<Vector2> OnMovementEvent = null;

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
        var val = context.ReadValue<Vector2>();
        OnMovementEvent?.Invoke(val);
    }
}
