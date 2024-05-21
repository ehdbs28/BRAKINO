using UnityEngine;

public abstract class State
{
    protected StateController Controller { get; private set; }
    protected Entity Owner { get; private set; }

    protected bool _animationTriggerCalled;

    protected readonly int AnimationHash;
    
    public State(StateController controller, string animationParameter)
    {
        Controller = controller;
        Owner = Controller.Owner;
        AnimationHash = Animator.StringToHash(animationParameter);
    }

    public virtual void EnterState()
    {
        _animationTriggerCalled = false;
        AnimationSetting(true);
    }
    
    public abstract void UpdateState();

    public virtual void ExitState()
    {
        AnimationSetting(false);
    }

    public virtual void AnimationTrigger(string eventKey)
    {
        _animationTriggerCalled = true;
    }

    private void AnimationSetting(bool value)
    {
        Owner.AnimatorCompo.SetBool(AnimationHash, value);
    }
}
