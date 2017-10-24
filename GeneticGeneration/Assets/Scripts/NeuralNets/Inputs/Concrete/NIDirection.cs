using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NIDirection : INeuralInput {

	private CreatureController m_controller;

	private EObjectTypes m_object;
	private Vector2 m_direction;

	private int m_order_by_proximity;

	public NIDirection (CreatureController p_controller, float[] p_params){
		m_controller = p_controller;
		m_direction = new Vector2(p_params[0], p_params[1]);
		m_object = (EObjectTypes)(int)p_params[2];
		m_order_by_proximity = (int)p_params[3];

//		Debug.Log("DIRECTION: " + m_object);
	}

    public SNeuralInputDNA dnaify()
    {
        float[] l_params = {m_direction[0], m_direction[1]};
		return new SNeuralInputDNA(ENeuralInput.DIRECTION, l_params);
    }

    public float input()
    {
		if(!m_controller.senseExistsObject(m_object, m_order_by_proximity)) { return 0; }

		Vector3 object_direction = m_controller.sensePosition() - m_controller.senseNearestObjectPosition(m_object);
		return VectorCalc.getAngle( m_direction, object_direction )/180f;
    }
}
