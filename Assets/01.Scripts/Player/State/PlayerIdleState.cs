public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Owner.StopImmediately();
    }

    public override void UpdateState()
    {
        var movementInput = Player.InputReader.movementInput;
        if (movementInput.sqrMagnitude >= 0.05f)
        {
            Controller.ChangeState(typeof(PlayerMovementState));
        }
    }
}