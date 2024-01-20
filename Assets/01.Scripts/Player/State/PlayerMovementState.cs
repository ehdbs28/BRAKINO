using UnityEngine;

public class PlayerMovementState : PlayerBaseState
{
    public PlayerMovementState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Player.InputReader.OnPrimaryAttackEvent += PrimaryAttackHandle;
        Player.InputReader.OnRollEvent += RollHandle;
    }

    public override void UpdateState()
    {
        var inputDir = Player.InputReader.movementInput;
        
        if (inputDir.sqrMagnitude <= 0.05f)
        {
            Controller.ChangeState(typeof(PlayerIdleState));
            return;
        }
        
        Player.Rotate(Quaternion.LookRotation(inputDir));
        Player.Move(inputDir * Player.Data.movementSpeed);
    }

    public override void ExitState()
    {
        base.ExitState();
        Player.InputReader.OnPrimaryAttackEvent -= PrimaryAttackHandle;
        Player.InputReader.OnRollEvent -= RollHandle;
    }
}