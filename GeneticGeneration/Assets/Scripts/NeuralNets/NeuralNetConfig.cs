using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Calc;

public static class NeuralNetConfig {

	//Neural Net
	public static bool debug_net = false;
	

	//Neural DNA
	public static bool debug_net_dna = false;

	public static Range<int> input_mm= new Range<int>(0,0);
	public static Range<int> output_mm= new Range<int>(2,4);
	public static Range<int> hidden_layer_mm= new Range<int>(1,2);
	public static Range<int> hidden_size_mm= new Range<int>(1,3);

	public static Range<float> mutation_amount = new Range<float>(0.2f, 0.6f);

	public static float mutation_chance = 20f;

	//Activation Functions
	public static DActivationFunction[] all_functions = {Activators.Bipolar(), Activators.ModifiedSignmoid(), Activators.Cosine(), Activators.OppositeCosine()};
  	public static DActivationFunction[] continous_functions = {Activators.ModifiedSignmoid(), Activators.Cosine(), Activators.OppositeCosine()};
  	public static DActivationFunction[] binary_functions = {Activators.Bipolar()};

	//Links
	public static Range<float> weight_mm= new Range<float>(-1f,1f);

	//InputLayerDNA
	public static ENeuralInput[] active_inputs = {ENeuralInput.DIRECTION, ENeuralInput.PROXIMITY};
	public static Range<float> direction_mm= new Range<float>(-1f,1f);
	public static Range<float> prox_mm= new Range<float>(5f,8f);


	//OutputLayerDNA
	public static ENeuralOutput[] activate_outputs = {ENeuralOutput.NOVeloX, ENeuralOutput.NOVeloY};

	

}

