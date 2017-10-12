using UnityEngine;

// this interface makes easy to know who was hit
// if the object has this interface we know that he can take a hit
public interface IDamageable
{
    void TakeHit(float damage, RaycastHit hit);

    // created to remove the RaycastHit
    void TakeDamage(float damage);
}