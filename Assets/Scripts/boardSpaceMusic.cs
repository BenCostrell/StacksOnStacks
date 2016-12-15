using UnityEngine;
using System.Collections;

public class boardSpaceMusic : MonoBehaviour {

	// Use this for initialization

	public AudioClip sfxScore;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void playOnScoring(){
		GetComponent<AudioSource> ().clip = sfxScore;
		GetComponent<AudioSource> ().pitch = GameObject.FindWithTag ("JuicyManager").GetComponent<JuicyManager> ().scorePitch;
		GetComponent<AudioSource> ().Play ();
	}
}
