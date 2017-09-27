using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class MoveAction : IAction  {
	
	BehaviourTree m_tree;
	Rigidbody2D m_rb;

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

	//LOADING
  public void load(BehaviourTree p_tree)
  {
    m_tree = p_tree;
		m_rb = m_tree.getActor().GetComponent<Rigidbody2D>();
		m_direction.load(m_tree);
  }

  public void unload()
  {
    m_tree = null;
		m_rb = null;
		m_direction.unload();
  }

	//BEHAVIOUR
	public bool act(){
		//Debug.Log("MoveAction: " + m_speed);

		float speed = m_tree.getActorController().getTrait(ETrait.SPEED, false);

		if(m_sudden){
			m_rb.velocity = m_direction.direction()*speed;
		} else {
			float cur_speed = m_rb.velocity.magnitude;
			
			if(m_tree.getActorController().getTrait(ETrait.SPEED, false) < cur_speed + speed){
				m_rb.velocity = m_direction.direction()*speed;
			} else {
				m_rb.velocity += m_direction.direction()*speed;
			}	
		}		

		m_timer += Time.deltaTime;

		return m_timer > m_timeLimit;
	}

  public void reset(){
		m_timer = 0;
	}

	public static IAction random(){
		float time_limit = Random.Range(0, 101) <= 60 ? 0 : Random.Range(0f, 2f);
		float speed = Random.Range(0f, 1f);
		bool sudden = Random.Range(0,2) == 0 ? true : false;
		IDirection direction = RandomGen.IDirection();

		return new MoveAction(time_limit, speed, sudden, direction);
	}

  public IAction clone()
  {
    return new MoveAction(m_timeLimit, m_speed_percentage, m_sudden, m_direction.clone());
  }
}
