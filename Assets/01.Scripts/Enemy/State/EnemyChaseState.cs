using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
    }

    public override void UpdateState()
    {
        if (!Enemy.Sense(Enemy.EnemyData.senseRadius))
        {
            Controller.ChangeState(typeof(EnemyBattleIdleState));
            return;
        }
        
        if (Enemy.Sense(Enemy.EnemyData.attackRadius + Enemy.EnemyData.attackDistance))
        {
            Controller.ChangeState(typeof(EnemyPrimaryAttackState));
            return;
        }

        var dir = (Enemy.targetPlayer.transform.position - Enemy.transform.position).normalized;
        Enemy.Rotate(Quaternion.LookRotation(dir));
        Enemy.Move(dir);
    }
}