using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDCanvasController : MonoBehaviour {
	public GameObject Live1;
	public GameObject Live2;
	public GameObject Live3;
	public Text ammoText;
	public Text scoreValueText;
	public Text reloadingText;

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

	public void SetAmmoText(int ammo, int ammunitionLeft){
		string ammoString = ammunitionLeft == GunsConstants.InfinityAmmunition() ? string.Format("{0}/∞", ammo) : string.Format("{0}/{1}", ammo, ammunitionLeft);
		ammoText.text = ammoString;
	}

	public void SetScoreValueText(int scoreValue) {
		scoreValueText.text = string.Format("{0}", scoreValue);
	}

	public void ShowReloadingText() {
		reloadingText.text = "Reloading";
	}

	public void HideReloadingText() {
		reloadingText.text = "";
	}
}
