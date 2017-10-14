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

	//RANDOMIZATION FUNCTIONS
	public INeuralInput getNeuralInput(GameObject p_actor, ObjectLogger p_logger){
		switch(m_input_type){
			case ENeuralInput.DIRECTION: 
				return new NIDirection(p_actor, p_logger, (float[])m_params.Clone());
		}
		return null;
	}


	//RANDOMIZAITON FUNCTIONS
	private static ENeuralInput[] m_active_inputs = {ENeuralInput.DIRECTION}; 

	public static SNeuralInputDNA randomInputDNA(ENeuralInput p_input_type){

		switch(p_input_type){
			case ENeuralInput.DIRECTION: 
				float[] dir_params = {Random.Range(-1f, 1f), Random.Range(-1f, 1f)};
				return new SNeuralInputDNA(ENeuralInput.DIRECTION, dir_params); 
		}

		Debug.LogError("SHOULD ALWAYS RETURN A SNEURALINPUTDNA");
		return new SNeuralInputDNA();
	}	

	public static SNeuralInputDNA[] randomInputArrayNoRepeat(int size){ 

		List<ENeuralInput> inputs = new List<ENeuralInput>(m_active_inputs);	

		List<SNeuralInputDNA> dna = new List<SNeuralInputDNA>();

		for(int i = 0; i<size; i++){
			if(inputs.Count == 0){		break;	}
			
			ENeuralInput input = inputs[Random.Range(0, inputs.Count)];
			dna.Add(randomInputDNA(input));
			inputs.Remove(input);
		}

		return dna.ToArray(); 
	}

	public static SNeuralInputDNA[] randomInputArrayRepeat(int size){ 

		List<SNeuralInputDNA> dna = new List<SNeuralInputDNA>();

		for(int i = 0; i<size; i++){
			dna.Add(randomInputDNA(ArrayCalc.randomElement(m_active_inputs)));
		}

		return dna.ToArray(); 
	}




}
