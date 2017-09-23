using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class ProximityDetector : IDetector {
  
	private GameObject m_actor;
	private GameObject m_target;
	private float m_threshold;

	public ProximityDetector(GameObject p_actor, GameObject p_target, float p_threshold){
		m_actor = p_actor;
		m_target = p_target;
		m_threshold = p_threshold;
	}

	public bool detect()
  {
    return (VectorCalc.CalcVec3to2(m_target.transform.position) - VectorCalc.CalcVec3to2(m_actor.transform.position)).magnitude <= m_threshold;
  }
}
