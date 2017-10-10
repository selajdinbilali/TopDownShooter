using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
public class Player : MonoBehaviour {
    
    public float moveSpeed = 5f;

    // we need a ref to the camera for the ray cast
    Camera viewCamera;
    PlayerController controller;

	void Start () {
        controller = GetComponent<PlayerController>();
        viewCamera = Camera.main;
	}
	
	void Update () {
        // Move input
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        // we call the Move method of the PlayerController class
        controller.Move(moveVelocity);

        // fire a ray from the camera on the ground
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        // instersect with ground
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        // this var will be filled by the Raycast method
        float rayDistance;

        // if the ground interesects the ray
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            // we get the point of intersection
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin, point, Color.red);

            // we call the LookAt method of the PlayerController
            controller.LookAt(point);
        }
	}
}
