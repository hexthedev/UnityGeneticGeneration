using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class MoveAction : IAction  {
	
	IDirection m_direction;

	float m_timer = 0;
	float m_timeLimit;

	float m_speed;

	public MoveAction(float p_timerLimit, float p_speed, IDirection p_direction){
		m_timeLimit = p_timerLimit;
		m_speed = p_speed;
		m_direction = p_direction;
	}

	public bool act(GameObject p_actor){
		Debug.Log("MoveAction: " + m_speed);

		p_actor.GetComponent<Rigidbody2D>().velocity = m_direction.direction()*m_speed;

		m_timer += Time.deltaTime;

		return m_timer > m_timeLimit;
	}

	public void reset(){
		m_timer = 0;
	}

}
