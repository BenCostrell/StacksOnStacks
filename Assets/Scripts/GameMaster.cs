using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadGame(){
		StartCoroutine (executeLoadGame());
	}

	IEnumerator executeLoadGame(){
		yield return new WaitForSeconds (0.3f);

		if (SceneManager.GetActiveScene ().name == "intro") {

			SceneManager.LoadScene ("instructions");

		} else {

			SceneManager.LoadScene ("main");

		}
			

	}
}
