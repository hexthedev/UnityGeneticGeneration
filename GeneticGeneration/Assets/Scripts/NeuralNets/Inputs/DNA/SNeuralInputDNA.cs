using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public struct SNeuralInputDNA {

	private ENeuralInput m_input_type;

	private float[] m_params;

	public SNeuralInputDNA(ENeuralInput p_input, float[] p_params){
		m_input_type = p_input;
		m_params = p_params;
	}

	//BIRTHING FUNCTIONS FUNCTIONS
	public INeuralInput getNeuralInput(GameObject p_actor, ObjectLogger p_logger){
		switch(m_input_type){
			case ENeuralInput.DIRECTION: 
				return new NIDirection(p_actor, p_logger, (float[])m_params.Clone());
			case ENeuralInput.PROXPLAYER:
				return new NIProxPlayer(p_actor, p_logger, m_params);
		}
		Debug.LogError("SHOULD ALWAYS RETURN A INeuralInput");
		return null;
	}


	//RANDOMIZAITON FUNCTIONS
	
	public static SNeuralInputDNA randomInputDNA(ENeuralInput p_input_type){

		switch(p_input_type){
			case ENeuralInput.DIRECTION: 
				float[] dir_params = {RandomCalc.Rand(NeuralNetConfig.direction_mm), RandomCalc.Rand(NeuralNetConfig.direction_mm)};
				return new SNeuralInputDNA(ENeuralInput.DIRECTION, dir_params); 
			case ENeuralInput.PROXPLAYER: 
				float[] prox_params = {RandomCalc.Rand(NeuralNetConfig.prox_mm)};
				return new SNeuralInputDNA(ENeuralInput.PROXPLAYER, prox_params); 
		}

		Debug.LogError("SHOULD ALWAYS RETURN A SNEURALINPUTDNA");
		return new SNeuralInputDNA();
	}	

	public static SNeuralInputDNA[] randomInputArrayNoRepeat(int size){ 

		List<ENeuralInput> inputs = new List<ENeuralInput>(NeuralNetConfig.active_inputs);	

		List<SNeuralInputDNA> dna = new List<SNeuralInputDNA>();

		for(int i = 0; i<size; i++){
			if(inputs.Count == 0){		break;	}
			
			ENeuralInput input = inputs[RandomCalc.Rand(new MinMaxInt(0, inputs.Count-1))];
			dna.Add(randomInputDNA(input));
			inputs.Remove(input);
		}

		return dna.ToArray(); 
	}

	public static SNeuralInputDNA[] randomInputArrayRepeat(int size){ 

		List<SNeuralInputDNA> dna = new List<SNeuralInputDNA>();

		for(int i = 0; i<size; i++){
			dna.Add(randomInputDNA(ArrayCalc.randomElement(NeuralNetConfig.active_inputs)));
		}

		return dna.ToArray(); 
	}




}
