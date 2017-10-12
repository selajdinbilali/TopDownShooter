using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    // the collision mask is important to not calculate collision with everything
    public LayerMask collisionMask;
    float speed = 10f;
    float damage = 1f;

    // to destroy the bullet after a time
    float lifeTime = 3f;
    // to compensate if the enemy moves between frames
    float skinWidth = 0.1f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
        // useful to hit enemies even if the projectile starts inside him
        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, 0.1f, collisionMask);
        if (initialCollisions.Length > 0)
        {
            OnHitObject(initialCollisions[0]);
        }
    }

    // set the speed of the projectile for example if we have different weapons
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

	void Update () {
        float moveDistance = speed * Time.deltaTime;
        // check if the projectile collides with an enemy
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
	}

    void CheckCollisions(float moveDistance)
    {
        // the ray starts from the projectil and goes forward
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // if we have a hit in our collission layer, we added the skinwidth here
        if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }

    }

    void OnHitObject(RaycastHit hit)
    {
        // we print the name of the object hit
        //Debug.Log(hit.collider.gameObject.name);
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hit);
        }
        // we destroy the projectile
        GameObject.Destroy(gameObject);
    }

    // same method as above but with a collider instead of a RaycastHit
    void OnHitObject(Collider c)
    {
        // we print the name of the object hit
        //Debug.Log(hit.collider.gameObject.name);
        IDamageable damageableObject = c.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(damage);
        }
        // we destroy the projectile
        GameObject.Destroy(gameObject);
    }
}
