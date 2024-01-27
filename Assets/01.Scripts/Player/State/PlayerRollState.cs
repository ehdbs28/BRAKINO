using System.Collections;
using UnityEngine;

public class PlayerRollState : PlayerBaseState
{
    private Vector3 _rollDirection;
    private float _rollStartTime;

    private Coroutine _runningRoutine;

    public PlayerRollState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Player.OnlyUseBaseAnimatorLayer();
        
        var inputDir = Player.InputReader.movementInput;
        _rollDirection = inputDir.sqrMagnitude >= 0.05f ? inputDir : Player.transform.forward;
        Player.Rotate(Quaternion.LookRotation(_rollDirection), 1);
        _rollStartTime = Time.time;

        if (_runningRoutine != null)
        {
            Player.StopCoroutine(_runningRoutine);
        }
        _runningRoutine = Player.StartCoroutine(PlayParticleRoutine());
    }

    public override void UpdateState()
    {
        if (Time.time < _rollStartTime + Player.PlayerData.rollTime)
        {
            Player.Move(_rollDirection * Player.PlayerData.rollSpeed);
        }
        else
        {
            Controller.ChangeState(typeof(PlayerIdleState));
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        Player.StopCoroutine(_runningRoutine);
        _runningRoutine = null;
    }

    private IEnumerator PlayParticleRoutine()
    {
        while (true)
        {
            PlayDustParticle();
            var waitTime = Random.Range(0.065f, 0.09f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void PlayDustParticle()
    {
        var dustParticle = PoolManager.Instance.Pop("RollDustParticle") as PoolableParticle;
        var pos = Player.transform.position; 
        var rot = Quaternion.LookRotation(-_rollDirection + Vector3.up);
        dustParticle.SetPositionAndRotation(pos, rot);
        dustParticle.Play();
    }
}