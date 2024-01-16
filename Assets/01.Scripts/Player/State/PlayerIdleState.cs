public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(StateController controller) : base(controller)
    {
    }

    public override void EnterState()
    {
        Owner.StopImmediately();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        var movementInput = Player.InputReader.movementInput;
        if (movementInput.sqrMagnitude >= 0.05f)
        {
            Controller.ChangeState(typeof(PlayerMovementState));
        }
    }

    public override void ExitState()
    {
    }
}