using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNet {

	NeuralInputLayer m_input;
	List<NeuralHiddenLayer> m_layers;
	NeuralOutputLayer m_output;

	List<NeuralLink> m_links;

	//Neural nets are created completely through NeuralDNA. They are created on a creature as the "brain"
	public NeuralNet(NeuralDNA p_DNA, CreatureController p_controller){
		//Place the input layers
		m_input = p_DNA.inputLayer(p_controller);
		m_output = p_DNA.outputLayer(p_controller);

		//Hidden layers are based on Activators
		m_layers = new List<NeuralHiddenLayer>();

		for(int i = 0; i<p_DNA.hiddenLayerCount();i++){
			m_layers.Add(new NeuralHiddenLayer(p_DNA.hiddenLayerSize(i), p_DNA.getActivators(i)));
		}

		//Setup links with weights
		m_links = new List<NeuralLink>();

		
		if(m_layers.Count == 0){	//No hidden, link input to output
			m_links.Add(new NeuralLink(m_input, m_output, p_DNA.getOutputWeights()));
		} else { //Othewise link input to first hidden
			debug("ADDING START");
			
			m_links.Add(new NeuralLink(m_input, m_layers[0], p_DNA.getWeights(0)));	

			debug("ADDING HIDDEN");
			debug("LAYERS" + m_layers.Count);

			//Link layers with weights
			for(int i = 0; i<m_layers.Count-1; i++){
				m_links.Add(new NeuralLink(m_layers[i], m_layers[i+1], p_DNA.getWeights(i+1)));
			}

			debug("ADDING OUTPUT");

			//Link output
			m_links.Add(new NeuralLink(m_layers[m_layers.Count-1], m_output, p_DNA.getOutputWeights()));

			debug("DONE");
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

	private void debug(string p){
		if(NeuralNetConfig.debug_net){
			Debug.Log(p);
		}
	}

	

}
