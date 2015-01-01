using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	float speed = 0.2f;
	private Vector2 amountToMove;
	private int direction; // -1 - left 1 - right

	public void Shoot(int dir) {
		direction = dir;
	}

	void Update() {
		Move();
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.tag == "Player") {
			Destroy(this.gameObject);
		} 
	}

	void OnBecameInvisible() {
		Destroy(this.gameObject);
	}
	
	void Move() {
		float deltaX = speed * direction;
		float deltaY = 0;
		
		Vector2 finalTransform = new Vector2(deltaX,deltaY);
		
		transform.Translate(finalTransform, Space.World);
	}
}
