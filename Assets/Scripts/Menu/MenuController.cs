using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {	
	// Update is called once per frame
	void Update () {
	}

	public void StartGame() {
		GameManager.instance.RestartGame();
	}

	public void ExitGame() {
		Application.Quit();
	}
}
