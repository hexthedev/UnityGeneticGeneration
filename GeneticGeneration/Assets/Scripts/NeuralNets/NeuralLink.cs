using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralLink {

	IPropagatable m_input;
	IRecievable m_output;

	float[,] m_weights;

	public NeuralLink(IPropagatable p_input, IRecievable p_output){
		m_input = p_input;
		m_output = p_output;

		m_weights = new float[p_output.count() , p_input.count()];
	}

	


}
