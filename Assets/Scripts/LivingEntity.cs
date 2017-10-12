using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// super class
// pretty straight forward
public class LivingEntity : MonoBehaviour, IDamageable {

    public float startingHealth;
    protected float health;
    protected bool dead;

    // we create an event
    // other objects can subscribe to it
    public event System.Action OnDeath;

    protected virtual void Start()
    {
        health = startingHealth;
    }

    public void TakeHit(float damage, RaycastHit hit)
    {
        // do some stuff here with the hit variable
        TakeDamage(damage);
    }

    // new function demanded by the interface IDamageable
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    protected void Die()
    {
        dead = true;
        if (OnDeath != null)
        {
            // the object that are subscribed to this event
            // will call all the methods subscribed the this event
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
}
