using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralDNA {

	SNeuralInputDNA[] m_inputs;

	List<DActivationFunction[]> m_links;

	Matrix[] m_weights;

	SNeuralOutputDNA[] m_outputs;

	public NeuralDNA( SNeuralInputDNA[] p_inputs, Matrix[] p_weights, SNeuralOutputDNA[] p_outputs ){
		m_inputs = (SNeuralInputDNA[])p_inputs.Clone();
		m_weights = (Matrix[])p_weights.Clone();
		m_outputs = (SNeuralOutputDNA[])p_outputs.Clone();
	}

	//TEST DNA
	public NeuralDNA(){
		//INputs and outputs are simple
		SNeuralInputDNA[] inputs = { new SNeuralInputDNA(), new SNeuralInputDNA(), new SNeuralInputDNA() };
		m_inputs = inputs;
		SNeuralOutputDNA[] outputs = { new SNeuralOutputDNA(), new SNeuralOutputDNA(), new SNeuralOutputDNA() };
		m_outputs = outputs;

		//Want to know the number of links
		int links = Random.Range(2,5);
		
		m_links = new List<DActivationFunction[]>();
		//Links are indicated using their activator arrays
		for(int i = 0; i<links-1; i++){
			m_links.Add(Activators.randomArray());
		}

		m_links.Add(Activators.randomArrayOfSize(outputs.Length));

		//Now need weight arrays which rep the weightings used in the links
		m_weights = new Matrix[m_links.Count];

		//The first weights are hidden layer x input matrix
		m_weights[0] = new Matrix(m_links[0].Length, m_inputs.Length);

		//The following weights are next layer x last layer matrix
		for(int i = 0; i<m_links.Count-1; i++){
			m_weights[i+1] = new Matrix(m_links[i+1].Length, m_links[i].Length);
		}

		//The last weights are the output x last layer matrix
		m_weights[m_weights.Length-1] = new Matrix(m_outputs.Length, m_links[m_links.Count-2].Length);

		Debug.Log(m_inputs.Length);
		
		string x = "[";

		foreach(DActivationFunction[] array in m_links){
			x+=array.Length+",";
		}

		x+="]";
				
		Debug.Log(x);

		x = "[";

		foreach(Matrix mat in m_weights){
			x+=mat.numColumns()+"x"+mat.numRows()+",";
		}

		x+="]";
				
		Debug.Log(x);

		Debug.Log(m_outputs.Length);
		
	}

	public NeuralInputLayer inputLayer(){
		NeuralInput[] input = new NeuralInput[m_inputs.Length];

		for(int i = 0; i<input.Length; i++){
			input[i] = m_inputs[i].getNeuralInput();
		}

		return new NeuralInputLayer(input);
	}

	public NeuralOutputLayer outputLayer(){
		NeuralOutput[] output = new NeuralOutput[m_outputs.Length];
		
		for(int i = 0 ; i<output.Length;i++){
			output[i] = new NeuralOutput();
		}

		return new NeuralOutputLayer(output, getOutputActivators());
	}

	public Matrix[] getWeights(){
		return m_weights;
	}

	public DActivationFunction[] getActivators(int p_link_index){
		return m_links[p_link_index];
	}

	public DActivationFunction[] getOutputActivators(){
		return m_links[ m_links.Count-1 ];		
	}

	public Matrix getWeights(int p_layer_index){
		return m_weights[p_layer_index];
	}

	public Matrix getOutputWeights(){
		return m_weights[m_weights.Length-1];
	}

	public int hiddenLayerCount(){
		return m_links.Count - 1; 
	}

	public int hiddenLayerSize(int p_layer_index){
		return m_links[p_layer_index].Length;
	}



}
