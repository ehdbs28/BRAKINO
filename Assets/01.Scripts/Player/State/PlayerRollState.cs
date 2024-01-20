using UnityEngine;

public class PlayerRollState : PlayerBaseState
{
    private Vector3 _rollDirection;
    private float _rollStartTime;

    public PlayerRollState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        var inputDir = Player.InputReader.movementInput;
        _rollDirection = inputDir.sqrMagnitude >= 0.05f ? inputDir : Player.transform.forward;
        Player.Rotate(Quaternion.LookRotation(_rollDirection), 1);
        _rollStartTime = Time.time;
    }

    public override void UpdateState()
    {
        if (Time.time < _rollStartTime + Player.PlayerData.rollTime)
        {
            Player.Move(_rollDirection * Player.PlayerData.rollSpeed);
        }
        else
        {
            Controller.ChangeState(typeof(PlayerIdleState));
        }
    }
}