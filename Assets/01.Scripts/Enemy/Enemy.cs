using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    [SerializeField] private LayerMask _enemyMask;
    
    public EnemyData EnemyData => (EnemyData)Data;
    
    public NavMeshAgent NavAgent { get; private set; }

    public override void Awake()
    {
        base.Awake();

        NavAgent = GetComponent<NavMeshAgent>();
        
        StateController.RegisterState(new EnemyIdleState(StateController, "Idle"));
        StateController.RegisterState(new EnemyChaseState(StateController, "Chase"));
        StateController.RegisterState(new EnemyPrimaryAttackState(StateController, "PrimaryAttack"));
        StateController.RegisterState(new EnemyHitState(StateController, "Hit"));
        StateController.RegisterState(new EnemyDizzyState(StateController, "Dizzy"));
        StateController.RegisterState(new EnemyDieState(StateController, "Die"));
        StateController.ChangeState(typeof(EnemyIdleState));
    }
}