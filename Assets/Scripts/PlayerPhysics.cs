﻿using UnityEngine;
using System.Collections;


[RequireComponent (typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour {
	
	public LayerMask collisionMask;
	
	private BoxCollider collider;
	private Vector3 size;
	private Vector3 center;
	
	private float skin = .005f;
	
	[HideInInspector]
	public bool grounded;
	
	Ray ray;
	RaycastHit hit;
	
	void Start() {
		collider = GetComponent<BoxCollider>();
		size = collider.size;
		center = collider.center;
	}
	
	public void Move(Vector2 moveAmount) {
		
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		Vector2 position = transform.position;
		
		// Check collisions above and below
		grounded = false;
		
		for (int i = 0; i<3; i ++) {
			float dir = Mathf.Sign(deltaY);
			float x = (position.x + center.x - size.x/2) + size.x/2 * i; // Left, centre and then rightmost point of collider
			float y = position.y + center.y + size.y/2 * dir; // Bottom of collider
			
			ray = new Ray(new Vector2(x,y), new Vector2(0,dir));
			Debug.DrawRay(ray.origin,ray.direction);
			if (Physics.Raycast(ray,out hit,Mathf.Abs(deltaY),collisionMask)) {
				// Get Distance between player and ground
				float dst = Vector3.Distance (ray.origin, hit.point);
				
				// Stop player's downwards movement after coming within skin width of a collider
				deltaY = dst > skin? dst * dir + skin : deltaY = 0;
				
				grounded = true;
				
				break;
				
			}
		}
		
		
		
		Vector2 finalTransform = new Vector2(deltaX,deltaY);
		
		transform.Translate(finalTransform);
	}
	
}
