using UnityEngine;

public class PlayerMovementState : PlayerBaseState
{
    private readonly int _movementDirXHash;
    private readonly int _movementDirYHash;
    
    public PlayerMovementState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
        _movementDirXHash = Animator.StringToHash("MovementDirX");
        _movementDirYHash = Animator.StringToHash("MovementDirY");
    }

    public override void EnterState()
    {
        base.EnterState();
        Player.InputReader.OnAttackEvent += AttackHandle;
        Player.InputReader.OnRollEvent += RollHandle;
        Player.InputReader.OnShieldEvent += Player.ActivateShield;
    }

    public override void UpdateState()
    {
        var inputDir = Player.InputReader.movementInput;
        MovementDirAnimatorParameterSet(inputDir);

        if (_animationTriggerCalled)
        {
            CreateWalkDustTrail();
            _animationTriggerCalled = false;
        }

        if (inputDir.sqrMagnitude <= 0.05f)
        {
            Controller.ChangeState(typeof(PlayerIdleState));
            return;
        }

        if (Player.OnShieldState)
        {
            Player.Move(inputDir * Player.PlayerData.shieldMovementSpeed);
        }
        else
        {
            Player.Rotate(Quaternion.LookRotation(inputDir));
            Player.Move(inputDir * Player.Data.movementSpeed);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        Player.InputReader.OnAttackEvent -= AttackHandle;
        Player.InputReader.OnRollEvent -= RollHandle;
        Player.InputReader.OnShieldEvent -= Player.ActivateShield;
    }

    private void CreateWalkDustTrail()
    {
        var particle = PoolManager.Instance.Pop("WalkDustParticle") as PoolableParticle;
        var pos = Player.transform.position;
        var rot = Quaternion.LookRotation(-Player.ModelTrm.forward + Vector3.up);
        particle.SetPositionAndRotation(pos, rot);
        particle.Play();
    }

    private void MovementDirAnimatorParameterSet(Vector3 inputDir)
    {
        var movementDir = Quaternion.LookRotation(Player.ModelTrm.forward) * inputDir;
        Player.AnimatorCompo.SetFloat(_movementDirXHash, movementDir.x);
        Player.AnimatorCompo.SetFloat(_movementDirYHash, movementDir.z);
    }
}