using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class PlayerController : MonoBehaviour {

	public float m_speed = 20;
	private Rigidbody2D m_rb;
	public GameObject m_bullet;

	// Use this for initialization
	void Start () {
		m_rb = gameObject.GetComponent<Rigidbody2D>();

	}
	
	// Update is called once per frame
	void Update () {
		m_rb.velocity = playerVelocityUpdate();

		if(Input.GetKeyDown(KeyCode.Mouse0)){
			shoot();
		}
	}

	Vector2 playerVelocityUpdate(){

		Vector2 toReturn = Vector2.zero;

		if(Input.GetKey(KeyCode.A)){
			toReturn += new Vector2(-1, 0);
		}

		if(Input.GetKey(KeyCode.W)){
			toReturn += new Vector2(0, 1);
		}


		if(Input.GetKey(KeyCode.D)){
			toReturn += new Vector2(1, 0);
		}

		if(Input.GetKey(KeyCode.S)){
			toReturn += new Vector2(0, -1);
		}
				
		return toReturn * m_speed;
	}

	void shoot(){
		GameObject bullet = Instantiate(m_bullet, gameObject.transform.position, Quaternion.identity);	
		Vector3 position  = Camera.allCameras[0].ScreenToWorldPoint(Input.mousePosition);
		Vector2 direction = VectorCalc.CalcVec3to2( position - gameObject.transform.position ).normalized;
		bullet.GetComponent<Bullet>().Initalize(direction);
	}
}
