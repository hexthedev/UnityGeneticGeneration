using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralDNA {

	SNeuralInputDNA[] m_input;

	Matrix[] m_weights;

	SNeuralOutputDNA[] m_outputs;

	public NeuralDNA( SNeuralInputDNA[] p_inputs, Matrix[] p_weights, SNeuralOutputDNA[] p_outputs ){
		m_input = (SNeuralInputDNA[])p_inputs.Clone();
		m_weights = (Matrix[])p_weights.Clone();
		m_outputs = (SNeuralOutputDNA[])p_outputs.Clone();
	}

	public NeuralInputLayer inputLayer(){
		NeuralInput[] input = new NeuralInput[m_input.Length];

		for(int i = 0; i<input.Length; i++){
			input[i] = m_input[i].getNeuralInput();
		}

		return new NeuralInputLayer(input);
	}

	public NeuralOutputLayer outputLayer(){
		NeuralOutput[] output = new NeuralOutput[m_outputs.Length];
		DActivationFunction[] activators = new DActivationFunction[m_outputs.Length];

		for(int i = 0; i<m_outputs.Length; i++){
			switch(m_outputs[i].m_output){
				case ENeuralOutput.OUTPUT: 
					output[i] = new NeuralOutput();
					break;
			}

			activators[i] = m_outputs[i].m_activate; 
		}

		return new NeuralOutputLayer(output, activators);
	}

}
