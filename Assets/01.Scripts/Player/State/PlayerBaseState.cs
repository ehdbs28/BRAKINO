using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected Player Player => (Player)Owner;
    
    public PlayerBaseState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
    }

    protected virtual void PrimaryAttackHandle()
    {
        Controller.ChangeState(typeof(PlayerPrimaryAttackState));
    }
}