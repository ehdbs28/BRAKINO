using UnityEngine;

public abstract class State
{
    protected StateController Controller { get; private set; }
    protected Entity Owner { get; private set; }

    private readonly int _animationHash;
    
    public State(StateController controller, string animationParameter)
    {
        Controller = controller;
        Owner = Controller.Owner;
        _animationHash = Animator.StringToHash(animationParameter);
    }

    public virtual void EnterState()
    {
        Owner.AnimatorCompo.SetBool(_animationHash, true);
    }
    
    public abstract void UpdateState();

    public virtual void ExitState()
    {
        Owner.AnimatorCompo.SetBool(_animationHash, false);
    }
}
