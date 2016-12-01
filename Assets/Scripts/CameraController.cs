using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject boardCenter;
	public GameObject turnManagerObj;
	private TurnManager turnManager;
	public int currentPositionIndex;

	// Use this for initialization
	void Start () {
		turnManager = turnManagerObj.GetComponent<TurnManager> ();
		currentPositionIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("rotateClockwise")){
			RotateBoard(1);
		}

		if(Input.GetButtonDown("rotateCounterclockwise")){
			RotateBoard(-1);
		}
	}

	void RotateBoard(int rotationDirection){
		currentPositionIndex = (currentPositionIndex + rotationDirection + 4)%4;
		turnManager.rotationIndex = currentPositionIndex;
		Vector3 rotation = new Vector3 (0, rotationDirection * 90, 0);

		iTween.RotateAdd (transform.parent.gameObject, iTween.Hash ("amount", rotation, "easetype", "easeOutBack"));

	}
}
