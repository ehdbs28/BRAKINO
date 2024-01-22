using UnityEngine;

public class PlayerParryingState : PlayerBaseState
{
    public PlayerParryingState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        Player.OnlyUseBaseAnimatorLayer();
        Player.InputReader.OnRollEvent += RollHandle;
    }

    public override void UpdateState()
    {
        if (_animationEndTriggerCalled)
        {
            Controller.ChangeState(typeof(PlayerIdleState));
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        Player.InputReader.OnRollEvent -= RollHandle;
    }
}