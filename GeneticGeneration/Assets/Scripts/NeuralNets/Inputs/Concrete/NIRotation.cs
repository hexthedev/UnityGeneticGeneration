using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NIRotation : INeuralInput {

	private CreatureController m_controller;

	private EObjectTypes m_object;

	private int m_order_by_proximity;

	//PARAMS {ObjType, NthObject}
	public NIRotation (CreatureController p_controller, float[] p_params){
		if(p_params.Length != 2) Debug.LogError("INVLAID PROXIMITY PARAMS");
		
		m_controller = p_controller;
		m_object = (EObjectTypes)(int)p_params[0];
		m_order_by_proximity = (int)p_params[1];

//		Debug.Log("PROX: " + m_object);
	}

    public SNeuralInputDNA dnaify()
    {
        float[] l_params = {(int)m_object, m_order_by_proximity};
		return new SNeuralInputDNA(ENeuralInput.ROTATION, l_params);
    }

    public float input()
    {	
		if(!m_controller.senseExistsObject(m_object, m_order_by_proximity)) { return 0; }

		float rotation = m_controller.senseNthNearestObjectRotation(m_order_by_proximity, m_object);

		return rotation/180f;
    }
}
