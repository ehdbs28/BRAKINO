public class Dummy : Entity
{
    public override void Awake()
    {
        base.Awake();
        StateController.RegisterState(new DummyIdleState(StateController, "Idle"));
        StateController.RegisterState(new DummyHitState(StateController, "Hit"));
        StateController.ChangeState(typeof(DummyIdleState));
    }
}