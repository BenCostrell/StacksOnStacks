using UnityEngine;
using System.Collections;

public class AnimationTest : MonoBehaviour {

	public GameObject prefab;

	// Use this for initialization
	void Start () {

		GameObject pre = Instantiate (prefab,
			                 prefab.transform.position, Quaternion.identity) as GameObject;
		pre.transform.SetParent (GameObject.FindWithTag ("UICanvas").transform, false);

		GameObject pre2 = Instantiate (prefab,
			new Vector3(prefab.transform.position.x+60f, prefab.transform.position.y,
				prefab.transform.position.z), Quaternion.identity) as GameObject;
		pre2.transform.SetParent (GameObject.FindWithTag ("UICanvas").transform, false);

		GameObject pre3 = Instantiate (prefab,
			new Vector3(prefab.transform.position.x+60f*2f, prefab.transform.position.y,
				prefab.transform.position.z), Quaternion.identity) as GameObject;
		pre3.transform.SetParent (GameObject.FindWithTag ("UICanvas").transform, false);

	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = new Vector3 (transform.position.x,-0.13f, transform.position.z );
		//transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, 0.5f);
		//iTween.PunchPosition (transform.gameObject, iTween.Hash ("amount", Vector3.forward,"time", 2, "delay", 0f));
		//iTween.PunchScale (transform.gameObject, iTween.Hash ("amount", Vector3.up,"time", 2, "delay", 0f));
	}
	void LateUpdate(){
		//transform.position = new Vector3 (transform.position.x, transform.position.y, -3f);
	}
}
