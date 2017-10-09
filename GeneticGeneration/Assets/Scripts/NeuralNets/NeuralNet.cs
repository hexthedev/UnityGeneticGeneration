using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNet {

	NeuralInputLayer m_input;

	List<NeuralLink> m_links;

	List<NeuralHiddenLayer> m_layers;

	NeuralOutputLayer m_output;

	public NeuralNet(NeuralInputLayer p_input, NeuralOutputLayer p_output, int[] p_hidden_layers){
		m_input = p_input;
		m_output = p_output;

		m_layers = new List<NeuralHiddenLayer>();

		foreach(int layer_size in p_hidden_layers){
			m_layers.Add(new NeuralHiddenLayer(layer_size) );
		}

		m_links = new List<NeuralLink>();

		if(p_hidden_layers.Length == 0){
			m_links.Add(new NeuralLink(m_input, m_output));
		} else {
			m_links.Add(new NeuralLink(m_input, m_layers[0]));	

			for(int i = 0; i<m_layers.Count-1; i++){
				m_links.Add(new NeuralLink(m_layers[i], m_layers[i+1]));
			}

			m_links.Add(new NeuralLink(m_layers[m_layers.Count-1], m_output));
		}
	}

	public NeuralNet(NeuralDNA p_DNA){
		//Place the input layers
		m_input = p_DNA.inputLayer();
		m_output = p_DNA.outputLayer();

		//Hidden layers are based on Activators
		m_layers = new List<NeuralHiddenLayer>();

		for(int i = 0; i<p_DNA.hiddenLayerCount();i++){
			//Debug.Log(weights[i] + "  WEIGHTS");
			m_layers.Add(new NeuralHiddenLayer(p_DNA.hiddenLayerSize(i), p_DNA.getActivators(i)));
		}

		//Setup links with activators and weights between them
		m_links = new List<NeuralLink>();

		if(m_layers.Count == 1){
			m_links.Add(new NeuralLink(m_input, m_output, p_DNA.getOutputWeights()));
		} else {
			m_links.Add(new NeuralLink(m_input, m_layers[0], p_DNA.getWeights(0)));	

			for(int i = 0; i<m_layers.Count-1; i++){
				m_links.Add(new NeuralLink(m_layers[i], m_layers[i+1], p_DNA.getWeights(i+1)));
			}

			m_links.Add(new NeuralLink(m_layers[m_layers.Count-1], m_output, p_DNA.getOutputWeights()));
		}
	}
	
	public void propagate(){
		foreach(NeuralLink link in m_links){
			link.propagate();
		}
	}

	public NeuralDNA dnaify(){
		Matrix[] links = new Matrix[m_links.Count];

		for(int i = 0; i<m_links.Count; i++){
			links[i] = m_links[i].dnaify();
		}
		
		return new NeuralDNA(m_input.dnaify(), links, m_output.dnaify());
	}


}
