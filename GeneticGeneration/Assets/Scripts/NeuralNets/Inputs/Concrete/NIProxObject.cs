using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NIProxObject : INeuralInput {

	private CreatureController m_controller;

	private EObjectTypes m_object;

	private float m_scale;

	private int m_order_by_proximity;


	public NIProxObject (CreatureController p_controller, float[] p_params){
		m_controller = p_controller;
		m_scale = p_params[0];
		m_object = (EObjectTypes)(int)p_params[1];
		m_order_by_proximity = (int)p_params[2];

//		Debug.Log("PROX: " + m_object);
	}

    public SNeuralInputDNA dnaify()
    {
        float[] l_params = {m_scale};
		return new SNeuralInputDNA(ENeuralInput.PROXPLAYER, l_params);
    }

    public float input()
    {	
		if(!m_controller.senseExistsObject(m_object, m_order_by_proximity)) { return 0; }

		float object_prox = (m_controller.sensePosition() - m_controller.senseVisibleObjects(m_object)[m_order_by_proximity].transform.position).magnitude;
	
		// Debug.DrawLine(m_controller.sensePosition(), m_controller.senseNearestObjectPosition(m_object), Color.red, Time.fixedDeltaTime );
	
		float input = (m_scale/object_prox)/m_scale;
		// Debug.Log(m_object + " " + m_order_by_proximity + " " +  input);

		return input;
    }
}
