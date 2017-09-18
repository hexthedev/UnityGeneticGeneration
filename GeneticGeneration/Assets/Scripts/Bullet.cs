using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class Bullet : MonoBehaviour {

	public float m_speed = 20;

	private Rigidbody2D m_rb;
	
	public void Initalize( Vector2 p_direction ){
		m_rb = gameObject.GetComponent<Rigidbody2D>();
		this.m_rb.velocity = p_direction * m_speed;
	}

	// Update is called once per frame
    void OnBecameInvisible() {
        Destroy(gameObject);
    }

	void OnCollisionEnter2D(Collision2D coll) {
		if(coll.gameObject.CompareTag("Player")){
			return;
		}

		Debug.Log("Happens");
		Destroy(gameObject);
    }


}
