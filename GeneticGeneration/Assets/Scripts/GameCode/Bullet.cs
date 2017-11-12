using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.Events;
using JTools.DataStructures.ObjectLogger;

public class Bullet : MonoBehaviour {

//	GameController m_game_controller;

	private float m_damage;

  private GameObject m_shooter;

	public float m_speed = 20;

	private Rigidbody2D m_rb;
	private Vector3 m_standard_velocity;

	private TimeoutEventManager m_timeout;

	public void Initalize( Vector2 p_direction, float p_damage, GameObject p_shooter){
		m_rb = gameObject.GetComponent<Rigidbody2D>();
		
		m_standard_velocity = p_direction * m_speed;
		m_damage = p_damage;
		m_shooter = p_shooter;

		ObjectLogger.log(gameObject, "BULLET");

		m_timeout = new TimeoutEventManager();
		
		m_timeout.addTimeout(10f, ()=>{ ObjectLogger.unlog(gameObject,"BULLET"); Destroy(gameObject);  });
	}

	// Update is called once per frame
	void FixedUpdate(){
		m_rb.velocity = m_standard_velocity;
		m_timeout.tick(Time.fixedDeltaTime);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		IDamagable[] damageables = coll.gameObject.GetComponents<IDamagable>();

		foreach(IDamagable damagable in damageables){
			damagable.damage(m_damage);
		}

		ObjectLogger.unlog(gameObject, "BULLET");
		Destroy(gameObject);
	}

}
