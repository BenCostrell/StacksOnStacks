using UnityEngine;
using System.Collections;

public class JuicyManager : MonoBehaviour {

	BoardManager boardmanager;
	TurnManager turnmanager;

	bool finishedintro;

	float boardSpaceBeginY = -15.0f;
	float tileBeginY = 15.0f;

	// Use this for initialization
	void Start () {
		boardmanager = GameObject.FindWithTag ("BoardManager").GetComponent<BoardManager> ();
		turnmanager = GameObject.FindWithTag("TurnManager").GetComponent<TurnManager> ();

		finishedintro = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!finishedintro && boardmanager.boardInitialized) {
			introAnimation ();
			finishedintro = true;
		}
		
	}


	void introAnimation(){
		float boardspacetime = 0.05f;
		float boardspacetimerate = 0.05f;
		for (int r = 0; r < boardmanager.numRows; r++) {

			for (int c = 0; c < boardmanager.numCols; c++) {
				Vector3 curr = boardmanager.board [r, c].transform.position;
				boardmanager.board[r, c].transform.position = new Vector3(curr.x,boardSpaceBeginY,curr.z);
				//float boardspacetime = Random.Range (0.3f, 1.5f);
				iTween.MoveTo(boardmanager.board[r,c].gameObject,iTween.Hash(
					"position",curr,
					"time",boardspacetime,
					//"easetype",iTween.EaseType.easeOutElastic));
					"easetype",iTween.EaseType.easeOutBack,
					"oncomplete","boardSpaceEnterSound",
					"oncompletetarget",transform.gameObject,
					"oncompleteparams",boardmanager.board[r,c].gameObject
				));
				boardspacetime += boardspacetimerate;
			}
		}

		float delaytiles = 0.2f;
		float delaytilesrate = 0.1f;
		float tiletime = 0.8f;


		tileEnter (1, 0, tiletime, delaytiles,delaytilesrate);
		delaytiles += delaytilesrate;


		for (int j1 = 0; j1 < boardmanager.numCols; j1++) {
			tileEnter (0, j1, tiletime,  delaytiles, delaytilesrate);
			if (boardmanager.board [0, j1].tileList.Count > 0) {
				delaytiles += delaytilesrate;
			}
		}

		for(int i1 = 1; i1 < boardmanager.numRows; i1++){
			tileEnter (i1,boardmanager.numCols-1, tiletime, delaytiles,delaytilesrate);
			if (boardmanager.board [i1,boardmanager.numCols-1].tileList.Count > 0) {
				delaytiles += delaytilesrate;
			}
		}

		for (int j2 = boardmanager.numCols - 2; j2 >= 0; j2--) {
			tileEnter (boardmanager.numRows - 1, j2, tiletime, delaytiles,delaytilesrate);
			if (boardmanager.board [boardmanager.numRows-1, j2].tileList.Count > 0) {
				delaytiles += delaytilesrate;
			}
		}
		for (int i2 = boardmanager.numRows - 2; i2 >= 2; i2--) {
			tileEnter (i2, 0, tiletime,delaytiles,delaytilesrate);
			if (boardmanager.board [i2,0].tileList.Count > 0) {
				delaytiles += delaytilesrate;
			}
		}

	}

	void tileEnter(int i, int j, float tiletime, float delaytiles, float delaytilesrate){
		for (int t = 0; t < boardmanager.board [i, j].tileList.Count; t++) {
			if (t > 0) {
				delaytiles += 1.2f;
			}
			Vector3 currt = boardmanager.board [i, j].tileList [t].transform.position;
			boardmanager.board [i, j].tileList [t].transform.position = new Vector3 (currt.x, tileBeginY, currt.z);

			iTween.MoveTo (boardmanager.board [i, j].tileList [t].gameObject, iTween.Hash (
				"position", currt,
				"time", tiletime,
				"easetype", iTween.EaseType.easeInSine,
				"delay", delaytiles,
				"oncomplete", "tileEnterSound",
				"oncompletetarget", transform.gameObject,
				"oncompleteparams",boardmanager.board [i, j].tileList [t].gameObject));
		}

	}


	void tileEnterSound(GameObject go){
		
		go.GetComponent<AudioSource> ().Play ();
	}

	void boardSpaceEnterSound(GameObject go){
		go.GetComponent<AudioSource> ().Play ();
	}
	
}