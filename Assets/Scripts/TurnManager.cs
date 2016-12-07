using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour {

	public string phase;
	public GameObject boardManagerObj;
	private BoardManager boardManager;
	public Vector3[] tileSpawnPositions;
	public int rotationIndex;
	private bool isTileSpawned;
	public Tile spawnedTile;
	public LayerMask topTiles;
	public LayerMask spawnedTileLayer;
	public bool tilePlaced;
	public bool tileSelected;
	public bool stackUnstacked;
	public GameObject mainCamera;
	public GameObject pivotPoint;

	// Use this for initialization
	void Start () {
		boardManager = boardManagerObj.GetComponent<BoardManager> ();
		isTileSpawned = false;
		tilePlaced = false;
		tileSelected = false;
		stackUnstacked = true;
		rotationIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isTileSpawned && stackUnstacked) {
			DrawTileToPlace ();
			isTileSpawned = true;
			stackUnstacked = false;
		}
		if (Input.GetMouseButton (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (isTileSpawned) {
				if (tileSelected) {
					PlaceTile (ray);
				} else {
					SelectTile (ray);
				}
			} else {
				SelectStack (ray);
			}
		}
		if (Input.GetKeyDown (KeyCode.Space) && tilePlaced) {
			FinalizeTilePlacement ();
		}
	}

	void SelectStack(Ray ray){
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, topTiles)) {
			Vector3 pointOnBoard = hit.transform.position;
		}
	}

	void SelectTile(Ray ray){
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, spawnedTileLayer)) {
			Color spawnColor = spawnedTile.GetComponent<MeshRenderer> ().material.color;
			spawnColor.a = 0.7f;			
			spawnedTile.GetComponent<MeshRenderer> ().material.color = spawnColor;
			tileSelected = true;
			spawnedTile.GetComponentInChildren<ParticleSystem> ().Play ();
		}
	}

	void PlaceTile(Ray ray){
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, topTiles)) {
			spawnedTile.transform.parent = null;
			Vector3 pointOnBoard = hit.transform.position;
			spawnedTile.transform.position = new Vector3 (pointOnBoard.x, pointOnBoard.y + 0.2f, pointOnBoard.z);
			tilePlaced = true;
		}
	}

	void FinalizeTilePlacement(){
		int colNum = Mathf.RoundToInt (spawnedTile.transform.position.x - 0.5f + boardManager.numCols / 2);
		int rowNum = Mathf.RoundToInt (spawnedTile.transform.position.z - 0.5f + boardManager.numRows / 2);
		boardManager.board [colNum, rowNum].AddTile (spawnedTile);
		tilePlaced = false;
		isTileSpawned = false;
		tileSelected = false;
		spawnedTile.GetComponent<MeshRenderer> ().sortingOrder = 0;
		spawnedTile.GetComponentInChildren<ParticleSystem> ().Stop ();
		spawnedTile.GetComponentInChildren<ParticleSystem> ().Clear ();

	}

	void DrawTileToPlace(){
		Tile tileToPlace;
		tileToPlace = boardManager.DrawTile ();
		tileToPlace.transform.SetParent (pivotPoint.transform);
		tileToPlace.transform.localPosition = new Vector3 (-5, 0, 0);
		tileToPlace.gameObject.layer = LayerMask.NameToLayer ("DrawnTile");
		spawnedTile = tileToPlace;
		spawnedTile.GetComponent<MeshRenderer> ().sortingOrder = 2;
	}
}
