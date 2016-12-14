using UnityEngine;
using System.Collections;

public class ChangeRenderQueue : MonoBehaviour {

	public int renderQueuePosition;

	// Use this for initialization
	void Start () {
		GetComponent<Renderer> ().material.renderQueue = renderQueuePosition;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
