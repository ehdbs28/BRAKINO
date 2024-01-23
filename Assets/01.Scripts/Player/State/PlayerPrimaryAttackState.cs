using System.Collections;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerBaseState
{
    private Vector3 _attackDir;

    private float _triggerCalledTime;
    
    private readonly int _comboCounterHash;
    private readonly LayerMask _groundMask;

    private Coroutine _runningRoutine;
    
    public PlayerPrimaryAttackState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
        _comboCounterHash = Animator.StringToHash("ComboCounter");
        _groundMask = LayerMask.GetMask("Ground");
    }

    public override void EnterState()
    {
        base.EnterState();
        Player.OnlyUseBaseAnimatorLayer();
        
        Player.InputReader.OnRollEvent += RollHandle;

        _attackDir = GetAttackDir();
        Player.Rotate(Quaternion.LookRotation(_attackDir), 1f);

        if (Player.PlayerAttackComboCounter > 2)
        {
            Player.PlayerAttackComboCounter = 0;
        }
        Player.AnimatorCompo.SetFloat(_comboCounterHash, Player.PlayerAttackComboCounter);

        if (_runningRoutine is not null)
        {
            Player.StopCoroutine(_runningRoutine);
        }
        _runningRoutine = Player.StartCoroutine(AdvanceRoutine(0.2f));
        
        Attack();
    }

    public override void UpdateState()
    {
        if (_animationTriggerCalled)
        {
            if (Time.time >= _triggerCalledTime + Player.PlayerData.attackDelayTimes[Player.PlayerAttackComboCounter])
            {
                Controller.ChangeState(typeof(PlayerPrimaryAttackEndState));
            }
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        ++Player.PlayerAttackComboCounter;
        Player.InputReader.OnRollEvent -= RollHandle;
    }

    private void Attack()
    {
        var entities = Player.GetCanAttackAEntities();
        foreach (var entity in entities)
        {
            entity.OnDamage(Player.PlayerData.damage);
        }
    }

    private IEnumerator AdvanceRoutine(float time)
    {
        var currentAttackMovement = Player.PlayerData.attackAdvances[Player.PlayerAttackComboCounter];

        var force = Quaternion.LookRotation(_attackDir) * currentAttackMovement;

        var currentTime = 0f;
        while (currentTime <= time)
        {
            currentTime += Time.deltaTime;
            Player.Move(force);
            yield return null;
        }
        Player.StopImmediately();
    }

    private Vector3 GetAttackDir()
    {
        var screenPos = Player.InputReader.screenPos;
        var ray = CameraManager.Instance.MainCam.ScreenPointToRay(screenPos);
        var rayDistance = CameraManager.Instance.MainCam.farClipPlane;
        var isHit = Physics.Raycast(ray, out var hit, rayDistance, _groundMask);

        if (isHit)
        {
            var attackDir = hit.point - Player.transform.position;
            attackDir.y = 0;
            return attackDir.normalized;
        }
        else
        {
            return Player.transform.forward;
        }
    }

    public override void AnimationTrigger()
    {
        _triggerCalledTime = Time.time;
        base.AnimationTrigger();
    }
}