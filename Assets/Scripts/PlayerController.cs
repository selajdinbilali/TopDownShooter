using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the playercontroller requires a rigidbody
// this line will add it automatically in unity editor
[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {

    // private variables
    Vector3 velocity;
    Rigidbody myRigidbody;
	
	void Start ()
    {
        // we have to get the rigidbody
        myRigidbody = GetComponent<Rigidbody>();
	}

    public void Move(Vector3 vel)
    {
        // reset the velocity, this method is called from the Player class
        velocity = vel;
    }

    public void LookAt(Vector3 lookPoint)
    {
        // corrects the height because the player will tilt to look at the ground
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        // make the player look at the point
        transform.LookAt(heightCorrectedPoint);
    }

    private void FixedUpdate()
    {
        // we always move rigidbodies in fixedupdate
        myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
    }

    
}
