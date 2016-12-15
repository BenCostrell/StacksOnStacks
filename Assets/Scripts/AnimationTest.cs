using UnityEngine;
using System.Collections;

public class AnimationTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = new Vector3 (transform.position.x,-0.13f, transform.position.z );
		//transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, 0.5f);
		//iTween.PunchPosition (transform.gameObject, iTween.Hash ("amount", Vector3.forward,"time", 2, "delay", 0f));
		//iTween.PunchScale (transform.gameObject, iTween.Hash ("amount", Vector3.up,"time", 2, "delay", 0f));
		
	}
	void LateUpdate(){
		transform.position = new Vector3 (transform.position.x, transform.position.y, -3f);
	}
}
