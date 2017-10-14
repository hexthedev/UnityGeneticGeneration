using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SNeuralOutputDNA {

	public ENeuralOutput m_output_type;
	public DActivationFunction m_activate;

	public SNeuralOutputDNA(ENeuralOutput p_output, DActivationFunction p_activate){
		m_output_type = p_output;
		m_activate = p_activate;
	}

	public INeuralOutput getNeuralOutput(EnemyControllerNeural p_actor_controller){
		switch(m_output_type){
			case ENeuralOutput.NOVeloX: 
				return new NOVeloX(p_actor_controller);
		}
		return null;
	}

}