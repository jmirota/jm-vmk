using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadLevelHudCanvasController : MonoBehaviour {

	public Text levelText; 
	private float StartTime;

	// Use this for initialization
	void Start () {
		levelText.text = string.Format("Level {0}", GameManager.currentLevel + 1);
		StartTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - StartTime > 3.0f) {
			GameManager.instance.LoadNextLevel();
		}
	}
}
