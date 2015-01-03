using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
	private Transform target;
	private float trackSpeed = 12;
	private const float CAMERA_Y_POSITION = 2.5f;
	private const float CAMERA_Z_POSITION = -10.0f;

	public GameObject LeftBorder;
	public GameObject RightBorder;

	public void SetTarget(Transform newTransform) {
		target = newTransform;
		transform.position = new Vector3(newTransform.position.x, CAMERA_Y_POSITION, CAMERA_Z_POSITION);
	}

	void LateUpdate() {
		if (target) {
			float x = Move(transform.position.x, target.position.x, trackSpeed);
			float y = transform.position.y;
			transform.position = new Vector3(x,y,transform.position.z);
		}
	}

	private float Move(float position, float target, float trackSpeed) {
		if (position == target) {
			return position;
		} else {
			Bounds cameraBounds = CameraViewPort.OrthographicBounds();
			float distanceToLeftBorder = Mathf.Sqrt((cameraBounds.center.x - cameraBounds.min.x) * (cameraBounds.center.x - cameraBounds.min.x));
			float distanceToRightBorder = Mathf.Sqrt((cameraBounds.center.x - cameraBounds.max.x) * (cameraBounds.center.x - cameraBounds.max.x));
			float direction = Mathf.Sign (target - position);
			float nextPosition = position + trackSpeed * Time.deltaTime * direction;
			float nextPositionAndLeftBorderDistance = Mathf.Sqrt((nextPosition - LeftBorder.transform.position.x) * (nextPosition - LeftBorder.transform.position.x));
			float nextPositionAndRightBorderDistance = Mathf.Sqrt((nextPosition - RightBorder.transform.position.x) * (nextPosition - RightBorder.transform.position.x));
			if (nextPositionAndLeftBorderDistance <= distanceToLeftBorder || nextPositionAndRightBorderDistance <= distanceToRightBorder) {
				return position;
			} else {
				position = nextPosition;
			return (direction == Mathf.Sign (target - position)) ? position: target;
			}
		}
	}

	void Start() {
		GameManager.instance.UpdateScore();
	}
}
