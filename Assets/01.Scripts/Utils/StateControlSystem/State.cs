using UnityEngine;

namespace StateControlSystem
{
    public class State : IState
    {
        protected StateMachine _stateMachine;

        private readonly int _animToggleHash;
        private readonly int _velocityYHash;
        
        protected bool _animFinishTriggerCalled;

        protected Entity _owner;

        public State(StateMachine stateMachine, Entity owner, System.Enum type)
        {
            _stateMachine = stateMachine;
            _owner = owner;
            _animToggleHash = Animator.StringToHash(type.ToString());
            _velocityYHash = Animator.StringToHash("velocity_y");
        }

        public virtual void EnterState()
        {
            _owner.AnimatorCompo.SetBool(_animToggleHash, true);
        }

        public virtual void UpdateState()
        {
            _owner.AnimatorCompo.SetFloat(_velocityYHash, _owner.RigidbodyCompo.velocity.y);
        }

        public virtual void ExitState()
        {
            _owner.AnimatorCompo.SetBool(_animToggleHash, false);
        }

        public virtual void AnimationFinishTrigger()
        {
            _animFinishTriggerCalled = true;
        }
    }
}