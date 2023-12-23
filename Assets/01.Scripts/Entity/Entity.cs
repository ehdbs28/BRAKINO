using StateControlSystem;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Animator AnimatorCompo { get; protected set; }
    public Rigidbody2D RigidbodyCompo { get; protected set; }

    protected StateMachine _stateMachine;

    public int FacingDirection { get; protected set; }

    public virtual void Awake()
    {
        _stateMachine = new StateMachine();
        RegisterStates();
    }

    public virtual void Start()
    {
        SetInitState();
    }

    public virtual void Update()
    {
        _stateMachine.CurrentState?.UpdateState();
    }

    protected abstract void RegisterStates();
    protected abstract void SetInitState();
    
#region Movement Control

    public void SetVelocity(float x, float y, bool doNotFlip = false)
    {
        RigidbodyCompo.velocity = new Vector2(x, y);
        if (!doNotFlip)
        {
            FlipController(x);
        }
    }

    public void StopImmediately(bool withYAxis)
    {
        RigidbodyCompo.velocity = withYAxis ? Vector2.zero : new Vector2(0, RigidbodyCompo.velocity.y);
    }

#endregion
    
#region Flip Control

    public virtual void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }

    public virtual void FlipController(float dir)
    {
        var rightFlip = dir > 0 && FacingDirection < 0;
        var leftFlip = dir < 0 && FacingDirection > 0;

        if (rightFlip || leftFlip)
        {
            Flip();
        }
    }
    
#endregion
}