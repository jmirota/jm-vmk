using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	float speed = 0.2f;
	private Vector2 amountToMove;
	private float direction; // -1 - left 1 - right

	public void Shoot(float dir) {
		Debug.Log(dir);
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
			collider.GetComponent<Entity>().TakeDamage(1);
			Destroy(this.gameObject);
		}
	}
}
