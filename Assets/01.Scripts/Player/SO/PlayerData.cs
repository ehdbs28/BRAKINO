using UnityEngine;

[CreateAssetMenu(menuName = "SO/EntityData/PlayerData")]
public class PlayerData : EntityData
{
    [Header("Player Attack Data")] 
    public float attackRadius;
    public float attackDistance;
    public int maxAttackCount;
    public float comboWindowTime;
    public float[] attackDelayTimes;
    public Vector3[] attackAdvances;

    [Header("Player Roll Data")] 
    public float rollTime;
    public float rollSpeed;
    
    [Header("Player Shield Data")]
    public float shieldMovementSpeed;
}