using UnityEngine;

public interface IDamagable
{
    public void OnDamage(float damage, Vector3 attackedDir);
}