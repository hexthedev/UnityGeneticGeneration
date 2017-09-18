using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class EnemyController : MonoBehaviour {

	private float m_attack;
	private float m_defense;
	private float m_speed;
	private float m_hp;

	private float m_forward = -90;

	private GameObject m_player;

	// Use this for initialization
	void Start () {
		m_player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
		gameObject.transform.rotation *= Quaternion.AngleAxis(VectorCalc.getAngle(vecToPlayer(), forwardVector()) * m_speed/10, new Vector3(0,0,1));
		gameObject.GetComponent<Rigidbody2D>().velocity = VectorCalc.CalcVec3to2(vecToPlayer())*m_speed;

	}

	public void Initalize(float p_attack, float p_defense, float p_speed, float p_hp){
		//All passed in float are between 1 and 10. They need converting for proper values
		m_attack = p_attack;
		m_defense = p_defense/3;
		m_speed = p_speed;
		m_hp = p_hp*3;
	}


	private Vector2 forwardVector(){
		return VectorCalc.fromAngle(gameObject.transform.rotation.eulerAngles.z + m_forward);
	}

	private Vector3 vecToPlayer(){
		return (m_player.transform.position - gameObject.transform.position).normalized;	
	}

	void OnCollisionEnter2D(Collision2D coll) {
		Destroy(gameObject);
	}

	
}
