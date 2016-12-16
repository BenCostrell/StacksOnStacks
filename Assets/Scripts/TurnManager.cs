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
	public LayerMask invisibleBoardPlaneLayer;
	public string mode;
	public GameObject mainCamera;
	public GameObject pivotPoint;
	public GameObject spillUIPrefab;
	public GameObject spillUI;
	public GameObject GameOverUI;
	private bool firstTileFinalized;
	private int numSidesCollapsed;
	public bool anythingTweening;
	private bool tileInPosition;

	public GameObject juicyManagerObj;
	private JuicyManager juicyManager;

	private bool collapseSideIndicated;

	// Use this for initialization
	void Start () {
		boardManager = boardManagerObj.GetComponent<BoardManager> ();
		mode = "Spawn Tile"; //spawn tile, select tile, place tile 0 (you haven't placed it anywhere yet), place tile 1 (you've placed it somewhere, but not finalized), select stack
		rotationIndex = 0;
		firstTileFinalized = false;
		numSidesCollapsed = 0;
		GameOverUI.SetActive (false);
		anythingTweening = false;
		tileInPosition = false;

		juicyManager = juicyManagerObj.GetComponent<JuicyManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		IsAnythingTweening ();
		if (mode == "Game Over") {
			GameOverUI.SetActive (true);
		} else if (!anythingTweening) { 
			if (mode == "Spawn Tile") {
				if (juicyManager.finishedintro) {
					DrawTileToPlace ();
					mode = "Select Tile";
					if (!collapseSideIndicated && firstTileFinalized) {
						List<BoardSpace> boardspaces = boardManager.GetSpaceListFromSideNum ();
						foreach (BoardSpace bs in boardspaces) {
							bs.gameObject.GetComponent<Renderer> ().material = boardManager.mats [7];
						}
						collapseSideIndicated = true;
					}

				}
			}
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Input.GetMouseButton (0)) {
				if (mode == "Place Tile") {
					PlaceTile (ray);
				}
			}
			if (Input.GetMouseButtonUp (0)) {
				if (mode == "Place Tile") {
					if (tileInPosition) {
						FinalizeTilePlacement ();
						tileInPosition = false;
					} else {
						SetupSpawnedTile (spawnedTile);
						mode = "Select Tile";
						ToggleGlow (spawnedTile, false);
					}
				}
			}

			if (Input.GetMouseButtonDown (0)) {
				if (mode == "Select Tile") {
					SelectTile (ray);
				} else if (mode == "Select Stack") {
					SelectStack (ray);
				} else if (mode == "Queue Spill") {
					if (!SelectStack (ray)) {
						InitQueueSpill (ray);
					}
				} else if (mode == "Finalize Spill") {
					collapseSideIndicated = false;
				}
			}
		}
	}

	bool SelectStack(Ray ray){
		bool stackSelected = false;
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, topTilesAndSpillUI)) {
			if (hit.collider.transform.tag == "Tile") {
				Vector3 tileHitLocation = hit.transform.position;
				BoardSpace space = CalculateSpaceFromLocation (tileHitLocation);
				if (space.tileList.Count > 1) {
					stackSelected = true;
					if (selectedSpace != null) {
						if (selectedSpace != space) {
							ToggleGlow (selectedSpace.tileList, false);
							ToggleGlow (space.tileList, true);
						}
					} else {
						ToggleGlow (space.tileList, true);
					}
					selectedSpace = space;
					mode = "Queue Spill";
					Vector3 topTileLocation = selectedSpace.tileList [selectedSpace.tileList.Count - 1].transform.position;
					Destroy (spillUI);
					spillUI = Instantiate(spillUIPrefab,
						new Vector3 (topTileLocation.x, topTileLocation.y, topTileLocation.z), Quaternion.identity) as GameObject;
					spillUI.transform.eulerAngles = new Vector3 (0, rotationIndex * 90, 0);
					spillUI.transform.GetChild (0).transform.localEulerAngles = new Vector3 (0,-rotationIndex * 90,0);
				}
			}
		}
		return stackSelected;
	}

	void ToggleGlow(List<Tile> tiles, bool on){
		if (on) {
			foreach (Tile tile in tiles) {
				tile.transform.GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
			}
		} else {
			foreach (Tile tile in tiles) {
				tile.transform.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
			}
		}
	}

	void ToggleGlow(Tile tile, bool on){
		List<Tile> tiles = new List<Tile> ();
		tiles.Add (tile);
		ToggleGlow(tiles, on);
	}

	void SelectTile(Ray ray){
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, spawnedTileLayer)) {
			mode = "Place Tile";
			ToggleGlow(spawnedTile, true);
		}

	}

	void PlaceTile(Ray ray){
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, topTiles)) {
			if (!CalculateSpaceFromLocation (hit.collider.transform.position).isCenterTile) {
				Vector3 pointOnBoard = hit.transform.position;
				spawnedTile.transform.position = new Vector3 (pointOnBoard.x, pointOnBoard.y + 0.2f, pointOnBoard.z);
				spawnedTile.transform.parent = null;
				tileInPosition = true;
			}
		} else {
			tileInPosition = false;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, invisibleBoardPlaneLayer)){
				spawnedTile.transform.position = hit.point;
			}
		}
	}

	public void FinalizeTilePlacement(){
		BoardSpace space = CalculateSpaceFromLocation (spawnedTile.transform.position);
		space.AddTile (spawnedTile, false);
		spawnedTile.GetComponent<MeshRenderer> ().sortingOrder = 0;
		ToggleGlow (spawnedTile, false);
		boardManager.CheckForScore ();
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

			List<BoardSpace> boardspaces = boardManager.GetSpaceListFromSideNum ();
			foreach (BoardSpace bs in boardspaces) {
				bs.gameObject.GetComponent<Renderer> ().material = boardManager.mats [7];
			}
		}
		mode = "Select Stack";
		spawnedTile.GetComponent<AudioSource> ().Play ();

	}


	void DrawTileToPlace(){
		Tile tileToPlace;
		tileToPlace = boardManager.DrawTile ();
		SetupSpawnedTile (tileToPlace);
		spawnedTile = tileToPlace;
		spawnedTile.GetComponent<MeshRenderer> ().sortingOrder = 2;
	}

	void SetupSpawnedTile(Tile tileToPlace){
		tileToPlace.transform.SetParent (pivotPoint.transform);
		tileToPlace.transform.localPosition = new Vector3 (-5, 0, 0);
		tileToPlace.gameObject.layer = LayerMask.NameToLayer ("DrawnTile");
		juicyManager.spawnTileAnimation (tileToPlace.gameObject);
	}

	void InitQueueSpill(Ray ray){
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, spillUILayer)) {
			int xDirection = 0;
			int zDirection = 0;
			if (hit.collider.transform.name == "MinusX") {
				xDirection = -1;
			} else if (hit.collider.transform.name == "PlusX") {
				xDirection = 1;
			} else if (hit.collider.transform.name == "MinusZ") {
				zDirection = -1;
			} else if (hit.collider.transform.name == "PlusZ") {
				zDirection = 1;
			}
			boardManager.QueueSpill (selectedSpace, xDirection, zDirection);
			mode = "Finalize Spill";
		} else {
			mode = "Select Stack";
			ToggleGlow (selectedSpace.tileList, false);
			selectedSpace = null;
		}
		spillUI.SetActive (false);
	}

	public void UndoQueueSpill(){
		spillUI.SetActive (true);
		boardManager.spaceQueuedToSpillFrom.ResetTilesToPosition ();
		foreach (Tile tile in boardManager.tilesQueuedToSpill) {
			tile.spaceQueuedToSpillOnto.provisionalTileCount = tile.spaceQueuedToSpillOnto.tileList.Count;
		}
		boardManager.spaceQueuedToSpillFrom.provisionalTileCount = boardManager.spaceQueuedToSpillFrom.tileList.Count;
		spillUI.transform.eulerAngles = new Vector3 (0, rotationIndex * 90, 0);
		spillUI.transform.GetChild (0).transform.localEulerAngles = new Vector3 (0,-rotationIndex * 90,0);
		mode = "Queue Spill";
	}

	public void FinalizeSpill(){
		ToggleGlow (boardManager.tilesQueuedToSpill, false);
		mode = "Interim";
		boardManager.Spill (boardManager.tilesQueuedToSpill);
		boardManager.CheckForScore ();
		StartCoroutine (InitSideCollapse());
	}

	IEnumerator InitSideCollapse(){
		float wait;
		if (boardManager.scoring) {
			//wait = 2.5f;
			wait = juicyManager.waitForScoreAnimation;
			boardManager.scoring = false;
		} else {
			wait = 0.5f;
		}
		yield return new WaitForSeconds (wait);
		boardManager.CollapseSide ();
		yield return new WaitForSeconds (2f);
		boardManager.CheckForScore ();
		numSidesCollapsed += 1;
		if (numSidesCollapsed == 8) {
			yield return new WaitForSeconds (boardManager.totalSpillTime - 2f);
			mode = "Game Over";
		} else {
			mode = "Spawn Tile";
		}
		boardManager.totalSpillTime = 0f;
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

	void IsAnythingTweening(){
		bool tweenHappening = false;
		foreach (BoardSpace space in boardManager.board) {
			if (space != null) {
				if (space.gameObject.GetComponent<iTween> ()) {
					tweenHappening = true;
					break;
				}
				foreach (Tile tile in space.tileList) {
					if (tile.gameObject.GetComponent<iTween> ()) {
						tweenHappening = true;
						break;
					}
				}
			}
		}
		anythingTweening = tweenHappening;
	}
}
