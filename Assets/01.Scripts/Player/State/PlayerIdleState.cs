public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        
        Owner.StopImmediately();
        Player.UseAllAnimatorLayer();
        
        Player.InputReader.OnAttackEvent += AttackHandle;
        Player.InputReader.OnRollEvent += RollHandle;
        Player.InputReader.OnShieldEvent += Player.ActivateShield;
    }

    public override void UpdateState()
    {
        var movementInput = Player.InputReader.movementInput;
        if (movementInput.sqrMagnitude >= 0.05f)
        {
            Controller.ChangeState(typeof(PlayerMovementState));
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        Player.InputReader.OnAttackEvent -= AttackHandle;
        Player.InputReader.OnRollEvent -= RollHandle;
        Player.InputReader.OnShieldEvent -= Player.ActivateShield;
    }
}