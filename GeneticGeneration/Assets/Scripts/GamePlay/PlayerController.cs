﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class PlayerController : MonoBehaviour, IDamagable {
	
	public GameController m_game_controller;	
	private ObjectLogger m_logger;
	
	public float m_hp = 100f;
	public float m_speed = 20;
	private Rigidbody2D m_rb;
	public GameObject m_bullet;

	private float shot_timer;
	public float shot_rate;

	// Use this for initialization
	void Start () {
		m_rb = gameObject.GetComponent<Rigidbody2D>();
		shot_timer = shot_rate;

		m_logger = m_game_controller.GetComponent<ObjectLogger>();
		m_logger.log(gameObject, EObjectTypes.PLAYER);
	}
	
	// Update is called once per frame
	void Update () {
		shot_timer += Time.deltaTime;

		m_rb.velocity = playerVelocityUpdate();

		if((shot_timer > shot_rate) && Input.GetKey(KeyCode.Mouse0)){
			shoot();
			shot_timer = 0;
		}

		if(Input.GetKeyDown(KeyCode.P)){
			m_logger.getByType(EObjectTypes.ENEMY);
		}

		if(Input.GetKeyDown(KeyCode.O)){
			m_logger.getAll();
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
		bullet.GetComponent<Bullet>().Initalize(direction, 15f, "Player", m_logger);
	}

  public void damage(float damage)
  {
    m_hp -= damage;
		if(m_hp <= 0 ){
			m_logger.unlog(gameObject, EObjectTypes.PLAYER);
			Destroy(gameObject);
		}
  }
}
