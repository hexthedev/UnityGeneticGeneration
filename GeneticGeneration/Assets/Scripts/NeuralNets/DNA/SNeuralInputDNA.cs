using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SNeuralInputDNA {

	private ENeuralInput m_input_type;

	private float[] m_params;

	public SNeuralInputDNA(ENeuralInput p_input, float[] p_params){
		m_input_type = p_input;
		m_params = p_params;
	}

	public INeuralInput getNeuralInput(GameObject p_actor, ObjectLogger p_logger){
		switch(m_input_type){
			case ENeuralInput.DIRECTION: 
				return new NIDirection(p_actor, p_logger, (float[])m_params.Clone());
		}
		return null;
	}
	
}
