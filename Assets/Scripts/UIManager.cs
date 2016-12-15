using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	//public TurnManager turnmanager;
	TurnManager turnmanager;
	BoardManager boardmanager;

	Button[] buttons;

	public bool undoSpill;


	// Use this for initialization
	void Start () {
		turnmanager = GameObject.FindWithTag("TurnManager").GetComponent<TurnManager> ();
		boardmanager = GameObject.FindWithTag ("BoardManager").GetComponent<BoardManager> ();

		buttons = GetComponentsInChildren<Button>(true);
		undoSpill = false;
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

	public void RestartButtonClick(){
		SceneManager.LoadScene ("main");

	}
}
