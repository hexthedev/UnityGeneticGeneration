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
			case ENeuralOutput.NOVeloY:
				return new NOVeloY(p_actor_controller);
		}
		return null;
	}

	//RANDOMIZATION FUNCTIONS
	public static SNeuralOutputDNA randomOutputDNA(){
		ENeuralOutput output = ArrayCalc.randomElement(NeuralNetConfig.activate_outputs);
		return new SNeuralOutputDNA(output, randomOutputFunction(output));
	}

	public static SNeuralOutputDNA outputDNAwithRandomActivator(ENeuralOutput p_output){
		return new SNeuralOutputDNA(p_output, randomOutputFunction(p_output));
	}	

	public static SNeuralOutputDNA[] randomOutputArrayNoRepeat(int size){

		List<ENeuralOutput> outputs = new List<ENeuralOutput>(NeuralNetConfig.activate_outputs);	

		List<SNeuralOutputDNA> dna = new List<SNeuralOutputDNA>();

		for(int i = 0; i<size; i++){
			if(outputs.Count == 0){		break;	}
			
			ENeuralOutput output = outputs[RandomCalc.Rand(new MinMaxInt(0, outputs.Count-1))];
			dna.Add(outputDNAwithRandomActivator(output));
			outputs.Remove(output);
		}

		return dna.ToArray(); 
	}

	//HELPER FUNCTIONS
	public static DActivationFunction randomOutputFunction(ENeuralOutput p_output){

    if(p_output == ENeuralOutput.NOVeloX || p_output == ENeuralOutput.NOVeloY){
      return ArrayCalc.randomElement(NeuralNetConfig.all_functions);
    }

    Debug.LogError("MUST RETURN AN ACTIAVTOR");

    return null;
  }
}