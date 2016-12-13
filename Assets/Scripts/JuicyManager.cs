﻿using UnityEngine;
using System.Collections;

public class JuicyManager : MonoBehaviour {


	BoardManager boardmanager;
	TurnManager turnmanager;

	public float xSpillDir;
	public float zSpillDir;

	bool finishedintro;

	float boardSpaceBeginY = -15.0f;
	float tileBeginY = 15.0f;

	float delaySpaceCollapse;
	int spaceCount;

	public float delayTileSpill;

	// Use this for initialization
	void Start () {
		boardmanager = GameObject.FindWithTag ("BoardManager").GetComponent<BoardManager> ();
		turnmanager = GameObject.FindWithTag("TurnManager").GetComponent<TurnManager> ();

		delaySpaceCollapse = 0f;
		spaceCount = 0;
		finishedintro = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!finishedintro && boardmanager.boardInitialized) {
			introAnimation ();
			finishedintro = true;
		}
		
	}

	public void AnimateTileMove(Tile tile, int tileCount, Vector3 pos){
		if (turnmanager.mode == "Select Tile") {
			delayTileSpill = 0f;
		}

		if (finishedintro && (turnmanager.mode == "Queue Spill" || turnmanager.mode == "Spawn Tile")) {
			delayTileSpill += 0.2f;
			//float delayTile = (float)tileCount;
			//print(tileCount);
			iTween.MoveTo (tile.gameObject, iTween.Hash (
				"position", new Vector3 (pos.x, tileCount * 0.2f + 0.1f, pos.z),
				"time", 1.0f,
				"delay", delayTileSpill
			));
			print (xSpillDir+", "+zSpillDir);
			if (xSpillDir == 1) {
				float vrot = 180.0f;
				iTween.RotateAdd (tile.gameObject, iTween.Hash (
					"amount", new Vector3 (vrot, 0, 0),
					"time", 1.0f,
					"delay", delayTileSpill
				));
			} else if (xSpillDir == -1) {
				float vrot = -180.0f;
				iTween.RotateAdd (tile.gameObject, iTween.Hash (
					"amount", new Vector3 (vrot, 0,0),
					"time", 1.0f,
					"delay", delayTileSpill
				));
			} else if (zSpillDir == 1) {
				float vrot = 180.0f;
				iTween.RotateAdd (tile.gameObject, iTween.Hash (
					"amount", new Vector3 (0, 0, vrot),
					"time", 1.0f,
					"delay", delayTileSpill
				));
			} else if (zSpillDir == -1) {
				float vrot = -180.0f;
				iTween.RotateAdd (tile.gameObject, iTween.Hash (
					"amount", new Vector3 (0,0, vrot),
					"time", 1.0f,
					"delay", delayTileSpill
				));

			}
		
		} else {
			tile.transform.position = new Vector3 (pos.x, tileCount * 0.2f + 0.1f, pos.z);

		}
	}

	public void CollapseSideSpaces(GameObject go, int numOfSpaces){

		Camera.main.GetComponent<CameraShake> ().enabled = true;
		Camera.main.GetComponent<CameraShake> ().shakeDuration = 0.5f;
		iTween.MoveTo(go, iTween.Hash(
			"position", new Vector3(go.transform.position.x, -7f,go.transform.position.z),
			"time", 1.0f,
			"easetype", iTween.EaseType.easeInSine,
			"delay",delaySpaceCollapse, 
			"oncomplete", "destroySideSpace",
			"oncompletetarget",transform.gameObject,
			"oncompleteparams", go
		));
		delaySpaceCollapse += 0.2f;
		spaceCount++;
		if (spaceCount == numOfSpaces) {
			delaySpaceCollapse = 0f;
			spaceCount = 0;
		}
	}

	void destroySideSpace(GameObject go){
		Destroy (go);
		Camera.main.GetComponent<CameraShake> ().enabled = false;
		//boardmanager.Spill ();
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