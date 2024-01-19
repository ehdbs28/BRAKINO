using UnityEngine;

public class Player : Entity
{
    [SerializeField] private InputReader _inputReader;
    public InputReader InputReader => _inputReader;

    public PlayerData PlayerData => (PlayerData)Data;

    public override void Awake()
    {
        base.Awake();
        StateController.RegisterState(new PlayerIdleState(StateController, "Idle"));
        StateController.RegisterState(new PlayerMovementState(StateController, "Movement"));
        StateController.RegisterState(new PlayerPrimaryAttackState(StateController, "PrimaryAttack"));
        StateController.ChangeState(typeof(PlayerIdleState));
    }

    public void Rotate(Quaternion targetRot, float speed = -1)
    {
        ModelTrm.rotation = Quaternion.Lerp(ModelTrm.rotation, targetRot, speed < 0 ? Data.rotateSpeed : speed);
    }

    public void AnimationEndTrigger()
    {
        StateController.CurrentState.AnimationEndTrigger();
    }
}