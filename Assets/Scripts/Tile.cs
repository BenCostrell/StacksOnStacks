using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	private Renderer renderer;
	public BoardSpace spaceQueuedToSpillOnto;

	// Use this for initialization
	void Start () {
		renderer = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		float outlineSize = Camera.main.orthographicSize;
		renderer.material.SetFloat ("_Outline", outlineSize);
	}
}
