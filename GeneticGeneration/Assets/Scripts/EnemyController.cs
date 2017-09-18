using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class EnemyController : MonoBehaviour {

	[Range(0,1)]
	public float m_rotation_speed = 0.1f;

	private float m_forward = -90;

	private GameObject m_player;

	// Use this for initialization
	void Start () {
		m_player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
		//if(Input.GetKeyDown(KeyCode.Space)){
			
			Vector3 vecToPlayer = (m_player.transform.position - gameObject.transform.position).normalized;	
			gameObject.transform.rotation *= Quaternion.AngleAxis(VectorCalc.getAngle(vecToPlayer, forwardVector()) * m_rotation_speed, new Vector3(0,0,1));
		//}

		


	}


	private Vector2 forwardVector(){
		return VectorCalc.fromAngle(gameObject.transform.rotation.eulerAngles.z + m_forward);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		Debug.Log("Happens");
		Destroy(gameObject);
	}

	
}
