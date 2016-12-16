using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

public class UIManager : MonoBehaviour {

	//public TurnManager turnmanager;
	TurnManager turnmanager;
	BoardManager boardmanager;

	Button[] buttons;

	public bool undoSpill;

	bool cameraShakeWasEnabled;


	// Use this for initialization
	void Start () {
		turnmanager = GameObject.FindWithTag("TurnManager").GetComponent<TurnManager> ();
		boardmanager = GameObject.FindWithTag ("BoardManager").GetComponent<BoardManager> ();

		buttons = GetComponentsInChildren<Button>(true);
		undoSpill = false;

		cameraShakeWasEnabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		//undo button

		if (turnmanager.mode == "Finalize Spill" && !turnmanager.anythingTweening) {
			//undobutton.GetComponent<Animator> ().SetTrigger ("Disabled");
			buttons [0].interactable = true;
			buttons [1].interactable = true;

		} else {
			buttons [0].interactable = false;
			buttons [1].interactable = false;

		}
	}

	public void UndoButtonClick(){
		turnmanager.UndoQueueSpill ();
		undoSpill = true;
	}

	public void ConfirmButtonClick(){
		turnmanager.FinalizeSpill ();
	}

	public void PauseButtonClick(){
		transform.GetChild (0).gameObject.SetActive (false);
		transform.GetChild (1).gameObject.SetActive (true);
		//GameObject.FindWithTag ("MainCamera").GetComponent<BlurOptimized> ().enabled = true;
		Camera.main.GetComponent<BlurOptimized>().enabled = true;
		if (Camera.main.GetComponent<CameraShake> ().enabled) {
			cameraShakeWasEnabled = true;
			Camera.main.GetComponent<CameraShake>().enabled = false;
		}

		Time.timeScale = 0f;

	}

	public void ResumeButtonClick(){

		Time.timeScale = 1f;
		transform.GetChild (0).gameObject.SetActive (true);
		transform.GetChild (1).gameObject.SetActive (false);
		Camera.main.GetComponent<BlurOptimized> ().enabled = false;
		if (cameraShakeWasEnabled) {
			cameraShakeWasEnabled = false;
			Camera.main.GetComponent<CameraShake>().enabled = true;
		}

		//Camera.main.GetComponent<BlurOptimized>().enabled = false;
	}

	public void RestartButtonClick(){

		Time.timeScale = 1f;
		SceneManager.LoadScene ("main");


	}

	public void MainMenuButtonClick(){

		Time.timeScale = 1f;
		SceneManager.LoadScene ("intro");

	}
}
