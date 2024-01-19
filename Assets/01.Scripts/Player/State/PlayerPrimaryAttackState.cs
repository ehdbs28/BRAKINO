using System.Collections;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerBaseState
{
    private int _comboCounter;
    private float _lastAttackTime;
    
    private readonly int _comboCounterHash;

    private Coroutine _runningRoutine;
    
    public PlayerPrimaryAttackState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
        _comboCounterHash = Animator.StringToHash("ComboCounter");
        _comboCounter = 0;
    }

    public override void EnterState()
    {
        base.EnterState();
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
        var inputDir = Player.InputReader.movementInput;
        var advanceDir = inputDir.sqrMagnitude >= 0.05f ? inputDir : Player.ModelTrm.forward;
        var currentAttackMovement = Player.PlayerData.attackAdvances[_comboCounter];

        var force = Quaternion.LookRotation(advanceDir) * currentAttackMovement;

        var currentTime = 0f;
        while (currentTime <= time)
        {
            currentTime += Time.deltaTime;
            Player.SetVelocity(force);
            yield return null;
        }
        Player.StopImmediately();
    }
}