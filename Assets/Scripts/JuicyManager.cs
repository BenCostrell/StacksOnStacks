using UnityEngine;
using System.Collections;

public class JuicyManager : MonoBehaviour {

	BoardManager boardmanager;
	bool finishedintro;

	// Use this for initialization
	void Start () {
		boardmanager = GameObject.FindWithTag ("BoardManager").GetComponent<BoardManager> ();
		finishedintro = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!finishedintro && boardmanager.boardInitialized) {
			//iTween.EaseType = iTween.EaseType.easeOutBack;

			float boardspacetime = 0.05f;
			float boardspacetimerate = 0.05f;
			for (int r = 0; r < boardmanager.numRows; r++) {

				for (int c = 0; c < boardmanager.numCols; c++) {
					Vector3 curr = boardmanager.board [r, c].transform.position;
					boardmanager.board[r, c].transform.position = new Vector3(curr.x,-15,curr.z);
					//float boardspacetime = Random.Range (0.3f, 1.5f);
					iTween.MoveTo(boardmanager.board[r,c].gameObject,iTween.Hash(
						"position",curr,
						"time",boardspacetime,
						//"easetype",iTween.EaseType.easeOutElastic));
						"easetype",iTween.EaseType.easeOutBack));
					boardspacetime += boardspacetimerate;
				}
			}
			//float delaytiles = boardspacetime * boardmanager.numRows * boardmanager.numCols;
			//float delaytiles = 2.0f;
			float delaytiles = boardspacetime-1.3f;
			float delaytilesrate = 0.1f;
			float tiletime = 1.0f;
			float tiletimerate = 0.1f;


			tileEnter (1, 0, tiletime, tiletimerate, delaytiles,delaytilesrate);
		//	if (boardmanager.board [i, j].tileList.Count > 0) {
			delaytiles += delaytilesrate;


			for (int j1 = 0; j1 < boardmanager.numCols; j1++) {
				tileEnter (0, j1, tiletime, tiletimerate, delaytiles, delaytilesrate);
				if (boardmanager.board [0, j1].tileList.Count > 0) {
					delaytiles += delaytilesrate;
				}
			}

			for(int i1 = 1; i1 < boardmanager.numRows; i1++){
				tileEnter (i1,boardmanager.numCols-1, tiletime, tiletimerate, delaytiles,delaytilesrate);
				if (boardmanager.board [i1,boardmanager.numCols-1].tileList.Count > 0) {
					delaytiles += delaytilesrate;
				}
			}

			for (int j2 = boardmanager.numCols - 2; j2 >= 0; j2--) {
				tileEnter (boardmanager.numRows - 1, j2, tiletime, tiletimerate, delaytiles,delaytilesrate);
				if (boardmanager.board [boardmanager.numRows-1, j2].tileList.Count > 0) {
					delaytiles += delaytilesrate;
				}
			}
			for (int i2 = boardmanager.numRows - 2; i2 >= 2; i2--) {
				tileEnter (i2, 0, tiletime,tiletimerate,delaytiles,delaytilesrate);
				if (boardmanager.board [i2,0].tileList.Count > 0) {
					delaytiles += delaytilesrate;
				}
			}
			finishedintro = true;
		}
		
	}

	void tileEnter(int i, int j, float tiletime, float tiletimerate, float delaytiles, float delaytilesrate){
		for (int t = 0; t < boardmanager.board [i, j].tileList.Count; t++) {
			if (t > 0) {
				delaytiles += 1.0f;
			}
			Vector3 currt = boardmanager.board [i, j].tileList [t].transform.position;
			boardmanager.board [i, j].tileList [t].transform.position = new Vector3 (currt.x, 15, currt.z);
			iTween.MoveTo (boardmanager.board [i, j].tileList [t].gameObject, iTween.Hash (
				"position", currt,
				"time", tiletime,
				"easeytype", iTween.EaseType.easeInSine,
				"delay", delaytiles));
			tiletime += tiletimerate;
		}

	}
	
}