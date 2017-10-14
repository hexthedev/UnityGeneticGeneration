﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NeuralDNA {

	SNeuralInputDNA[] m_inputs;	//Represents the number of inputs in input layer
	List<DActivationFunction[]> m_hiddens;		//Represents the number of hidden layers. Activation functions act on hidden layer
	Matrix[] m_links;			//Represents the weight between two layers in order
	SNeuralOutputDNA[] m_outputs;		//Represents the outputs in the output layer

	//Constructs DNA from parts when creatures die
	public NeuralDNA( SNeuralInputDNA[] p_inputs, List<DActivationFunction[]> p_hiddens, Matrix[] p_weights, SNeuralOutputDNA[] p_outputs ){
		m_inputs = (SNeuralInputDNA[])p_inputs.Clone();
	
		m_hiddens = new List<DActivationFunction[]>();

		foreach(DActivationFunction[] func in p_hiddens){
			m_hiddens.Add(func);
		}
		debug("HIDDENS: " + m_hiddens.Count);
		
		m_links = (Matrix[])p_weights.Clone();
		debug("WEIGHTS: " + m_links.Length);

		m_outputs = (SNeuralOutputDNA[])p_outputs.Clone();
	}

	//Constructs random neural DNA
	public NeuralDNA(){
		m_inputs = SNeuralInputDNA.randomInputArrayRepeat(RandomCalc.Rand(NeuralNetConfig.input_mm));
		m_outputs = SNeuralOutputDNA.randomOutputArrayNoRepeat(RandomCalc.Rand(NeuralNetConfig.output_mm));

		//Now that we have output we can make some random hidden layers based on activation functions
		int hiddens = RandomCalc.Rand(NeuralNetConfig.hidden_layer_mm);

		m_hiddens = new List<DActivationFunction[]>();
		for(int i = 0; i<hiddens; i++){
			m_hiddens.Add(Activators.randomArrayOfSize(RandomCalc.Rand(NeuralNetConfig.hidden_size_mm)));
		}

		//Finally we construct the required links
		m_links = new Matrix[hiddens+1];

		m_links[0] = new Matrix(m_hiddens[0].Length, m_inputs.Length, NeuralNetConfig.weight_mm, true);

		for(int i = 1; i<m_links.Length-1; i++){
			m_links[i] = new Matrix(m_hiddens[i].Length, m_hiddens[i-1].Length, NeuralNetConfig.weight_mm, true);
		}

		m_links[m_links.Length-1] = new Matrix(m_outputs.Length, m_hiddens[m_hiddens.Count-1].Length, NeuralNetConfig.weight_mm, true);
		
	}

	//BIRTHING FUNCTIONS

	//Converts the DNA inputs into a concrete input layer
	public NeuralInputLayer inputLayer(GameObject p_actor, ObjectLogger p_logger){
		INeuralInput[] input = new INeuralInput[m_inputs.Length];

		for(int i = 0; i<input.Length; i++){
			input[i] = m_inputs[i].getNeuralInput(p_actor, p_logger);
		}

		return new NeuralInputLayer(input);
	}

	//Converts the DNA inputs into a concrete output layer
	public NeuralOutputLayer outputLayer(EnemyControllerNeural p_controller){
		INeuralOutput[] output = new INeuralOutput[m_outputs.Length];
		
		for(int i = 0 ; i<output.Length;i++){
			output[i] = m_outputs[i].getNeuralOutput(p_controller);
		}

		return new NeuralOutputLayer(output, getOutputActivators());
	}

	//Returns activators of a hidden layer at a certain depth
	public DActivationFunction[] getActivators(int p_link_index){
		return m_hiddens[p_link_index];
	}

	//Returns activators of a output layer
	public DActivationFunction[] getOutputActivators(){
		DActivationFunction[] activators = new DActivationFunction[m_outputs.Length];

		for(int i = 0 ; i<m_outputs.Length; i++){
			activators[i] = m_outputs[i].m_activate;
		}

		return activators;		
	}

	//Return weights of link at certain index
	public Matrix getWeights(int p_layer_index){
		return m_links[p_layer_index];
	}

	//Returns weights for the final link to output
	public Matrix getOutputWeights(){					
		return m_links[m_links.Length-1];
	}

	//Returns the number of hidden layers
	public int hiddenLayerCount(){
		return m_hiddens.Count; 
	}

	//Returns the size of a hidden layer at an index
	public int hiddenLayerSize(int p_layer_index){
		return m_hiddens[p_layer_index].Length;
	}


	//DEBUG CODE
	public void debug(string p){
		if(NeuralNetConfig.debug_net_dna){
			Debug.Log(p);
		}
	}



}
