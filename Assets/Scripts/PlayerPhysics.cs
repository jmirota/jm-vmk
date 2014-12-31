using UnityEngine;
using System.Collections;


[RequireComponent (typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour {
	
	public LayerMask collisionMask;
	
	private new BoxCollider collider;
	private Vector3 size;
	private Vector3 center;

	private Vector3 originalSize;
	private Vector3 originalCentre;
	private float colliderScale;
	
	private float skin = .005f;

	private int collisionDivisionsX = 3;
	private int collisionDivisionsY = 10;
	
	[HideInInspector]
	public bool grounded;
	[HideInInspector]
	public bool movementStopped;
	
	Ray ray;
	RaycastHit hit;
	
	void Start() {
		collider = GetComponent<BoxCollider>();
		colliderScale = transform.localScale.x;
		originalSize = collider.size;
		originalCentre = collider.center;
		size = collider.size;
		center = collider.center;
		SetCollider(originalSize, originalCentre);
	}
	
	public void Move(Vector2 moveAmount) {
		
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		Vector2 position = transform.position;
		
		
		grounded = false;
		// Check collisions above and below
		for (int i = 0; i<collisionDivisionsX; i ++) {
			float dir = Mathf.Sign(deltaY);
			float x = (position.x + center.x - size.x/2) + size.x/(collisionDivisionsX-1) * i; // Left, centre and then rightmost point of collider
			float y = position.y + center.y + size.y/2 * dir; // Bottom of collider
			
			ray = new Ray(new Vector2(x,y), new Vector2(0,dir));
			if (Physics.Raycast(ray,out hit,Mathf.Abs(deltaY) + skin,collisionMask)) {
				// Get Distance between player and ground
				float distance = Vector3.Distance (ray.origin, hit.point);
				
				// Stop player's downwards movement after coming within skin width of a collider
				deltaY = distance > skin? distance * dir - skin * dir : deltaY = 0;
				
				grounded = true;
				
				break;
				
			}
		}
		movementStopped = false;
		// Check collisions left and right
		for (int i = 0; i<collisionDivisionsY; i ++) {
			float dir = Mathf.Sign(deltaX);
			float x = position.x + center.x + size.x/2 * dir; // Left, centre and then rightmost point of collider
			float y = position.y + center.y - size.y/2 + size.y/(collisionDivisionsY-1) *i; // Bottom of collider
			
			ray = new Ray(new Vector2(x,y), new Vector2(dir,0));
			if (Physics.Raycast(ray,out hit,Mathf.Abs(deltaX) + skin,collisionMask)) {
				// Get Distance between player and ground
				float distance = Vector3.Distance (ray.origin, hit.point);
				
				// Stop player's downwards movement after coming within skin width of a collider
				deltaX = distance > skin? distance * dir - skin * dir : deltaX = 0;
				movementStopped = true;
				break;
				
			}
		}
		
		if (!grounded && !movementStopped) {
			Vector3 playerDircetion = new Vector3(deltaX, deltaY);
			Vector3 origin = new Vector3(position.x + center.x + size.x/2 * Mathf.Sign (deltaX),position.y + center.y + size.y/2 * Mathf.Sign(deltaY));
			ray = new Ray(origin, playerDircetion.normalized);
			
			if(Physics.Raycast(ray, Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY), collisionMask)) {
				grounded = true;
				deltaY = 0;
			}
		}
            
				
		Vector2 finalTransform = new Vector2(deltaX,deltaY);
		
		transform.Translate(finalTransform, Space.World);
	}

	public void SetCollider(Vector3 originSize, Vector3 originCentre) {
		collider.size = originSize;
		collider.center = originCentre;

		size = size * colliderScale;
		center = originCentre * colliderScale;
	}

}
