using UnityEngine;

[CreateAssetMenu(menuName = "SO/EntityData/EnemyData")]
public class EnemyData : EntityData
{
    [Header("Enemy Sense Data")] 
    public float senseRadius;
    public float senseDelay;

    [Header("Enemy Attack Data")] 
    public float attackRadius;
    public float attackDistance;
    
    
}