using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
	private Transform target;
	private float trackSpeed = 8;

	public void SetTarget(Transform transform) {
		target = transform;
	}

	void LateUpdate() {
		if (target) {
			float x = Move(transform.position.x, target.position.x, trackSpeed);
			float y = Move(transform.position.y, target.position.y, trackSpeed);
			transform.position = new Vector3(x,y,transform.position.z);
		}
	}

	private float Move(float position, float target, float trackSpeed) {
		if (position == target) {
			return position;
		} else {
			float direction = Mathf.Sign (target - position);
			position += trackSpeed * Time.deltaTime * direction;
			return (direction == Mathf.Sign (target - position)) ? position: target;
		}
	}
}
