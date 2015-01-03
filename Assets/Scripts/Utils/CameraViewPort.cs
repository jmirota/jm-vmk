using UnityEngine;
using System.Collections;

public class CameraViewPort : MonoBehaviour {

	public static float CameraLeftBorder() {
		return 1.0f;
	}

	public static float CameraRightBorder() {
		Vector3 stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));
		return stageDimensions.x;
	}

	public static Bounds OrthographicBounds()
	{
		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = Camera.main.orthographicSize * 2;
		Bounds bounds = new Bounds(
			Camera.main.transform.position,
			new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
		return bounds;
	}
}
