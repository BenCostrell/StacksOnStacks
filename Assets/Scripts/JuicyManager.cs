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
		//	iTween.EaseType = iTween.EaseType.easeOutBack;
			for (int i = 0; i < boardmanager.numRows; i++) {
				for (int j = 0; j < boardmanager.numCols; j++) {
					Vector3 curr = boardmanager.board [i, j].transform.position;
					boardmanager.board[i, j].transform.position = new Vector3(curr.x,-15,curr.z);
					iTween.MoveTo (boardmanager.board [i, j].gameObject, curr, Random.Range(0.7f,2.5f));
					float tiletime = Random.Range (0.7f, 1.5f);
					for (int t = 0; t < boardmanager.board [i, j].tileList.Count; t++) {
						Vector3 currt = boardmanager.board [i, j].tileList [t].transform.position;
						boardmanager.board [i, j].tileList [t].transform.position = new Vector3 (currt.x, 15, currt.z);
						iTween.MoveTo (boardmanager.board [i, j].tileList [t].gameObject, currt, tiletime);
						tiletime = Random.Range (tiletime, tiletime+1.0f);
					}
				}
			}
			finishedintro = true;
		}
		
	}

}
