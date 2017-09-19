﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class Bullet : MonoBehaviour {

	private float m_damage;

	private string m_shooter;

	private float m_speed = 20;

	private Rigidbody2D m_rb;

  	public void Initalize( Vector2 p_direction, float p_damage, string p_shooter){
		m_rb = gameObject.GetComponent<Rigidbody2D>();
		this.m_rb.velocity = p_direction * m_speed;
		m_damage = p_damage;
		m_shooter = p_shooter;
	}

	// Update is called once per frame
	void OnBecameInvisible() {
		Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		//Debug.Log(coll.gameObject.tag);

		if(coll.gameObject.CompareTag(m_shooter)){
			return;
		}

		IDamagable[] damageables = coll.gameObject.GetComponents<IDamagable>();

		foreach(IDamagable damagable in damageables){
			damagable.damage(m_damage);
		}

		Destroy(gameObject);
	}

	public bool compareShooter(string shooter){
		return m_shooter == shooter;
	}

}