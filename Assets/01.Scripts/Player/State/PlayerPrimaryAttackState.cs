using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerBaseState
{
    private Vector3 _attackDir;

    private float _triggerCalledTime;
    
    private readonly int _comboCounterHash;
    private readonly LayerMask _groundMask;
    private readonly TrailRenderer _swordTrail;
    
    private Coroutine _runningRoutine;

    public PlayerPrimaryAttackState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
        _comboCounterHash = Animator.StringToHash("ComboCounter");
        _groundMask = LayerMask.GetMask("Ground");
        _swordTrail = Player.ModelTrm
            .Find("Mini Simple Skeleton Armature/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/RightHand_Dummy/SwordWood/SwordTrail")
            .GetComponent<TrailRenderer>();
        _swordTrail.enabled = false;
    }

    public override void EnterState()
    {
        base.EnterState();
        Player.OnlyUseBaseAnimatorLayer();
        _swordTrail.enabled = false;
        
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
        _swordTrail.enabled = false;
        ++Player.PlayerAttackComboCounter;
        Player.InputReader.OnRollEvent -= RollHandle;
    }

    private void Attack()
    {
        var entities = Player.GetCanAttackEntities(out var points);
        for (var i = 0; i < entities.Count; i++)
        {
            AttackFeedback(points[i]);
            entities[i].OnDamage(Player.PlayerData.damage, _attackDir);
        }
    }

    private void AttackFeedback(Vector3 point)
    {
        var hitParticle = PoolManager.Instance.Pop("HitParticle") as PoolableParticle;
        point += Random.insideUnitSphere * 0.2f;
        hitParticle.SetPositionAndRotation(point, Quaternion.identity);
        hitParticle.Play();
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

    public override void AnimationTrigger(string eventKey)
    {
        if (eventKey == "TrailOn")
        {
            _swordTrail.enabled = true;
        }
        else if (eventKey == "Attack")
        {
            Attack();
        }
        else if (eventKey == "TrailOff")
        {
            _swordTrail.enabled = false;
        }
        else if(eventKey == "AttackEnd")
        {
            _triggerCalledTime = Time.time;
            base.AnimationTrigger(eventKey);
        }
    }
}