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
	public int color;

	void Start () {
		numCols = boardManager.numCols;
		numRows = boardManager.numRows;
		tileList = new List<Tile>();
		provisionalTileCount = 0;
		juicy = GameObject.FindWithTag ("JuicyManager").GetComponent<JuicyManager>();
	}

	public void AddTile(Tile tileToAdd, bool positionTile){
		if (!isCenterTile) {
			if (tileList.Count > 0) {
				tileList [tileList.Count - 1].gameObject.layer = LayerMask.NameToLayer ("Default");
			}
			if (positionTile) {
				tileToAdd.transform.position = new Vector3 (transform.position.x, provisionalTileCount * 0.2f + 0.1f, transform.position.z);
			}
				
			provisionalTileCount += 1;
			tileList.Add (tileToAdd);
			tileToAdd.gameObject.layer = LayerMask.NameToLayer ("TopTiles");
		} else {
			color = tileToAdd.color;
			GetComponent<MeshRenderer>().material = boardManager.mats[color + 3];
			Destroy (tileToAdd.gameObject);
			boardManager.centerSpaceChanged = true;
		}
	}

	public void PositionNewTile(Tile tileToPosition){
		juicy.AnimateTileMove (tileToPosition, provisionalTileCount, transform.position);
		provisionalTileCount += 1;
	}

	public void ResetTilesToPosition(){
		for(int i = 0; i < tileList.Count; i++) {
			Tile tile = tileList [i];
			iTween.Stop (tile.gameObject);
			tile.transform.rotation = Quaternion.Euler (Vector3.zero);
			tile.transform.position = new Vector3(transform.position.x, i * 0.2f + 0.1f, transform.position.z);
		}
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
