using UnityEngine;

[CreateAssetMenu(menuName = "SO/EntityData/PlayerData")]
public class PlayerData : EntityData
{
    [Header("Player Attack Data")]
    public float comboWindowTime;
    public Vector3[] attackAdvances;

    [Header("Player Roll Data")] 
    public float rollTime;
    public float rollSpeed;
}