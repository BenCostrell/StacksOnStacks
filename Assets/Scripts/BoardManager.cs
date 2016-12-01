using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

	public GameObject spacePrefab;
	public GameObject tilePrefab;
	public int numCols;
	public int numRows;
	public int currentNumCols;
	public int currentNumRows;
	public BoardSpace[,] board;
	private List<Tile> tileBag;
	public int initialNumberOfEachTileColor;
	private bool boardInitialized;

	public Material[] mats;
	/*
	 * index:	1) board1
	 * 			2) board2
	 * 			3) red
	 * 			4) blue
	 * 			5) yellow
	 * 			6) green
	 */

	// Use this for initialization
	void Start () {
		numCols = 6;
		numRows = 6;

		currentNumCols = numCols;
		currentNumRows = numRows;

		CreateBoard ();
		CreateTileBag ();
		boardInitialized = false;
	}

	// Update is called once per frame
	void Update () {
		if (!boardInitialized) {
			InitialBoardSetup ();
			boardInitialized = true;
		}
	}

	void CreateBoard(){
		board = new BoardSpace[numCols, numRows];
		for (int i = 0; i < numCols; i++) {
			for (int j = 0; j < numRows; j++) {
				int spaceColor;
				if (IsCentered (i, numCols) && IsCentered (j, numRows)) {
					//spaceColor = Color.magenta;
					spaceColor = 2;
				} else if ((i + j) % 2 == 0) {
					//spaceColor = Color.gray;
					//spaceColor = new Color(0.5f, 0.5f, 0.5f);
					spaceColor = 0;
				} else {
					//spaceColor = new Color(0.8f,0.8f,0.8f,1f);
					//spaceColor = Color.white;
					spaceColor = 1;
				}
				CreateBoardSpace (i, j, spaceColor);
			}
		}
	}

	void CreateBoardSpace (int colNum, int rowNum, int materialIndex){
		GameObject boardSpace;
		Vector3 location = new Vector3(colNum - numCols/2 + 0.5f, 0, rowNum - numRows/2 +  0.5f);
		boardSpace = Instantiate(spacePrefab, location, Quaternion.LookRotation(Vector3.down)) as GameObject;
		//boardSpace.GetComponent<MeshRenderer>().material.color = spaceColor;
		//boardSpace.GetComponent<MeshRenderer>().material.SetColor("_Color",spaceColor);
		boardSpace.GetComponent<MeshRenderer>().material = mats[materialIndex];
		boardSpace.GetComponent<BoardSpace>().boardManager = this;
		board [colNum, rowNum] = boardSpace.GetComponent<BoardSpace> ();
	}

	void CreateTile(int materialIndex){
		GameObject tile;
		Vector3 offscreen = new Vector3(-1000, -1000, -1000);
		tile = Instantiate(tilePrefab, offscreen, Quaternion.identity) as GameObject;
		tile.GetComponent<MeshRenderer>().material = mats[materialIndex];
		tileBag.Add(tile.GetComponent<Tile>());
	}

	void CreateTileBag(){
		tileBag = new List<Tile> ();

		//CreateTilesOfAColor (Color.red);

		//red
		CreateTilesOfAColor(3);

		//blue
		CreateTilesOfAColor (4);

		//yellow
		CreateTilesOfAColor (5);

		//green
		CreateTilesOfAColor (6);
	}

	void CreateTilesOfAColor(int materialIndex){
		for (int i = 0; i < initialNumberOfEachTileColor; i++) {
			CreateTile (materialIndex);
		}
	}

	void InitialBoardSetup(){
		for (int i = 0; i < numCols; i++) {
			for (int j = 0; j < numRows; j++) {
				if ((!IsCentered(i, numCols) && IsEdge(j, numRows)) || (!IsCentered(j, numRows) && IsEdge(i, numCols))){
					Tile firstTileToPlace;
					Tile secondTileToPlace;
					firstTileToPlace = DrawTile();
					secondTileToPlace = DrawTile ();
					board [i, j].AddTile (firstTileToPlace);
					board [i, j].AddTile (secondTileToPlace);
				}
			}
		}
	}

	public bool IsCentered(int index, int sideLength){
		bool centered = (index == sideLength / 2 - 1) || (index == sideLength / 2);
		return centered;
	}

	bool IsEdge(int index, int sideLength){
		bool edge = (index == 0) || (index == sideLength - 1);
		return edge;
	}

	public Tile DrawTile(){
		int numTilesInBag = tileBag.Count;
		int tileIndexToDraw = Random.Range (0, numTilesInBag);
		Tile drawnTile = tileBag [tileIndexToDraw];
		tileBag.Remove (drawnTile);
		return drawnTile;
	}

	public void UnstackSpace(int colNum, int rowNum, int xDirection, int zDirection){
		int boardSpaceX = colNum;
		int boardSpaceZ = rowNum;
		BoardSpace spaceToUnstack = board [colNum, rowNum];
		int numTilesToMove = spaceToUnstack.tileList.Count;
		for (int i = 0; i < numTilesToMove; i++) {
			int index = numTilesToMove - 1 - i;
			Tile tileToMove = spaceToUnstack.tileList [index];
			spaceToUnstack.tileList.RemoveAt (index);
			boardSpaceX = (boardSpaceX + xDirection) % currentNumCols;
			boardSpaceZ = (boardSpaceZ + zDirection) % currentNumRows;
			board [boardSpaceX, boardSpaceZ].AddTile (tileToMove);
		}
	}


}
