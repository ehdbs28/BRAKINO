using UnityEngine;

public abstract class State
{
    protected StateController Controller { get; private set; }
    protected Entity Owner { get; private set; }

    protected bool _animationTriggerCalled;

    private readonly int _animationHash;
    
    public State(StateController controller, string animationParameter)
    {
        Controller = controller;
        Owner = Controller.Owner;
        _animationHash = Animator.StringToHash(animationParameter);
    }

    public virtual void EnterState()
    {
        _animationTriggerCalled = false;
        Owner.AnimatorCompo.SetBool(_animationHash, true);
    }
    
    public abstract void UpdateState();

    public virtual void ExitState()
    {
        Owner.AnimatorCompo.SetBool(_animationHash, false);
    }

    public virtual void AnimationTrigger(string eventKey)
    {
        _animationTriggerCalled = true;
    }
}
