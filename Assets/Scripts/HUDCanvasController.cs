using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDCanvasController : MonoBehaviour {
	public GameObject Live1;
	public GameObject Live2;
	public GameObject Live3;

	private int lives;
	
	public void SetLives(int updatedLives) {
		lives = updatedLives;
		UpdateLivesImages();
	}

	private void UpdateLivesImages() {
		bool live1Active = (lives > 0) ? true : false;
		bool live2Active  = (lives > 1) ? true : false;
		bool live3Active  = (lives > 2) ? true : false;
		Live1.SetActive(live1Active);
		Live2.SetActive(live2Active);
		Live3.SetActive(live3Active);
	}

}
