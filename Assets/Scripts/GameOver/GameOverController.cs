using UnityEngine;
using System.Collections;

public class GameOverController : MonoBehaviour {

	void Update () {
		if(Input.GetButtonDown("Start Game")) {
			GameManager.instance.RestartGame();
		}
	}
}
