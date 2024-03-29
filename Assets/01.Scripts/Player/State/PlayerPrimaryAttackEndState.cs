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
        Player.InputReader.OnAttackEvent += AttackHandle;
        Player.InputReader.OnRollEvent += RollHandle;
    }

    public override void UpdateState()
    {
        if (Time.time >= _attackEndTime + Player.PlayerData.comboWindowTime)
        {
            Player.PlayerAttackComboCounter = 0;
            Controller.ChangeState(typeof(PlayerIdleState));
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        Player.InputReader.OnAttackEvent -= AttackHandle;
        Player.InputReader.OnRollEvent -= RollHandle;
    }

    protected override void RollHandle()
    {
        Player.PlayerAttackComboCounter = 0;
        base.RollHandle();
    }
}
