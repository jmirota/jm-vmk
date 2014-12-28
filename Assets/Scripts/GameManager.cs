using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject player;
	private GameCamera gameCamera;
	private GameObject currentPlayer;
	public GameObject HUDCanvas;
	public Vector3 checkpoint;

	public static int levelCount = 2;
	public static int currentLevel = 1;

	private HUDCanvasController canvasController;

	// Use this for initialization
	void Start () {
		gameCamera = GetComponent<GameCamera>();
		canvasController = HUDCanvas.GetComponent<HUDCanvasController>();
		if(GameObject.FindGameObjectWithTag("Spawn")) {
			checkpoint = GameObject.FindGameObjectWithTag("Spawn").transform.position;
		}
		SpawnPlayer(checkpoint);
	}
	
	private void SpawnPlayer(Vector3 spawnPosition) {
		currentPlayer = (Instantiate(player, spawnPosition, Quaternion.identity) as GameObject);
		gameCamera.SetTarget(currentPlayer.transform);
	}

	private void Update() {
		if (!currentPlayer) {
			if (Input.GetButtonDown("Respawn")) {
				SpawnPlayer(checkpoint);
			}
		}
	}

	public void EndLevel(){
		if (currentLevel < levelCount){
			currentLevel++;
			Application.LoadLevel("Level " + currentLevel);
		} else {
			Debug.Log ("Game over");
		}
	}

	public void SetCheckpoint(Vector3 newCheckpoint) {
		checkpoint = newCheckpoint;
	}

	public void SetLives(int lives) {
		canvasController.SetLives(lives);
	}
}
