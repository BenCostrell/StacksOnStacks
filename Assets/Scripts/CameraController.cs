using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject boardCenter;
	public GameObject turnManagerObj;
	private TurnManager turnManager;
	public int currentRotation;

	// Use this for initialization
	void Start () {
		turnManager = turnManagerObj.GetComponent<TurnManager> ();
		currentRotation = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("rotateClockwise")){
			transform.RotateAround (Vector3.zero, Vector3.up, 90f);
			currentRotation = (currentRotation + 90)%360;
			if (!turnManager.tilePlaced) {
				turnManager.spawnedTile.transform.RotateAround (Vector3.zero, Vector3.up, 90f);
			}

		}

		if(Input.GetButtonDown("rotateCounterclockwise")){
			transform.RotateAround (Vector3.zero, Vector3.up, -90f);
			currentRotation = (currentRotation - 90)%360;
			if (!turnManager.tilePlaced) {
				turnManager.spawnedTile.transform.RotateAround (Vector3.zero, Vector3.up, -90f);
			}
		}
	
	}

}
