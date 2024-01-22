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

    public void Rotate(Quaternion targetRot, float speed = -1)
    {
        ModelTrm.rotation = Quaternion.Lerp(ModelTrm.rotation, targetRot, speed < 0 ? Data.rotateSpeed : speed);
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

    public List<Entity> GetCanAttackAEntities()
    {
        var cols = new Collider[PlayerData.maxAttackCount];
        var result = new List<Entity>();
        var center = transform.position + CharacterControllerCompo.center + ModelTrm.forward * PlayerData.attackDistance;
        var count = Physics.OverlapSphereNonAlloc(center, PlayerData.attackRadius, cols, _enemyMask);
        
        for (var i = 0; i < count; i++)
        {
            if (cols[i].TryGetComponent<Entity>(out var entity))
            {
                result.Add(entity);
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
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position + GetComponent<CharacterController>().center + transform.Find("Model").forward * PlayerData.attackDistance,
            PlayerData.attackRadius
        );
    }

#endif
}