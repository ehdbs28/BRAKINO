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
        Player.InputReader.OnRollEvent += RollHandle;
    }

    public override void UpdateState()
    {
        if (Time.time >= _attackEndTime + Player.PlayerData.comboWindowTime)
        {
            Controller.ChangeState(typeof(PlayerIdleState));
            Player.PlayerAttackComboCounter = 0;
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        Player.InputReader.OnPrimaryAttackEvent -= PrimaryAttackHandle;
        Player.InputReader.OnRollEvent -= RollHandle;
    }
}
