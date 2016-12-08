using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public TurnManager turnmanager;

	public Button undobutton;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (turnmanager.mode == "Finalize Spill") {
			//undobutton.GetComponent<Animator> ().SetTrigger ("Disabled");
			undobutton.interactable = true;
		} else {
			undobutton.interactable = false;
		}
	}

	public void UndoButtonClick(){
		turnmanager.UndoQueueSpill ();
	}
}
