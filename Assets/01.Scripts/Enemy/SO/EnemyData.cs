using UnityEngine;

[CreateAssetMenu(menuName = "SO/EntityData/EnemyData")]
public class EnemyData : EntityData
{
    [Header("Enemy Attack Data")] 
    public float attackRadius;
    public float attackDistance;
    
    
}