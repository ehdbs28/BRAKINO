using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [SerializeField] private PlayerInputReader _inputReader;
    public PlayerInputReader InputReader => _inputReader;

    [SerializeField] private float _layerTransitionTime = 0.1f;
    
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _enemyMask;

    public PlayerData PlayerData => (PlayerData)Data;

    private readonly int _shieldHash = Animator.StringToHash("Shield");
    
    public int PlayerAttackComboCounter { get; set; }
    public bool OnShieldState { get; private set; }

    private Coroutine _runningRoutine;
    
    #region Gizmos Control Variable
    #if UNITY_EDITOR
    [Space(10)] [Header("For Gizmos")]
    [SerializeField] private bool _drawAttackRange;
    [SerializeField] private bool _drawBlockRange;
    #endif    
    #endregion

    public override void Awake()
    {
        base.Awake();
        PlayerAttackComboCounter = 0;
        StateController.RegisterState(new PlayerIdleState(StateController, "Idle"));
        StateController.RegisterState(new PlayerMovementState(StateController, "Movement"));
        StateController.RegisterState(new PlayerPrimaryAttackState(StateController, "PrimaryAttack"));
        StateController.RegisterState(new PlayerPrimaryAttackEndState(StateController, "PrimaryAttackEnd"));
        StateController.RegisterState(new PlayerRollState(StateController, "Roll"));
        StateController.RegisterState(new PlayerParryingState(StateController, "Parrying"));
        StateController.ChangeState(typeof(PlayerIdleState));
    }

    public override void Update()
    {
        base.Update();
        if (OnShieldState)
        {
            RotateToMousePoint();
        }
    }

    public override void OnDamage(float damage, Vector3 attackedDir)
    {
        if (OnShieldState && damage < PlayerData.shieldCanBlockDamage && IsBlock(attackedDir))
        {
            // block
            Debug.Log("Block!!");
        }
        else
        {
            base.OnDamage(damage, attackedDir);
        }
    }

    private bool IsBlock(Vector3 attackDir)
    {
        var dot = Vector3.Dot(-attackDir, ModelTrm.forward);
        var theta = Mathf.Acos(dot);
        var degree = theta * Mathf.Rad2Deg;
        return degree <= PlayerData.shieldCanBlockAngle / 2f;
    }

    public void ActivateShield(bool active)
    {
        OnShieldState = active;
        AnimatorCompo.SetBool(_shieldHash, OnShieldState);   
    }

    public void OnlyUseBaseAnimatorLayer()
    {
        if (_runningRoutine is not null)
        {
            StopCoroutine(_runningRoutine);
        }
        _runningRoutine = StartCoroutine(SetAnimatorLayerWeightRoutine(0));
    }

    public void UseAllAnimatorLayer()
    {
        if (_runningRoutine is not null)
        {
            StopCoroutine(_runningRoutine);
        }
        _runningRoutine = StartCoroutine(SetAnimatorLayerWeightRoutine(1));
    }

    private IEnumerator SetAnimatorLayerWeightRoutine(float value)
    {
        for (var i = 1; i < AnimatorCompo.layerCount; i++)
        {
            if (Math.Abs(AnimatorCompo.GetLayerWeight(i) - value) < 0.01f)
            {
                yield break;
            }
        }
        
        var current = 0f;

        while (current < _layerTransitionTime)
        {
            current += Time.deltaTime;
            var percent = current / _layerTransitionTime;
            
            for (var i = 1; i < AnimatorCompo.layerCount; i++)
            {
                AnimatorCompo.SetLayerWeight(i, 1f - Mathf.Abs(value - percent));
            }

            yield return null;
        }
        
        for (var i = 1; i < AnimatorCompo.layerCount; i++)
        {
            AnimatorCompo.SetLayerWeight(i, value);
        }
    }

    private void RotateToMousePoint()
    {
        var screenPos = InputReader.screenPos;
        var cameraRay = CameraManager.Instance.MainCam.ScreenPointToRay(screenPos);
        var rayDistance = CameraManager.Instance.MainCam.farClipPlane;
    
        var isHit = Physics.Raycast(cameraRay, out var hit, rayDistance, _groundMask);
        
        if (isHit)
        {
            var dir = hit.point - transform.position;
            dir.y = 0;
            dir.Normalize();
    
            var lookRotation = Quaternion.LookRotation(dir);
            
            Rotate(lookRotation);
        }
    }

    public List<IDamageable> GetDamageableObjects(out List<Vector3> points)
    {
        var cols = new Collider[PlayerData.maxAttackCount];
        var result = new List<IDamageable>();
        var center = transform.position + CharacterControllerCompo.center + ModelTrm.forward * PlayerData.attackDistance;
        var count = Physics.OverlapSphereNonAlloc(center, PlayerData.attackRadius, cols, _enemyMask);

        points = new List<Vector3>();
        for (var i = 0; i < count; i++)
        {
            if (cols[i].TryGetComponent<IDamageable>(out var damageable))
            {
                points.Add(cols[i].ClosestPointOnBounds(center));
                result.Add(damageable);
            }
        }

        return result;
    }
    
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (Data is null)
        {
            return;
        }

        if (_drawAttackRange)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(
                transform.position + GetComponent<CharacterController>().center +
                transform.Find("Model").forward * PlayerData.attackDistance,
                PlayerData.attackRadius
            );
        }

        if (_drawBlockRange)
        {
            if (OnShieldState)
            {
                Gizmos.color = Color.yellow;
                MoreGizmos.DrawWireArc(ModelTrm.position, ModelTrm.forward, PlayerData.shieldCanBlockAngle, 3f);
            }
        }
    }

#endif
}