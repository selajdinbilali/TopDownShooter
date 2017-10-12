using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// add this to use the navmeshagent
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : LivingEntity {

    // enemy states to not conflict with attack and update path
    public enum State
    {
        Idle, Chasing, Attacking
    };
    State currentState;

    NavMeshAgent pathFinder;
    Transform target;
    LivingEntity targetEntity;
    // the material of the enemy
    Material skinMaterial;
    // to change the color when he attacks
    Color originalColor;

    // the distance the enemy stops before attacking
    float attackDistanceThreshold = 0.5f;
    float timeBetweenAttacks = 1f;
    // the damage that the enemy will inflict
    float damage = 1f;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

    // to avoid errors if the player is dead
    bool hasTarget;

	
	protected override void Start () {
        base.Start();
        pathFinder = GetComponent<NavMeshAgent>();
        skinMaterial = GetComponent<Renderer>().material;
        originalColor = skinMaterial.color;

        // to avoid errors if the player is dead
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {

            currentState = State.Chasing;
            hasTarget = true;
            // the enemy will chase the player
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;
            // we get the radiuses
            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = GetComponent<CapsuleCollider>().radius;

            StartCoroutine(UpdatePath());
        }
        
	}

    void OnTargetDeath()
    {
        hasTarget = false;
        // the enemy now stays idle
        currentState = State.Idle;
    }
	
	
	void Update () {
        
        if (hasTarget)
        {
            // Vector.Distance uses sqrt that is expensive
            if (Time.time > nextAttackTime)
            {
                float sqrtDistToTarget = (target.position - transform.position).sqrMagnitude;

                if (sqrtDistToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        }
        
        
	}

    IEnumerator Attack()
    {
        // we change our state
        currentState = State.Attacking;
        // we deactivate the path finder
        pathFinder.enabled = false;
        // the position before the attack starts
        Vector3 originalPosition = transform.position;
        // let's say we are i (1,1) and the player is (2,2) so
        // (2,2) - (1,1) = (1,1) 
        // we dont want to go faster diagonally so we normalize
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        // to not entirely go inside the player
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);
        // percentage of the attack done
        float percent = 0f;
        float attackSpeed = 3f;

        // we become red when we attack
        skinMaterial.color = Color.red;

        // to apply damage only when inside the player
        bool hasAppliedDamage = false;

        // while the attack is note done
        while (percent <= 1)
        {
            if (percent >= 0.5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }
            // we augment the percent to arrive at 1
            percent += Time.deltaTime * attackSpeed;
            // we do some math
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            // we update the position
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
            // we return a frame
            yield return null;
        }
        // we go back to our color and our chasing state
        skinMaterial.color = originalColor;
        currentState = State.Chasing;
        pathFinder.enabled = true;
    }

    // to avoid calculating the path every frame
    IEnumerator UpdatePath()
    {
        float refreshRate = 0.25f;
        // while there is a target
        while (hasTarget)
        {
            if (currentState == State.Chasing)
            {
                // go chase the player
                // same as in attack()
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2.0f);
                if (!dead)
                {
                    pathFinder.SetDestination(targetPosition);
                }
                
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
