using UnityEngine;
using System.Collections;

public class FinishController : MonoBehaviour {
	
	public GameObject HUDCanvas;
	private FinishHUDManagers HUDController;
	void Start() {
		HUDController = HUDCanvas.GetComponent<FinishHUDManagers>();
		HUDController.SetScoreText(GameManager.instance.FinalScore());
	}

	void Update () {
		if(Input.GetButtonDown("Start Game")) {
			GameManager.instance.StartOver();
		}

		HUDController.SetScoreText(GameManager.instance.FinalScore());
	}
}
