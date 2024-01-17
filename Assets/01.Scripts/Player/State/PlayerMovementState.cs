using UnityEngine;

public class PlayerMovementState : PlayerBaseState
{
    public PlayerMovementState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
    }

    public override void UpdateState()
    {
        var movementInput = Player.InputReader.movementInput;
        var dir = new Vector3(movementInput.x, 0, movementInput.y);
        
        if (movementInput.sqrMagnitude <= 0.05f)
        {
            Controller.ChangeState(typeof(PlayerIdleState));
            return;
        }
        
        Player.Rotate(Quaternion.LookRotation(dir));
        Player.SetVelocity(dir * Player.Data.movementSpeed);
    }
}