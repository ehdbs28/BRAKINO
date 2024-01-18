using UnityEngine;

public class Player : Entity
{
    [SerializeField] private InputReader _inputReader;
    public InputReader InputReader => _inputReader;

    public override void Awake()
    {
        base.Awake();
        StateController.RegisterState(new PlayerIdleState(StateController, "Idle"));
        StateController.RegisterState(new PlayerMovementState(StateController, "Movement"));
        StateController.ChangeState(typeof(PlayerIdleState));
    }

    public void Rotate(Quaternion targetRot)
    {
        ModelTrm.rotation = Quaternion.Lerp(ModelTrm.rotation, targetRot, Data.rotateSpeed);
    }
}