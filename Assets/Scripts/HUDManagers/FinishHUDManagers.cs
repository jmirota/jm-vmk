using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FinishHUDManagers : MonoBehaviour {

	public Text scoreText;

	public void SetScoreText(int score) {
		scoreText.text = string.Format("Your score: {0}", score);
	}
}
