using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardSpace : MonoBehaviour {

	public BoardManager boardManager;
	JuicyManager juicy;

	public List<Tile> tileList;
	public int numCols;
	public int numRows;
	public int colNum;
	public int rowNum;
	public int provisionalTileCount;
	public bool isCenterTile;

	void Start () {
		numCols = boardManager.numCols;
		numRows = boardManager.numRows;
		tileList = new List<Tile>();
		provisionalTileCount = 0;
		juicy = GameObject.FindWithTag ("JuicyManager").GetComponent<JuicyManager>();
	}

	public void AddTile(Tile tileToAdd){
		if (tileList.Count > 0) {
			tileList [tileList.Count - 1].gameObject.layer = LayerMask.NameToLayer ("Default");
		}
		PositionNewTile (tileToAdd);
		tileList.Add(tileToAdd);
		tileToAdd.gameObject.layer = LayerMask.NameToLayer ("TopTiles");
	}

	public void PositionNewTile(Tile tileToPosition){
		tileToPosition.transform.position = new Vector3(transform.position.x, provisionalTileCount * 0.2f + 0.1f, transform.position.z);
		provisionalTileCount += 1;
	}

	public void ResetTilesToPosition(){
		for(int i = 0; i < tileList.Count; i++) {
			tileList[i].transform.position = new Vector3(transform.position.x, i * 0.2f + 0.1f, transform.position.z);
		}
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
