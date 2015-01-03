using UnityEngine;
using System.Collections;

public class GameHUDManager : MonoBehaviour {

	private GameObject HUDCanvas;
	private HUDCanvasController canvasController;

	private static GameHUDManager _instance;
	
	public static GameHUDManager instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GameHUDManager>();
				
				//Tell unity not to destroy this object when loading a new scene!
				DontDestroyOnLoad(_instance.gameObject);
			}
			
			return _instance;
		}
	}
	
	void Awake() 
	{
		if(_instance == null)
		{
			//If I am the first instance, make me the Singleton
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != _instance)
				Destroy(this.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		HUDCanvas = GameObject.FindGameObjectWithTag("HUDCanvas") as GameObject;
		if (HUDCanvas != null) {
			canvasController = HUDCanvas.GetComponent<HUDCanvasController>();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetLives(int lives) {
		if (canvasController != null) {
			canvasController.SetLives(lives);
		}
	}

	public void SetAmmoText(int ammo, int magazines){
		if (canvasController != null) {
			canvasController.SetAmmoText(ammo, magazines);
		}
	}

	public void SetScoreText(int score) {
		if (canvasController != null) {
			canvasController.SetScoreValueText(score);
		}
	}

	public void ShowReloadingText() {
		if (canvasController != null) {
			canvasController.ShowReloadingText();
		}
	}
	
	public void HideReloadingText() {
		if (canvasController != null) {
			canvasController.HideReloadingText();
		}
	}
}
