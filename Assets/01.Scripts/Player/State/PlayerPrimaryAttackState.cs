using System.Collections;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerBaseState
{
    private int _comboCounter;
    private float _lastAttackTime;

    private Vector3 _attackDir;
    
    private readonly int _comboCounterHash;
    private readonly int _speedCurveHash;
    private readonly LayerMask _groundMask;

    private Coroutine _runningRoutine;
    
    public PlayerPrimaryAttackState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
        _comboCounterHash = Animator.StringToHash("ComboCounter");
        _speedCurveHash = Animator.StringToHash("SpeedCurve");
        _groundMask = LayerMask.GetMask("Ground");
        _comboCounter = 0;
    }

    public override void EnterState()
    {
        base.EnterState();

        _attackDir = GetAttackDir();
        Player.Rotate(Quaternion.LookRotation(_attackDir), 1f);
        
        if (_comboCounter > 2 || Time.time >= _lastAttackTime + Player.PlayerData.comboWindowTime)
        {
            _comboCounter = 0;
        }
        Player.AnimatorCompo.SetFloat(_comboCounterHash, _comboCounter);

        if (_runningRoutine is not null)
        {
            Player.StopCoroutine(_runningRoutine);
        }
        _runningRoutine = Player.StartCoroutine(AdvanceRoutine(0.2f));
    }

    public override void UpdateState()
    {
        Player.AnimatorCompo.speed = Player.AnimatorCompo.GetFloat(_speedCurveHash);

        if (_animationEndTriggerCalled)
        {
            Controller.ChangeState(typeof(PlayerIdleState));
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        ++_comboCounter;
        _lastAttackTime = Time.time;
    }

    private IEnumerator AdvanceRoutine(float time)
    {
        var currentAttackMovement = Player.PlayerData.attackAdvances[_comboCounter];

        var force = Quaternion.LookRotation(_attackDir) * currentAttackMovement;

        var currentTime = 0f;
        while (currentTime <= time)
        {
            currentTime += Time.deltaTime;
            Player.SetVelocity(force);
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
}