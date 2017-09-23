using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		return Vector2.Angle( target_direction, m_direction ) < m_angle_threshold;
  }
}
