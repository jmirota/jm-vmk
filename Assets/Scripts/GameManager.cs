using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject player;
	private GameCamera gameCamera;
	// Use this for initialization
	void Start () {
		gameCamera = GetComponent<GameCamera>();
		SpawnPlayer();
	}
	
	private void SpawnPlayer() {
		gameCamera.SetTarget((Instantiate(player, Vector3.zero, Quaternion.identity) as GameObject).transform);
	}

}
