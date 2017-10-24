using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NeuralNetConfig {

	//Neural Net
	public static bool debug_net = false;
	

	//Neural DNA
	public static bool debug_net_dna = false;

	public static MinMaxInt input_mm= new MinMaxInt(0,0);
	public static MinMaxInt output_mm= new MinMaxInt(2,4);
	public static MinMaxInt hidden_layer_mm= new MinMaxInt(1,2);
	public static MinMaxInt hidden_size_mm= new MinMaxInt(1,3);

	public static MinMaxFloat mutation_amount = new MinMaxFloat(0.2f, 0.6f);

	public static float mutation_chance = 20f;

	//Activation Functions
	public static DActivationFunction[] all_functions = {Activators.Bipolar(), Activators.ModifiedSignmoid(), Activators.Cosine(), Activators.OppositeCosine()};
  	public static DActivationFunction[] continous_functions = {Activators.ModifiedSignmoid(), Activators.Cosine(), Activators.OppositeCosine()};
  	public static DActivationFunction[] binary_functions = {Activators.Bipolar()};

	//Links
	public static MinMaxFloat weight_mm= new MinMaxFloat(-1f,1f);

	//InputLayerDNA
	public static ENeuralInput[] active_inputs = {ENeuralInput.DIRECTION, ENeuralInput.PROXPLAYER};
	public static MinMaxFloat direction_mm= new MinMaxFloat(-1f,1f);
	public static MinMaxFloat prox_mm= new MinMaxFloat(5f,8f);


	//OutputLayerDNA
	public static ENeuralOutput[] activate_outputs = {ENeuralOutput.NOVeloX, ENeuralOutput.NOVeloY};

	

}

