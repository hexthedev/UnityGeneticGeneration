using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class DirectionDetector : IDetector
{
	private GameObject m_actor;
	private GameObject m_target;
	private Vector2 m_direction;
	private float m_angle_threshold;

 
  public DirectionDetector(GameObject p_actor, GameObject p_target, Vector2 p_direction, float p_angle_threshold)
  {
		m_actor = p_actor;
		m_target = p_target;
		m_direction = p_direction;
		m_angle_threshold = p_angle_threshold;
  }

  public bool detect()
  {
    //Vector representing direction actor should be pointing
		Vector2 target_direction = m_target.transform.position - m_actor.transform.position;

		debug(m_direction, target_direction, Color.cyan, Time.deltaTime);

		return Vector2.Angle( target_direction, m_direction ) < m_angle_threshold;


  }


	public void debug(Vector3 p_actual_direction, Vector3 p_target_direction, Color p_color, float p_duration){
		
		Vector3 origin = m_actor.transform.position;
		Vector3 actual_direction = origin + p_actual_direction;
		Vector3 actual_direction_big = origin + p_actual_direction*5;

		Vector3 target_direction = origin + p_target_direction;
		Vector3 target_direction_big = origin + p_target_direction*5;
	

		Debug.DrawLine(origin, actual_direction_big, p_color, p_duration);
		Debug.DrawLine(origin, target_direction, p_color, p_duration);


		Debug.DrawLine(origin, origin+VectorCalc.rotateDirectionVector(p_actual_direction, m_angle_threshold), p_color, p_duration);
		Debug.DrawLine(origin, origin+VectorCalc.rotateDirectionVector(p_actual_direction, -m_angle_threshold), p_color, p_duration);

	}
}
