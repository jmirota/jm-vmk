using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDCanvasController : MonoBehaviour {
	public GameObject Live1;
	public GameObject Live2;
	public GameObject Live3;
	public Text ammoText;

	private int lives;
	
	public void SetLives(int updatedLives) {
		lives = updatedLives;
		Debug.Log(lives);
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

	public void SetAmmoText(int ammo, int magazines){
		int allAmmo;

		if(magazines == 0) {
			allAmmo = ammo;
		} else {
			allAmmo = ammo*(magazines - 1) + ammo;
		}
		
		Debug.Log(magazines);
		string ammoInfo = magazines == -1 ? string.Format("{0}", ammo) : string.Format("{0}/{1}", ammo, allAmmo);
		ammoText.text = ammoInfo;
	}

}
