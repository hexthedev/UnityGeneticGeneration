using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorLevel : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if(gameObject.transform.position.x > 12.5){
			gameObject.transform.position = new Vector3(-12.8f,gameObject.transform.position.y,gameObject.transform.position.z);
		}

		if(gameObject.transform.position.x < -13.3){
			gameObject.transform.position = new Vector3(12f,gameObject.transform.position.y,gameObject.transform.position.z);
		}

		if(gameObject.transform.position.y > 8.8){
			gameObject.transform.position = new Vector3(gameObject.transform.position.x, -8.3f,gameObject.transform.position.z);
		}

		if(gameObject.transform.position.y < -8.8){
			gameObject.transform.position = new Vector3(gameObject.transform.position.x, 8.3f,gameObject.transform.position.z);
		}

	}
}
