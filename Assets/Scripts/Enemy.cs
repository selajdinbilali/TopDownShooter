using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// add this to use the navmeshagent
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : MonoBehaviour {

    NavMeshAgent pathFinder;
    Transform target;

	
	void Start () {
        pathFinder = GetComponent<NavMeshAgent>();
        // the enemy will chase the player
        target = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(UpdatePath());
	}
	
	
	void Update () {
        
	}

    // to avoid calculating the path every frame
    IEnumerator UpdatePath()
    {
        float refreshRate = 0.25f;
        // while there is a target
        while (target != null)
        {
            // go chase the player
            Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);
            pathFinder.SetDestination(targetPosition);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
