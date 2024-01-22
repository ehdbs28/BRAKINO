public class DummyHitState : State
{
    public DummyHitState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Owner.AnimatorCompo.Play("Hit", -1, 0);
    }

    public override void UpdateState()
    {
        if (_animationEndTriggerCalled)
        {
            Controller.ChangeState(typeof(DummyIdleState));
        }
    }
}