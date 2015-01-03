using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject pistol;
	public GameObject submachinegun;
	public GameObject machinegun;

	private int levelCount = 6;
	public static int currentLevel = 1;
	public static int score = 0;
	public static int levelScore = 0;
	public static float levelStartTime;

	private const int END_LEVEL_BOUNS_POINTS = 1000;
	private const int END_GAME_BONUS_POINTS = 10000;

	private const string EXIT_KEY = "Exit";

	private GameObject[] guns;

	private static GameManager _instance;
	
	public static GameManager instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GameManager>();
				
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
		guns = new GameObject[]{ pistol, submachinegun, machinegun};
	}

	private void Update() {
		UpdateScore();

		if (Input.GetButtonDown(EXIT_KEY)) {
			Application.LoadLevel("Menu");
		}
	}

	public void EndLevel(){
		Application.LoadLevel("LoadNextLevel");
		score += levelScore;
		CalculateScore();
	}
	
	public void LoadNextLevel() {
		if (currentLevel < levelCount){
			currentLevel++;
			Application.LoadLevel("Level " + currentLevel);
			levelScore = 0;
			levelStartTime = Time.time;
			GameHUDManager.instance.SetScoreText(score);
		} else {
			Application.LoadLevel("Finish");
		}
	}

	public void SpawnGun(int gunType, Vector3 position) {
		int type = gunType == GunsConstants.BossGunType() ? GunsConstants.MachinegunGunType() : gunType;
		Instantiate(guns[type], position, Quaternion.Euler(new Vector3(0,90,0)));
	}
	
	public void RestartGame() {
		PlayerManager.instance.SetLives(1);
		Application.LoadLevel("Level " + currentLevel);
		levelScore = 0;
		PlayerManager.instance.Restart();
	}
	public void AddScore() {
		levelScore += 100;
		GameHUDManager.instance.SetScoreText(levelScore + score);
	}

	private void CalculateScore() {
		float deltaTime = Time.time - levelStartTime;
		int bonusPoints = (int)(1/deltaTime * 1000); 
		score += bonusPoints;
	}

	public void StartOver() {
		currentLevel = 0;
		score = 0;
		levelScore = 0;
		PlayerManager.instance.Restart();
		LoadNextLevel();
	}

	public void UpdateScore() {
		GameHUDManager.instance.SetScoreText(score + levelScore);
	}

	public void AddBonusPoints() {
		score += END_LEVEL_BOUNS_POINTS;
	}

	public int FinalScore() {
		return score + levelScore + END_GAME_BONUS_POINTS;
	}

	public void FinishGame() {
		Application.LoadLevel("Finish");
	}
}
