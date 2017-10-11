using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    // the collision mask is important to not calculate collision with everything
    public LayerMask collisionMask;
    float speed = 10f;
    float damage = 1f;

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

        // if we have a hit in our collission layer
        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
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
}
