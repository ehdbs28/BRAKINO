public abstract class EnemyBaseState : State
{
    protected Enemy Enemy => (Enemy)Owner;
    
    public EnemyBaseState(StateController controller, string animationParameter) : base(controller, animationParameter)
    {
    }
}