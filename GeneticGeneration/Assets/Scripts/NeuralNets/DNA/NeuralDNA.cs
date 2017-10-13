using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NeuralDNA {

	SNeuralInputDNA[] m_inputs;	//Represents the number of inputs in input layer
	List<DActivationFunction[]> m_hiddens;		//Represents the number of hidden layers. Activation functions act on hidden layer
	Matrix[] m_links;			//Represents the weight between two layers in order
	SNeuralOutputDNA[] m_outputs;		//Represents the outputs in the output layer

	public NeuralDNA( SNeuralInputDNA[] p_inputs, List<DActivationFunction[]> p_hiddens, Matrix[] p_weights, SNeuralOutputDNA[] p_outputs ){
		m_inputs = (SNeuralInputDNA[])p_inputs.Clone();
	
		m_hiddens = new List<DActivationFunction[]>();

		foreach(DActivationFunction[] func in p_hiddens){
			m_hiddens.Add(func);
		}

		Debug.Log("HIDDENS: " + m_hiddens.Count);

		m_links = (Matrix[])p_weights.Clone();

		Debug.Log("WEIGHTS: " + m_links.Length);

		m_outputs = (SNeuralOutputDNA[])p_outputs.Clone();
	}

	//TEST DNA
	public NeuralDNA(){
		//INputs and outputs are simple
		float[] x1 = {1,0};
		float[] y = {0,-1};
		float[] z = {-1,0};

		SNeuralInputDNA[] inputs = { new SNeuralInputDNA(ENeuralInput.DIRECTION, x1), new SNeuralInputDNA(ENeuralInput.DIRECTION, y), new SNeuralInputDNA(ENeuralInput.DIRECTION, z) };
		m_inputs = inputs;
		SNeuralOutputDNA[] outputs = { new SNeuralOutputDNA(ENeuralOutput.OUTPUT, Activators.Sqrt()), new SNeuralOutputDNA(ENeuralOutput.OUTPUT, Activators.Sqrt()), new SNeuralOutputDNA(ENeuralOutput.OUTPUT, Activators.Sqrt()) };
		m_outputs = outputs;

		//Want to know the number of links
		int links = Random.Range(2,5);
		
		m_hiddens = new List<DActivationFunction[]>();
		//Links are indicated using their activator arrays
		for(int i = 0; i<links-1; i++){
			m_hiddens.Add(Activators.randomArray());
		}

		DActivationFunction[] activators = {Activators.Sqrt(),Activators.Sqrt(),Activators.Sqrt()};
		m_hiddens.Add(activators);

		Debug.Log("HIDDENS: " + m_hiddens.Count);



		m_hiddens.Add(Activators.randomArrayOfSize(outputs.Length));

		//Now need weight arrays which rep the weightings used in the links
		m_links = new Matrix[m_hiddens.Count];

		//The first weights are hidden layer x input matrix
		m_links[0] = new Matrix(m_hiddens[0].Length, m_inputs.Length);

		//The following weights are next layer x last layer matrix
		for(int i = 0; i<m_hiddens.Count-1; i++){
			m_links[i+1] = new Matrix(m_hiddens[i+1].Length, m_hiddens[i].Length);
		}

		//The last weights are the output x last layer matrix
		m_links[m_links.Length-1] = new Matrix(m_outputs.Length, m_hiddens[m_hiddens.Count-2].Length);

		Debug.Log("WEIGHTS: " + m_links.Length);

		Debug.Log(m_inputs.Length);
		
		string x = "[";

		foreach(DActivationFunction[] array in m_hiddens){
			x+=array.Length+",";
		}

		x+="]";
				
		Debug.Log(x);

		x = "[";

		foreach(Matrix mat in m_links){
			x+=mat.numColumns()+"x"+mat.numRows()+",";
		}

		x+="]";
				
		Debug.Log(x);

		Debug.Log(m_outputs.Length);
		
	}

	public NeuralInputLayer inputLayer(GameObject p_actor, ObjectLogger p_logger){
		INeuralInput[] input = new INeuralInput[m_inputs.Length];

		for(int i = 0; i<input.Length; i++){
			input[i] = m_inputs[i].getNeuralInput(p_actor, p_logger);
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

	public DActivationFunction[] getActivators(int p_link_index){
		return m_hiddens[p_link_index];
	}

	public DActivationFunction[] getOutputActivators(){
		DActivationFunction[] activators = new DActivationFunction[m_outputs.Length];

		for(int i = 0 ; i<m_outputs.Length; i++){
			activators[i] = m_outputs[i].m_activate;
		}

		return activators;		
	}

	public Matrix getWeights(int p_layer_index){
		return m_links[p_layer_index];
	}

	public Matrix getOutputWeights(){					
		string x = "Ouput weights\n\n";

		foreach(Matrix m in m_links){
			x += m + "\n\n";
		}

		Debug.Log(x);


		return m_links[m_links.Length-1];
	}

	public int hiddenLayerCount(){
		return m_hiddens.Count; 
	}

	public int hiddenLayerSize(int p_layer_index){
		return m_hiddens[p_layer_index].Length;
	}



}
