using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	public void LoadGame(){
		GetComponent<AudioSource> ().Play ();
		StartCoroutine (executeGame());
	}

	public void LoadTutorial(){
		GetComponent<AudioSource> ().Play ();
		StartCoroutine (executeTutorial());
	}

	IEnumerator executeGame(){
		yield return new WaitForSeconds (0.2f);
		SceneManager.LoadScene ("main");
	}

	IEnumerator executeTutorial(){
		yield return new WaitForSeconds (0.2f);
		SceneManager.LoadScene ("instructions");

			

	}
}
