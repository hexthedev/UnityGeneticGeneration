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

	public void propagate(){
		foreach(NeuralLink link in m_links){
			link.propagate();
		}
	}




}
