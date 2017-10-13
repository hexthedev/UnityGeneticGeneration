using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SNeuralOutputDNA {

	public ENeuralOutput m_output;
	public DActivationFunction m_activate;

	public SNeuralOutputDNA(ENeuralOutput p_output, DActivationFunction p_activate){
		m_output = p_output;
		m_activate = p_activate;
	}

}
