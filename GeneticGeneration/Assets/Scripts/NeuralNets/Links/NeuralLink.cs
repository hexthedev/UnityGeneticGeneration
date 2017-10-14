using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NeuralLink {

	//Holds the weights between matricies
	IInputable m_input;
	IRecievable m_output;
	Matrix m_weights;

	//Create with random weights
	public NeuralLink(IInputable p_input, IRecievable p_output){
		m_input = p_input;
		m_output = p_output;

		m_weights = new Matrix(p_output.count(), p_input.count());
	}

	//Create with specified weights. Weights must be right size
	public NeuralLink(IInputable p_input, IRecievable p_output, Matrix p_weights){
		m_input = p_input;
		m_output = p_output;

//		Debug.Log("Creating Link: \n" + p_input.count() + " : " + p_output.count() + "\n Weights \n" + p_weights);

		//Inputs = Rows, Outputs = columns
		if(m_input.count() != p_weights.numRows() || m_output.count() != p_weights.numColumns()){
			Debug.LogError("WEIGHTS INCORRECT SIZE FOR LINK");
		}

		m_weights = p_weights.clone();
	}
	
	//Neural Link Function does one propagation step
	public void propagate(){		
		//Get inputs
		Matrix propagation = m_input.getInputs();
		
//		Debug.Log("Propagation Weights \n" + m_weights);

		//multiply by weights to get output
		m_output.recievePropagation( propagation * m_weights );
	}

	//Links are stored as weight arrays. Not necessary for structure, but for evolution
	public Matrix dnaify(){
		return m_weights.clone();
	}
}
