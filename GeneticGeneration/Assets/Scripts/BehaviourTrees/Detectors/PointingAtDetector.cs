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

		//debug(forward_vector, pointing_vector, Color.black, Time.deltaTime);

		return Vector2.Angle( forward_vector, pointing_vector ) < m_threshold;
  }


	public void debug(Vector3 p_forward, Vector3 p_pointing, Color p_color, float duration){
		
		Vector3 origin = m_actor.transform.position;

		Vector3 forward = m_actor.transform.position+(p_forward);
		Vector3 forwardBig = m_actor.transform.position+(p_forward)*5;
		
		Vector3 pointing = m_actor.transform.position+(p_pointing);
		Vector3 pointingNormal = m_actor.transform.position+(p_pointing.normalized);
		
		
		Debug.DrawLine(origin, forwardBig, p_color, duration);
		Debug.DrawLine(origin, pointing, p_color, duration);
		Debug.DrawLine(forward, pointingNormal, p_color, duration);
	}

}
