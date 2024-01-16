using UnityEngine;

public class EntityData : ScriptableObject
{
    public float maxHp;
    
    public float movementSpeed;
    [Range(0.1f, 1f)] public float rotateSpeed;

    public float damage;
}