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
	private BoardSpace selectedSpace;
	public LayerMask topTiles;
	public LayerMask spawnedTileLayer;
	public LayerMask topTilesAndSpillUI;
	public LayerMask spillUILayer;
	public string mode;
	public GameObject mainCamera;
	public GameObject pivotPoint;
	public GameObject spillUIPrefab;
	public GameObject spillUI;
	private bool firstTileFinalized;


	// Use this for initialization
	void Start () {
		boardManager = boardManagerObj.GetComponent<BoardManager> ();
		mode = "Spawn Tile"; //spawn tile, select tile, place tile 0 (you haven't placed it anywhere yet), place tile 1 (you've placed it somewhere, but not finalized), select stack
		rotationIndex = 0;
		firstTileFinalized = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (mode == "Spawn Tile") {
			DrawTileToPlace ();
			mode = "Select Tile";
		}
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (mode == "Select Tile") {
				SelectTile (ray);
			} else if (mode == "Place Tile 0" || mode == "Place Tile 1") {
				PlaceTile (ray);
			} else if (mode == "Select Stack") {
				SelectStack (ray);
			} else if (mode == "Queue Spill") {
				if (!SelectStack (ray)) {
					InitQueueSpill (ray);
				}
			} else if (mode == "Finalize Spill") {
				UndoQueueSpill ();
			}
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (mode == "Place Tile 1") {
				FinalizeTilePlacement ();
			} else if (mode == "Finalize Spill") {
				FinalizeSpill ();
			}
		}
	}

	bool SelectStack(Ray ray){
		bool stackSelected = false;
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, topTilesAndSpillUI)) {
			if (hit.collider.transform.tag == "Tile") {
				stackSelected = true;
				Vector3 tileHitLocation = hit.transform.position;
				BoardSpace space = CalculateSpaceFromLocation (tileHitLocation);
				if (selectedSpace != null) {
					if (selectedSpace != space) {
						foreach (Tile tile in selectedSpace.tileList) {
							tile.GetComponentInChildren<ParticleSystem> ().Stop ();
							tile.GetComponentInChildren<ParticleSystem> ().Clear ();
						}
						foreach (Tile tile in space.tileList) {
							tile.GetComponentInChildren<ParticleSystem> ().Play ();
						}
					}
				} else {
					foreach (Tile tile in space.tileList) {
						tile.GetComponentInChildren<ParticleSystem> ().Play ();
					}
				}
				selectedSpace = space;
				mode = "Queue Spill";
				Vector3 topTileLocation = selectedSpace.tileList [selectedSpace.tileList.Count - 1].transform.position;
				Destroy (spillUI);
				spillUI = Instantiate (spillUIPrefab, 
					new Vector3 (topTileLocation.x-9.2f, topTileLocation.y + 9.2f, topTileLocation.z-9.2f), Quaternion.identity) as GameObject;
			}
		}
		return stackSelected;
	}

	void SelectTile(Ray ray){
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, spawnedTileLayer)) {
			mode = "Place Tile 0";
			spawnedTile.GetComponentInChildren<ParticleSystem> ().Play ();
		}
	}

	void PlaceTile(Ray ray){
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, topTiles)) {
			mode = "Place Tile 1";
			spawnedTile.transform.parent = null;
			Vector3 pointOnBoard = hit.transform.position;
			spawnedTile.transform.position = new Vector3 (pointOnBoard.x, pointOnBoard.y + 0.2f, pointOnBoard.z);
		}
	}

	void FinalizeTilePlacement(){
		BoardSpace space = CalculateSpaceFromLocation (spawnedTile.transform.position);
		space.AddTile (spawnedTile);
		spawnedTile.GetComponent<MeshRenderer> ().sortingOrder = 0;
		spawnedTile.GetComponentInChildren<ParticleSystem> ().Stop ();
		spawnedTile.GetComponentInChildren<ParticleSystem> ().Clear ();
		mode = "Select Stack";
		if (!firstTileFinalized) {
			firstTileFinalized = true;
			if ((space.colNum == 0) && (space.rowNum != 0)) {
				boardManager.sideAboutToCollapse = 0;
			} else if (space.rowNum == 5) {
				boardManager.sideAboutToCollapse = 1;
			} else if (space.colNum == 5) {
				boardManager.sideAboutToCollapse = 2;
			} else {
				boardManager.sideAboutToCollapse = 3;
			}
		}
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

	void InitQueueSpill(Ray ray){
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, spillUILayer)) {
			spillUI.SetActive (false);
			int xDirection = 0;
			int zDirection = 0;
			if (hit.collider.transform.name == "MinusX") {
				xDirection = -1;
			} else if (hit.collider.transform.name == "PlusX"){
				xDirection = 1;
			}else if (hit.collider.transform.name == "MinusZ") {
				zDirection = -1;
			} else if (hit.collider.transform.name == "PlusZ"){
				zDirection = 1;
			}
			boardManager.QueueSpill (selectedSpace, xDirection, zDirection);
			mode = "Finalize Spill";
		}
	}

	public void UndoQueueSpill(){
		spillUI.SetActive (true);
		boardManager.spaceQueuedToSpillFrom.ResetTilesToPosition ();
		mode = "Queue Spill";
	}

	void FinalizeSpill(){
		foreach (Tile tile in boardManager.tilesQueuedToSpill) {
			ParticleSystem ps = tile.GetComponentInChildren<ParticleSystem> ();
			ps.Stop ();
			ps.Clear();
		}
		boardManager.Spill ();
		mode = "Spawn Tile";
		StartCoroutine (InitSideCollapse());
	}

	IEnumerator InitSideCollapse(){
		yield return new WaitForSeconds (2);
		boardManager.CollapseSide ();
	}

	BoardSpace CalculateSpaceFromLocation(Vector3 location){
		int col = Mathf.RoundToInt (location.x - 0.5f + boardManager.numCols / 2);
		int row = Mathf.RoundToInt (location.z - 0.5f + boardManager.numRows / 2);
		int[] coords;
		coords = new int[2];
		coords [0] = col;
		coords [1] = row;
		return boardManager.board[coords[0], coords[1]];
	}
}
