public class EnemyBattleIdleState : EnemyBaseState
{
    public EnemyBattleIdleState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Enemy.StopImmediately();
    }

    public override void UpdateState()
    {
        if (Enemy.Sense(Enemy.EnemyData.senseRadius))
        {
            Controller.ChangeState(typeof(EnemyChaseState));
        }
    }
}