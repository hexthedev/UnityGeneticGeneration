using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class MoveAction : VSequenceAction  {
	
	Rigidbody2D m_rb;
	EnemyController m_controller;
	IDirection m_direction;

	float m_timer = 0;
	float m_timeLimit;
	float m_speed_percentage;
	bool m_sudden;

	public MoveAction(float p_timerLimit, float p_speed_percentage, bool p_sudden, IDirectionGenoType p_direction, ActionSequence p_sequence):base(p_sequence){
		m_timeLimit = p_timerLimit;
		m_speed_percentage = p_speed_percentage;
		m_direction = p_direction.phenotype(this);
		m_sudden = p_sudden;

		m_rb = m_sequence.getTree().getActorBody();
		m_controller = m_sequence.getTree().getActorController();
	}
	//BEHAVIOUR
	public override bool performAction(){
		//Debug.Log("MoveAction: " + m_speed);

		float speed = m_controller.getTrait(ETrait.SPEED, false) * m_speed_percentage;

		if(m_sudden){
			m_rb.velocity = m_direction.direction()*speed;
		} else {
			float cur_speed = m_rb.velocity.magnitude;
			
			if(m_controller.getTrait(ETrait.SPEED, false) < cur_speed + speed){
				m_rb.velocity = m_direction.direction()*speed;
			} else {
				m_rb.velocity += m_direction.direction()*speed;
			}	
		}		

		m_timer += Time.deltaTime;

		return m_timer > m_timeLimit;
	}

  public override void reset(){
		m_timer = 0;
	}

}
