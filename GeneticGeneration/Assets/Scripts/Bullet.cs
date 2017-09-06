using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	private float m_speed = 500f;

	private Rigidbody2D m_rb;

	// Use this for initialization
	void Start () {
	 m_rb = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		Vector3 angle = gameObject.transform.rotation.ToEuler();
		m_rb.velocity = new Vector2(angle.x * m_speed, angle.y*m_speed);
	}

}
