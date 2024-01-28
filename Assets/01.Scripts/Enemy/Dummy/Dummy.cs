using UnityEngine;

public class Dummy : Entity
{
    public override void Awake()
    {
        base.Awake();

        OnHitEvent += HitHandle;
        
        StateController.RegisterState(new DummyIdleState(StateController, "Idle"));
        StateController.RegisterState(new DummyHitState(StateController, "Hit"));
        StateController.ChangeState(typeof(DummyIdleState));
    }

    private void HitHandle(Vector3 attackedDir)
    {
        StateController.ChangeState(typeof(DummyHitState));
    }
}