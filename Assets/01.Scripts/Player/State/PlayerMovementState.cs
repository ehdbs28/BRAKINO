using UnityEngine;

public class PlayerMovementState : PlayerBaseState
{
    public PlayerMovementState(StateController controller) : base(controller)
    {
    }

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        base.UpdateState();
        var movementInput = Player.InputReader.movementInput;
        if (movementInput.sqrMagnitude <= 0.05f)
        {
            Controller.ChangeState(typeof(PlayerIdleState));
            return;
        }
        
        Owner.SetVelocity(new Vector3(movementInput.x, 0, movementInput.y) * Owner.Data.movementSpeed);
    }

    public override void ExitState()
    {
    }
}