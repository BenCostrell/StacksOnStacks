using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject boardCenter;
	public GameObject turnManagerObj;
	private TurnManager turnManager;
	public int currentPositionIndex;

	GameObject soundplayer;


	JuicyManager juicymanager;

	// Use this for initialization
	void Start () {
		turnManager = turnManagerObj.GetComponent<TurnManager> ();
		currentPositionIndex = 0;

		transform.parent.transform.position = new Vector3(10,0,0);

		juicymanager = GameObject.FindWithTag ("JuicyManager").GetComponent<JuicyManager> ();

		soundplayer = GameObject.FindWithTag ("SoundPlayer");
	}
	
	// Update is called once per frame
	void Update () {
		if (juicymanager.boardSpaceEntered) {

			//transform.parent.transform.position = new Vector3(0,0,0);
			iTween.MoveTo(transform.parent.gameObject,new Vector3(0,0,0),0.7f);
			juicymanager.boardSpaceEntered = false;
		}
	}

	public void RotateBoard(int rotationDirection){
		currentPositionIndex = (currentPositionIndex + rotationDirection + 4)%4;
		turnManager.rotationIndex = currentPositionIndex;
		Vector3 rotation = new Vector3 (0, currentPositionIndex * 90, 0);

		iTween.RotateTo (transform.parent.gameObject, iTween.Hash ("rotation", rotation, "time", 0.8f,"easetype", "easeOutBack"));

		if (turnManager.spillUI != null) {
			Vector3 uirotation = new Vector3 (0, rotation.y, 0);
			Vector3 uisubrotation = new Vector3 (0,-rotation.y, 0);

			iTween.RotateTo (turnManager.spillUI.gameObject, iTween.Hash(
				"rotation", uirotation,
				"time", 0.8f,
				"easetype", "easeOutBack"
			
			));

			GameObject spillUIchild = turnManager.spillUI.transform.GetChild (0).gameObject;
			iTween.RotateTo (spillUIchild, iTween.Hash (
				"islocal", true,
				"rotation", uisubrotation,
				"time", 0.8f,
				"easetype", "easeOutBack"
			));
		}

		if (!soundplayer.transform.GetChild (0).gameObject.GetComponent<AudioSource> ().isPlaying) {
			soundplayer.transform.GetChild (0).gameObject.GetComponent<AudioSource> ().Play ();
		}
	}
}
