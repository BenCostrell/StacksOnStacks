using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JuicyManager : MonoBehaviour {


	BoardManager boardmanager;
	TurnManager turnmanager;
	UIManager uimanager;

	int countTileList;
	bool playTileSinkSound;
	public bool gameend;
	public float xSpillDir;
	public float zSpillDir;

	public bool finishedintro;
	public bool realfinishedintro;
	public bool scoring;

	float boardSpaceBeginY = -15.0f;
	float tileBeginY = 15.0f;

	public float delaySpaceCollapse;
	public int spaceCount;

	public float delayTileSpill;

	List<float> stackHeights;

	List<Vector3> centerPos;

	public float waitForScoreAnimation;
	public float scorePitch;

	public AudioClip scoreSfx;
	public AudioClip tilePlaceSfx;

	public bool boardSpaceEntered;

	public bool spawnTileEntry;

	GameObject soundplayer;

	bool collapseSideSpaceStarted;

	Color[,] juicyColors;
	Color[] lastColors;

	float fadetime = 1.0f;
	float tColor;


	// Use this for initialization
	void Start () {
		boardmanager = GameObject.FindWithTag ("BoardManager").GetComponent<BoardManager> ();
		turnmanager = GameObject.FindWithTag("TurnManager").GetComponent<TurnManager> ();
		uimanager = GameObject.FindWithTag ("UICanvas").GetComponent<UIManager> ();
		soundplayer = GameObject.FindWithTag ("SoundPlayer");
		stackHeights = new List<float> ();

		delaySpaceCollapse = 0f;
		spaceCount = 0;
		finishedintro = false;
		realfinishedintro = false;

		centerPos = new List<Vector3> ();

		waitForScoreAnimation = 2f;

		boardSpaceEntered = false;
		spawnTileEntry = true;

		juicyColors = new Color[5, 3];

		/*
		juicyColors[0,0] = new Color (0x6E, 0x36, 0X7D, 0xFF); //campurple
		juicyColors[0,1] = new Color(0x8D,0x83,0x90,0xFF); //purple1
		juicyColors[0,2] = new Color(0x84,0x7D,0x87,0xFF); //purple2
		juicyColors[1,0] = new Color(0x36,0x53,0x7D,0xFF); //camblue
		juicyColors[1,1] = new Color(0X5C,0x8E,0xAC,0xFF); //blue1
		juicyColors[1,2] = new Color(0x5B,0x86,0xA5,0xFF); //blue2
		juicyColors[2,0] = new Color(0x43,0x83,0x69,0xFF); //camgreen
		juicyColors[2,1] = new Color(0x5C,0xAC,0x77,0xFF); //green1
		juicyColors[2,2] = new Color(0x5B,0xA5,0x64,0xFF); //gren2
		juicyColors[3,0] = new Color(0x94,0x3B,0x5C,0xFF); //camred
		juicyColors[3,1] = new Color(0xB5,0x6D,0x75,0XFF); //red1
		juicyColors[3,2] = new Color(0xAE,0x60,0x71,0xFF); //red2
		juicyColors[4,0] = new Color(0xFB,0xC2,0x7B,0xFF); //camyellow
		juicyColors[4,1] = new Color(0xD9,0xBE,0x64,0xFF); //yellow1
		juicyColors[4,2] = new Color(0xD9,0xB7,0x5B,0xFF); //yellow2
*/

		juicyColors[0,0] = normalizeColors(110f,54f,126f); //campurple
		//juicyColors[0,1] = normalizeColors(); //purple1
		//juicyColors[0,2] = normalizeColors(); //purple2
		juicyColors[1,0] = normalizeColors(54f, 83f, 125f); //camblue
		/*juicyColors[1,1] = normalizeColors(); //blue1
		juicyColors[1,2] = normalizeColors(); //blue2*/
		juicyColors[2,0] = normalizeColors(67f, 131f, 105f); //camgreen
	/*	juicyColors[2,1] = normalizeColors(); //green1
		juicyColors[2,2] = normalizeColors(); //gren2*/
		juicyColors[3,0] = normalizeColors(148f,59f,92f); //camred
	/*	juicyColors[3,1] = normalizeColors(); //red1
		juicyColors[3,2] = normalizeColors(); //red2*/
		juicyColors[4,0] = normalizeColors(251f,194f,123f); //camyellow
	/*	juicyColors[4,1] = normalizeColors(); //yellow1
		juicyColors[4,2] = normalizeColors(); //yellow2*/

		lastColors = new Color[3];
		lastColors [0] = juicyColors [0, 0];
	/*	lastColors [1] = juicyColors [0, 1];
		lastColors [2] = juicyColors [0, 2];*/

	}

	void LateUpdate(){
		if (finishedintro && boardmanager.boardInitialized) {
			int count = 0;
			foreach (BoardSpace space in boardmanager.centerSpaces) {
				space.transform.position = new Vector3 (centerPos [count].x, space.transform.position.y, centerPos [count].z);
				count++;
			}
		}

		if (!spawnTileEntry) {
			if (turnmanager.mode == "Select Tile") {
				turnmanager.spawnedTile.gameObject.GetComponent<Animator> ().enabled = true;
			} else {
				turnmanager.spawnedTile.gameObject.GetComponent<Animator> ().enabled = false;
				spawnTileEntry = true;
				soundplayer.transform.GetChild (5).gameObject.GetComponent<AudioSource> ().Play ();
			}
			if (!realfinishedintro) {
				realfinishedintro = true;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (!finishedintro && boardmanager.boardInitialized) {
			introAnimation ();
			finishedintro = true;

			foreach (BoardSpace space in boardmanager.centerSpaces) {
				space.gameObject.GetComponent<Animator> ().enabled = true;
				centerPos.Add (space.transform.position);
			}
		}

		if (turnmanager.mode == "Spawn Tile") {
			if (lastColors [0] != juicyColors [0, 0]) {
				if (tColor < 1f) {
					tColor += Time.deltaTime / fadetime;
					Camera.main.backgroundColor = Color.Lerp (lastColors [0], juicyColors [0, 0], tColor);

				} else {
					lastColors [0] = juicyColors [0, 0];
					tColor = 0f;
				}
			}
		} else if (turnmanager.mode == "Place Tile") {
			if (lastColors [0] != juicyColors [1, 0]) {
				if (tColor < 1f) {
					tColor += Time.deltaTime / fadetime;
					Camera.main.backgroundColor = Color.Lerp (lastColors [0], juicyColors [1, 0], tColor);

				} else {
					lastColors [0] = juicyColors [1, 0];
					tColor = 0f;
				}
			}
		} else if (turnmanager.mode == "Select Stack") {
			if (lastColors [0] != juicyColors [2, 0]) {
				if (tColor < 1f) {
					tColor += Time.deltaTime / fadetime;
					Camera.main.backgroundColor = Color.Lerp (lastColors [0], juicyColors [2, 0], tColor);

				} else {
					lastColors [0] = juicyColors [2, 0];
					tColor = 0f;
				}
			}
		} else if (turnmanager.collapsingMode) {
			if (lastColors [0] != juicyColors [3,0]) {
				if (tColor < 1f) {
					tColor += Time.deltaTime / fadetime;
					Camera.main.backgroundColor = Color.Lerp (lastColors [0], juicyColors [3, 0], tColor);

				} else {
					lastColors [0] = juicyColors [3, 0];
					tColor = 0f;
				}
			}
		//} else if (turnmanager.mode == "Scoring") {
		} else if(turnmanager.scoringMode){
			if (lastColors [0] != juicyColors [4, 0]) {
				if (tColor < 1f) {
					tColor += Time.deltaTime / fadetime;
					Camera.main.backgroundColor = Color.Lerp (lastColors [0], juicyColors [4, 0], tColor);

				} else {
					lastColors [0] = juicyColors [4, 0];
					tColor = 0f;
				}
			}
		}


	}

	Color normalizeColors(float r, float g, float b){
		return new Color (r / 255f, g / 255f, b / 255f);

	}


	public void spawnTileAnimation(GameObject tile){

		soundplayer.transform.GetChild (4).gameObject.GetComponent<AudioSource> ().Play ();
		iTween.MoveFrom (tile, iTween.Hash(
			"position",new Vector3(-9,0,0),
			"islocal", true,
			"time", 0.5f,
			"easetype","easeOutElastic",
			"oncomplete","toggleSpawnTileAnim",
			"oncompletetarget",transform.gameObject,
			"oncompleteparams",tile
		));
		iTween.RotateTo (tile, iTween.Hash (
			"rotation", new Vector3 (0, 0, 0),
			"islocal", true,
			"time", 0.5f
		));

	}



	void toggleSpawnTileAnim(GameObject tile){

		if (turnmanager.mode == "Select Tile") {
			tile.GetComponent<Animator> ().enabled = true;
		} else {
			tile.GetComponent<Animator> ().enabled = false;
		}

		spawnTileEntry = false;
	}


	public void ScoreAnimation(){
		scorePitch = 1f;
		StartCoroutine (scoreAnimTiming ());
	}

	IEnumerator scoreAnimTiming(){
		foreach (BoardSpace space in boardmanager.centerSpaces) {
			space.gameObject.GetComponent<Animator> ().SetTrigger ("getScore");
			scorePitch += 0.3f;
			yield return new WaitForSeconds (0.3f);
		}
		GetComponent<AudioSource> ().clip = scoreSfx;
		GetComponent<AudioSource> ().PlayOneShot (scoreSfx, 1f);

	}

	public void PositionStackToSpill(BoardSpace space){
		List<Tile> tiles = space.tileList;
		if (tiles.Count > 0) {
			float topHeight = tiles [tiles.Count - 1].transform.position.y;
			foreach (Tile tile in space.tileList) {
				iTween.MoveBy (tile.gameObject, topHeight * Vector3.up, 0.5f);
			}
		}
	}

	public void EndGameJuice(){
		gameend = true;
		if (boardmanager.score > 0) {
			soundplayer.transform.GetChild (3).gameObject.GetComponent<AudioSource> ().Play ();

		} else {
			soundplayer.transform.GetChild (2).gameObject.GetComponent<AudioSource> ().Play ();
		}

		StartCoroutine (FadeOut (soundplayer.GetComponent<AudioSource>(), 0.5f));

	}

	IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
		float startVolume = audioSource.volume;
		float decreasevolume = 0.01f;
		while (audioSource.volume > 0) {
			audioSource.volume -= decreasevolume;
			yield return null;
		}

		audioSource.Stop ();
		audioSource.volume = startVolume;
	}

	public void TileSinkAnimation(GameObject tile, GameObject centertile){
		playTileAbsorbSound ();

		List<GameObject> go = new List<GameObject> ();
		go.Add (tile);
		go.Add (centertile);
		iTween.MoveTo (tile, iTween.Hash (
			"position",new Vector3(centertile.transform.position.x,centertile.transform.position.y-0.05f,centertile.transform.position.z),
			"time",0.2f
		));
		iTween.ScaleTo (tile, iTween.Hash (
			"scale", new Vector3 (0, 0, 0),
			"time", 0.05f,
			"delay", 0.15f,
			"oncomplete","destroyTileSink",
			"oncompletetarget",transform.gameObject,
			"oncompleteparams",go
		));

	}

	public void playTileAbsorbSound(){
		if (!soundplayer.transform.GetChild (7).gameObject.GetComponent<AudioSource> ().isPlaying) {
			soundplayer.transform.GetChild (7).gameObject.GetComponent<AudioSource> ().Play ();
		}
	}

	void destroyTileSink(List<GameObject> go){
		go[1].GetComponent<BoardSpace>().color = go[0].GetComponent<Tile>().color;
		Destroy (go[0]);
	}


	public void AnimateTileMove(Tile tile, int tileCount, Vector3 pos){
		if (turnmanager.mode == "Select Tile" || uimanager.undoSpill) {
			delayTileSpill = 0f;
			stackHeights.Clear ();
			uimanager.undoSpill = false;
		}

		if (finishedintro && (turnmanager.mode == "Queue Spill" || turnmanager.mode == "Interim")) {
			delayTileSpill += 0.4f;
			float tileTime = 1.0f;

			float midpointx = (pos.x + tile.transform.position.x) / 2.0f;
			float midpointz = (pos.z + tile.transform.position.z) / 2.0f;
			stackHeights.Add (tileCount * 0.2f + 0.1f);
			int highestStack = 0;

			for (int i = 0; i < stackHeights.Count; i++) {
				if (stackHeights [i] > stackHeights[highestStack]) {
					highestStack = i;
				}
			}
			float height = stackHeights[highestStack] + 1f;
			Vector3 midpoint = new Vector3(midpointx, height, midpointz);
			Vector3 endpoint = new Vector3 (pos.x, tileCount * 0.2f + 0.1f, pos.z);
			Vector3[] path = new Vector3[2]{ midpoint, endpoint };


			iTween.MoveTo (tile.gameObject, iTween.Hash (
				"position", new Vector3 (pos.x, tileCount * 0.2f + 0.1f, pos.z),
				"path",path,
				"time", tileTime,
				"delay", delayTileSpill

			));
			StartCoroutine(playTilePlaceSFX ());
			if (xSpillDir == 0 && zSpillDir == 1) { //up
				float vrot = 180.0f;
				iTween.RotateAdd (tile.gameObject, iTween.Hash (
					"amount", new Vector3 (vrot, 0, 0),
					"time", tileTime,
					"delay", delayTileSpill,
					"oncomplete", "setTileStraight",
					"oncompletetarget",transform.gameObject,
					"oncompleteparams", tile.gameObject
				));
			} else if (xSpillDir == 0 && zSpillDir == -1) { //down
				float vrot = -180.0f;
				iTween.RotateAdd (tile.gameObject, iTween.Hash (
					"amount", new Vector3 (vrot, 0,0),
					"time", tileTime,
					"delay", delayTileSpill,
					"oncomplete", "setTileStraight",
					"oncompletetarget",transform.gameObject,
					"oncompleteparams", tile.gameObject
				));
			} else if (xSpillDir == -1 && zSpillDir == 0) { //left
				float vrot = 180.0f;
				iTween.RotateAdd (tile.gameObject, iTween.Hash (
					"amount", new Vector3 (0, 0, vrot),
					"time", tileTime,
					"delay", delayTileSpill,
					"oncomplete", "setTileStraight",
					"oncompletetarget",transform.gameObject,
					"oncompleteparams", tile.gameObject
				));
			} else if (xSpillDir == 1 && zSpillDir == 0) { //right
				float vrot = -180.0f;
				iTween.RotateAdd (tile.gameObject, iTween.Hash (
					"amount", new Vector3 (0,0, vrot),
					"time", tileTime,
					"delay", delayTileSpill,
					"oncomplete", "setTileStraight",
					"oncompletetarget",transform.gameObject,
					"oncompleteparams", tile.gameObject
				));

			}

		} else {
			tile.transform.position = new Vector3 (pos.x, tileCount * 0.2f + 0.1f, pos.z);

		}
	}


	void endScoreAnimation(List<GameObject> scores){
		foreach(GameObject score in scores){
			iTween.PunchScale (score, iTween.Hash (
				"amount", new Vector3(0.05f,0.05f,0),
				"time", 2.5f,
				"looptype","loop"
			));
		}
	}

	void setTileStraight(GameObject go){
		go.transform.eulerAngles = new Vector3 (0, 0, 0);
	}

	IEnumerator playTilePlaceSFX(){
		yield return new WaitForSeconds (delayTileSpill+0.5f);
		GetComponent<AudioSource> ().clip = tilePlaceSfx;
		//GetComponent<AudioSource> ().PlayOneShot(tilePlaceSfx,1f);
		GetComponent<AudioSource> ().Play();
	}

	public void CollapseSideSpaces(GameObject go, int numOfSpaces){

		if (!collapseSideSpaceStarted) {
			soundplayer.transform.GetChild (1).gameObject.GetComponent<AudioSource> ().Play ();
			collapseSideSpaceStarted = true;
		}

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
			collapseSideSpaceStarted = false;
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

		boardSpaceEntered = true;

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