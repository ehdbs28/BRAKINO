using UnityEngine;

[CreateAssetMenu(menuName = "SO/EntityData/PlayerData")]
public class PlayerData : EntityData
{
    public float comboWindowTime;
    public float attackEndDelay;
    public Vector3[] attackAdvances;
}