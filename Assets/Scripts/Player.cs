using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
public class Player : MonoBehaviour {
    public float moveSpeed = 5f;


    Camera viewCamera;
    PlayerController controller;

	// Use this for initialization
	void Start () {
        controller = GetComponent<PlayerController>();
        viewCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        // Move input
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity);

        // fire a ray from the camera on the ground
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        // instersect with ground
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin, point, Color.red);
            controller.LookAt(point);
        }
	}
}
