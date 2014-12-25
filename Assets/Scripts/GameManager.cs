using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject player;
	private GameCamera gameCamera;
	private GameObject currentPlayer;
	// Use this for initialization
	void Start () {
		gameCamera = GetComponent<GameCamera>();
		SpawnPlayer(Vector3.zero);
	}
	
	private void SpawnPlayer(Vector3 spawnPosition) {
		currentPlayer = (Instantiate(player, spawnPosition, Quaternion.identity) as GameObject);
		gameCamera.SetTarget(currentPlayer.transform);
	}

	private void Update() {
		if (!currentPlayer) {
			if (Input.GetButtonDown("Respawn")) {
				SpawnPlayer(Vector3.zero);
			}
		}
	}

}
