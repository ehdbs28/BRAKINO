using UnityEngine;

public interface IDamageable
{
    public void OnDamage(float damage, Vector3 attackedDir);
    public bool IsDead();
}