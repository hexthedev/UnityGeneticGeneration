using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class MoveAction : IAction  {
	
	IDirection m_direction;

	float m_timer = 0;
	float m_timeLimit;

	float m_speed_percentage;

	bool m_sudden;

	public MoveAction(float p_timerLimit, float p_speed_percentage, bool p_sudden, IDirection p_direction){
		m_timeLimit = p_timerLimit;
		m_speed_percentage = p_speed_percentage;
		m_direction = p_direction;
		m_sudden = p_sudden;
	}

	public bool act(BehaviourTree p_tree){
		//Debug.Log("MoveAction: " + m_speed);

		Rigidbody2D rb = p_tree.getActor().GetComponent<Rigidbody2D>();

		float speed = p_tree.getActorController().getTrait(ETrait.SPEED, false);

		if(m_sudden){
			rb.velocity = m_direction.direction()*speed;
		} else {
			float cur_speed = rb.velocity.magnitude;
			
			if(p_tree.getActor().GetComponent<EnemyController>().getTrait(ETrait.SPEED, false) < cur_speed + speed){
				rb.velocity = m_direction.direction()*speed;
			} else {
				rb.velocity += m_direction.direction()*speed;
			}	

		}		

		m_timer += Time.deltaTime;

		return m_timer > m_timeLimit;
	}


  public void reset(){
		m_timer = 0;
	}

	public static IAction random(BehaviourTree p_tree){
		float time_limit = Random.Range(0, 101) <= 60 ? 0 : Random.Range(0f, 2f);
		float speed = Random.Range(0f, 1f);
		bool sudden = Random.Range(0,2) == 0 ? true : false;
		IDirection direction = RandomGen.IDirection(p_tree);

		return new MoveAction(time_limit, speed, sudden, direction);
	}

}
