using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    [SerializeField] private LayerMask _playerMask;

    public EnemyData EnemyData => (EnemyData)Data;

    public Player targetPlayer;
    public NavMeshAgent NavAgent { get; private set; }
    
    #region Gizmos Control Variable
    #if UNITY_EDITOR
    [Space(10)] [Header("For Gizmos")]
    [SerializeField] private bool _drawSenseCircle;
    [SerializeField] private bool _drawAttackRange;
    #endif    
    #endregion

    public override void Awake()
    {
        base.Awake();

        targetPlayer = null;
        NavAgent = GetComponent<NavMeshAgent>();
        NavAgent.speed = EnemyData.movementSpeed;
        
        StateController.RegisterState(new EnemyIdleState(StateController, "Idle"));
        StateController.RegisterState(new EnemyBattleIdleState(StateController, "BattleIdle"));
        StateController.RegisterState(new EnemyChaseState(StateController, "Chase"));
        StateController.RegisterState(new EnemyPrimaryAttackState(StateController, "PrimaryAttack"));
        StateController.RegisterState(new EnemyHitState(StateController, "Hit"));
        StateController.RegisterState(new EnemyDizzyState(StateController, "Dizzy"));
        StateController.RegisterState(new EnemyDieState(StateController, "Die"));
        StateController.ChangeState(typeof(EnemyIdleState));
    }

    public override void Move(Vector3 velocity)
    {
        if (NavAgent.isStopped)
        {
            NavAgent.isStopped = false;
        }
        NavAgent.Move(velocity * Time.deltaTime);
    }

    public override void StopImmediately()
    {
        NavAgent.isStopped = true;
        NavAgent.SetDestination(transform.position);
    }

    public bool Sense(float senseRadius)
    {
        var cols = new Collider[EnemyData.maxSenseCnt];
        var center = transform.position + CharacterControllerCompo.center;
        var count = Physics.OverlapSphereNonAlloc(center, senseRadius, cols, _playerMask);

        if (count > 0)
        {
            targetPlayer = cols[0].GetComponent<Player>();
        }
        return count > 0;
    }
    
    public List<Entity> GetCanAttackEntities(out List<Vector3> points)
    {
        var cols = new Collider[EnemyData.maxAttackCnt];
        var result = new List<Entity>();
        var center = transform.position + CharacterControllerCompo.center + ModelTrm.forward * EnemyData.attackDistance;
        var count = Physics.OverlapSphereNonAlloc(center, EnemyData.attackRadius, cols, _playerMask);

        points = new List<Vector3>();
        for (var i = 0; i < count; i++)
        {
            if (cols[i].TryGetComponent<Entity>(out var entity))
            {
                points.Add(cols[i].ClosestPointOnBounds(center));
                result.Add(entity);
            }
        }

        return result;
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (EnemyData is null)
        {
            return;
        }

        if (_drawSenseCircle)
        {
            Gizmos.color = Color.red;
            MoreGizmos.DrawWireCircle(transform.position, EnemyData.senseRadius);
        }
        
        if (_drawAttackRange)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(
                transform.position + GetComponent<CharacterController>().center +
                transform.Find("Model").forward * EnemyData.attackDistance,
                EnemyData.attackRadius
            );
        }
    }

#endif
}