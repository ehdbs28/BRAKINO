using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float _senseTimer;
    
    public EnemyIdleState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        _senseTimer = 0f;
    }

    public override void UpdateState()
    {
        if (Enemy.Sense(Enemy.EnemyData.senseRadius))
        {
            _senseTimer += Time.deltaTime;
            if (_senseTimer >= Enemy.EnemyData.senseDelay)
            {
                Controller.ChangeState(typeof(EnemyBattleIdleState));
            }
        }
        else
        {
            _senseTimer = 0f;
        }
    }

    
}