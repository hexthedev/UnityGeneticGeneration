using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NeuralLink {

	IPropagatable m_input;
	IRecievable m_output;
	Matrix m_weights;

	public NeuralLink(IPropagatable p_input, IRecievable p_output){
		m_input = p_input;
		m_output = p_output;

		m_weights = new Matrix(p_output.count(), p_input.count());
	}

	public NeuralLink(IPropagatable p_input, IRecievable p_output, Matrix p_weights){
		m_input = p_input;
		m_output = p_output;

		if(m_input.count() != p_weights.numColumns() || m_output.count() != p_weights.numRows()){

		}

		m_weights = p_weights.clone();
	}
	

	public void propagate(){	
		Matrix propagation = m_input.propagate();
		Debug.Log(m_weights);
		m_output.recievePropagation( propagation * m_weights );
	}

	public Matrix dnaify(){
		return m_weights.clone();
	}
}
