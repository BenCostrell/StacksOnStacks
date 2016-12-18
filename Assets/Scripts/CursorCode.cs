using UnityEngine;
using System.Collections;

public class CursorCode : MonoBehaviour {

	public CursorMode cursorMode = CursorMode.Auto;
	public Texture2D cursorTexture;
	Vector2 hotSpot = Vector2.zero;

	void Awake(){
		

	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseEnter(){
		Cursor.SetCursor (cursorTexture, hotSpot, cursorMode);
	}

	void OnMouseExit() {
		Cursor.SetCursor(null, Vector2.zero, cursorMode);
	}
}
