using UnityEngine;
using System.Collections;

public class FirstAid : MonoBehaviour {

	void OnTriggerEnter(Collider collider) {
		if (collider.tag == Tags.PlayerTag()) {
			Destroy(this.gameObject);
		}
	}
}
