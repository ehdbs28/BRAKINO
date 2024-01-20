using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReader : ScriptableObject, InputControl.IPlayerActions
{
    public delegate void InputEventListener();
    public delegate void InputEventListener<in T>(T value);

    public event InputEventListener OnPrimaryAttackEvent = null;
    public event InputEventListener OnRollEvent = null;
    
    public Vector3 movementInput;
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
        movementInput.z = movementInput.y;
        movementInput.y = 0;
    }

    public void OnScreenPos(InputAction.CallbackContext context)
    {
        screenPos = context.ReadValue<Vector2>();
    }

    public void OnPrimaryAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnPrimaryAttackEvent?.Invoke();
        }
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnRollEvent?.Invoke();
        }
    }
}
