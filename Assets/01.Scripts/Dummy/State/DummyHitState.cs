public class DummyHitState : State
{
    public DummyHitState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Owner.AnimatorCompo.Play(AnimationHash, -1, 0);
    }

    public override void UpdateState()
    {
        if (_animationTriggerCalled)
        {
            Controller.ChangeState(typeof(DummyIdleState));
        }
    }
}