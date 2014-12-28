using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	float speed = 0.2f;
	private Vector2 amountToMove;
	private float direction; // -1 - left 1 - right

	public void Shoot(float dir) {
		direction = dir;
	}

	void Update() {
		float deltaX = speed * direction;
		float deltaY = 0;

		Vector2 finalTransform = new Vector2(deltaX,deltaY);
		
		transform.Translate(finalTransform, Space.World);
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.tag == "Player") {
			PlayerController playerController = collider.GetComponent<PlayerController>();
			playerController.TakeDamage(1);
			playerController.UpdateHealth();
			Destroy(this.gameObject);
		} else if (collider.tag == "Enemy") {
			Destroy(this.gameObject);
		}
	}

	void OnBecameInvisible() {
		Destroy(this.gameObject);
	}
}
