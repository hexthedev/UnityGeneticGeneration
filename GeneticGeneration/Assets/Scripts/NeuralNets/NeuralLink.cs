﻿using System.Collections;
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

	public NeuralLink(IPropagatable p_input, IRecievable p_output, float[,] p_weights){
		m_input = p_input;
		m_output = p_output;

		m_weights = new Matrix(p_weights);
	}
	

	public void propagate(){	
		Matrix propagation = m_input.propagate();
		Debug.Log(m_weights);
		m_output.recievePropagation( propagation * m_weights );
	}
}
