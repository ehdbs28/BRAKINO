public class PlayerEquipment : PoolableMono
{
    public Player Owner { get; private set; }
    
    
    
    public override void OnPop()
    {
    }

    public override void OnPush()
    {
        
    }
}