using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class PointingAtDetector : IDetector {

	GameObject m_actor;
	float m_forward;
	float m_threshold;
	GameObject m_pointing_at;

  public PointingAtDetector(GameObject p_actor, float p_forward, GameObject p_pointing_at, float p_threshold)
  {
		m_actor = p_actor;
		m_forward = p_forward;
		m_pointing_at = p_pointing_at;
		m_threshold = p_threshold;
  }

  public bool detect()
  {
    //Vector Representing direction actor is pointing
		Vector2 forward_vector = VectorCalc.forwardVector(m_actor.transform.rotation.eulerAngles.z, m_forward);
		//Vector representing direction actor should be pointing
		Vector2 pointing_vector = m_pointing_at.transform.position - m_actor.transform.position;

		return Vector2.Angle( forward_vector, pointing_vector ) < m_threshold;
  }

}
