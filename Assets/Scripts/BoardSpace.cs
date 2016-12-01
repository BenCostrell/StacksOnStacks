using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardSpace : MonoBehaviour {

	public BoardManager boardManager;

	public List<Tile> tileList;
	public int numCols;
	public int numRows;

	void Start () {
		numCols = boardManager.numCols;
		numRows = boardManager.numRows;
		tileList = new List<Tile>();
	}

	public void AddTile(Tile tileToAdd){
		if (tileList.Count > 0) {
			tileList [tileList.Count - 1].gameObject.layer = LayerMask.NameToLayer ("Default");
		}
		tileList.Add(tileToAdd);
		tileToAdd.transform.position = new Vector3(transform.position.x, tileList.Count * 0.2f - 0.1f, transform.position.z);
		tileToAdd.gameObject.layer = LayerMask.NameToLayer ("TopTiles");
		Color tileColor = tileToAdd.GetComponent<MeshRenderer> ().material.color;
		tileColor.a = 1;
		tileToAdd.GetComponent<MeshRenderer> ().material.color = tileColor;
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
