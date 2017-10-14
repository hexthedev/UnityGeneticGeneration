using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public struct SNeuralOutputDNA {

	//STRUCT
	public ENeuralOutput m_output_type;
	public DActivationFunction m_activate;

	public SNeuralOutputDNA(ENeuralOutput p_output, DActivationFunction p_activator){
		m_output_type = p_output;
		m_activate = p_activator;
	}

	//BIRTHING FUNCTION
	public INeuralOutput getNeuralOutput(EnemyControllerNeural p_actor_controller){
		switch(m_output_type){
			case ENeuralOutput.NOVeloX: 
				return new NOVeloX(p_actor_controller);
		}
		return null;
	}

	//RANDOMIZATION FUNCTIONS
	public static ENeuralOutput[] m_activate_outputs = {ENeuralOutput.NOVeloX};

	public static SNeuralOutputDNA randomOutputDNA(){
		ENeuralOutput output = ArrayCalc.randomElement(m_activate_outputs);
		return new SNeuralOutputDNA(output, Activators.randomOutputFunction(output));
	}

	public static SNeuralOutputDNA outputDNAwithRandomActivator(ENeuralOutput p_output){
		return new SNeuralOutputDNA(p_output, Activators.randomOutputFunction(p_output));
	}	

	public static SNeuralOutputDNA[] randomOutputArrayNoRepeat(int size){

		List<ENeuralOutput> outputs = new List<ENeuralOutput>(m_activate_outputs);	

		List<SNeuralOutputDNA> dna = new List<SNeuralOutputDNA>();

		for(int i = 0; i<size; i++){
			if(outputs.Count == 0){		break;	}
			
			ENeuralOutput output = outputs[Random.Range(0, outputs.Count)];
			dna.Add(outputDNAwithRandomActivator(output));
			outputs.Remove(output);
		}

		return dna.ToArray(); 
	}
}