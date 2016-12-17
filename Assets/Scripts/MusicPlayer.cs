using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

	JuicyManager juicymanager;
	BoardManager boardmanager;

	// Use this for initialization
	void Start () {
		juicymanager = GameObject.FindWithTag ("JuicyManager").GetComponent<JuicyManager> ();
		boardmanager = GameObject.FindWithTag ("BoardManager").GetComponent<BoardManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (juicymanager.realfinishedintro  && !GetComponent<AudioSource>().isPlaying && !juicymanager.gameend) {
			GetComponent<AudioSource> ().Play ();
		}
	}
}
