using UnityEngine;

public class PlayerPrimaryAttackEndState : PlayerBaseState
{
    private float _attackEndTime;
    
    public PlayerPrimaryAttackEndState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        _attackEndTime = Time.time;
        Player.InputReader.OnPrimaryAttackEvent += PrimaryAttackHandle;
    }

    public override void UpdateState()
    {
        if (Time.time >= _attackEndTime + Player.PlayerData.attackEndDelay)
        {
            Controller.ChangeState(typeof(PlayerIdleState));
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        Player.InputReader.OnPrimaryAttackEvent -= PrimaryAttackHandle;
    }
}
