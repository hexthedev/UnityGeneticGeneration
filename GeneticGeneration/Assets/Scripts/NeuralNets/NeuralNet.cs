using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNet {

	NeuralInputLayer m_input;

	List<NeuralHiddenLayer> m_layers;

	NeuralOutputLayer m_output;


	List<NeuralLink> m_links;

	//Create with hidden layers
	public NeuralNet(NeuralInputLayer p_input, NeuralOutputLayer p_output, int[] p_hidden_layers){
		//Input and output layers simple
		m_input = p_input;
		m_output = p_output;

		//Make a list of layers. Make them the right sizes
		m_layers = new List<NeuralHiddenLayer>();

		foreach(int layer_size in p_hidden_layers){
			m_layers.Add(new NeuralHiddenLayer(layer_size) );
		}

		//Make a list of neural links
		m_links = new List<NeuralLink>();

		//If no links, link input with output
		if(p_hidden_layers.Length == 0){
			m_links.Add(new NeuralLink(m_input, m_output));
		} else {
			//Otherwise, link inputs to first layer
			m_links.Add(new NeuralLink(m_input, m_layers[0]));	

			//Link other links to each other one after the other
			for(int i = 0; i<m_layers.Count-1; i++){
				m_links.Add(new NeuralLink(m_layers[i], m_layers[i+1]));
			}

			//Make one last link to output
			m_links.Add(new NeuralLink(m_layers[m_layers.Count-1], m_output));
		}
	}

	//Create Net with DNA
	public NeuralNet(NeuralDNA p_DNA, GameObject p_actor, ObjectLogger p_logger){
		//Place the input layers
		m_input = p_DNA.inputLayer(p_actor, p_logger);
		m_output = p_DNA.outputLayer();

		//Hidden layers are based on Activators
		m_layers = new List<NeuralHiddenLayer>();

		for(int i = 0; i<p_DNA.hiddenLayerCount();i++){
			//Debug.Log(weights[i] + "  WEIGHTS");
			m_layers.Add(new NeuralHiddenLayer(p_DNA.hiddenLayerSize(i), p_DNA.getActivators(i)));
		}

		//Setup links with weights
		m_links = new List<NeuralLink>();

		//No hidden, link input to output
		if(m_layers.Count == 0){
			m_links.Add(new NeuralLink(m_input, m_output, p_DNA.getOutputWeights()));
		} else {
			Debug.Log("ADDING START");

			//Othewise link input to first hidden
			m_links.Add(new NeuralLink(m_input, m_layers[0], p_DNA.getWeights(0)));	

			Debug.Log("ADDING HIDDEN");

			Debug.Log("LAYERS" + m_layers.Count);

			//Link layers with weights
			for(int i = 0; i<m_layers.Count-1; i++){
				m_links.Add(new NeuralLink(m_layers[i], m_layers[i+1], p_DNA.getWeights(i+1)));
			}

			Debug.Log("ADDING OUTPUT");

			//Link output
			m_links.Add(new NeuralLink(m_layers[m_layers.Count-1], m_output, p_DNA.getOutputWeights()));

			Debug.Log("DONE");
		}
	}

	//Call propagate on links one after another
	public void propagate(){
		foreach(NeuralLink link in m_links){
			link.propagate();
		}
	}


	public NeuralDNA dnaify(){
		//Hidden layer DNA
		List<DActivationFunction[]> hiddens = new List<DActivationFunction[]>();

		for(int i = 0; i<hiddens.Count-1; i++){
			hiddens.Add(m_layers[i].dnaify());
		}

		//Links DNA
		Matrix[] links = new Matrix[m_links.Count];

		for(int i = 0; i<m_links.Count; i++){
			links[i] = m_links[i].dnaify();
		}

		return new NeuralDNA(m_input.dnaify(), hiddens, links, m_output.dnaify());
	}


}
