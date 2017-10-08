using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NeuralLink {

	IPropagatable m_input;
	IRecievable m_output;

	float[,] m_weights;

	public NeuralLink(IPropagatable p_input, IRecievable p_output){
		m_input = p_input;
		m_output = p_output;

		m_weights = new float[p_output.count(), p_input.count()];

		MatrixCalc.randomPopulate(m_weights);
	}

	public void propagate(){	
		
		float[,] propagation = m_input.propagate();
		Debug.Log(MatrixCalc.String(m_weights));
		m_output.recievePropagation( MatrixCalc.Multiply( propagation , m_weights) );
	}
}
