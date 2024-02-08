using UnityEngine;

public class EnemyPrimaryAttackState : EnemyBaseState
{
    public EnemyPrimaryAttackState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Enemy.StopImmediately();
    }

    public override void UpdateState()
    {
        if (_animationTriggerCalled)
        {
            Controller.ChangeState(typeof(EnemyBattleIdleState));
        }
    }

    private void Attack()
    {
        Debug.Log("attack");
    }
    
    public override void AnimationTrigger(string eventKey)
    {
        // if (eventKey == "TrailOn")
        // {
        //     _swordTrail.enabled = true;
        // }
        if (eventKey == "Attack")
        {
            Attack();
        }
        // else if (eventKey == "TrailOff")
        // {
        //     _swordTrail.enabled = false;
        // }
        else if(eventKey == "AttackEnd")
        {
            // _triggerCalledTime = Time.time;
            base.AnimationTrigger(eventKey);
        }
    }
}