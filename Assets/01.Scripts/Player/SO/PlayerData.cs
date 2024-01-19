using UnityEngine;

[CreateAssetMenu(menuName = "SO/EntityData/PlayerData")]
public class PlayerData : EntityData
{
    [Header("Player Attack Data")]
    public float comboWindowTime;
    public float attackEndDelay;
    public Vector3[] attackAdvances;
}