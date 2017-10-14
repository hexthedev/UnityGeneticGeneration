using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NeuralNetConfig {

	//Neural Net
	public static bool debug_net = false;
	

	//Neural DNA
	public static bool debug_net_dna = false;

	public static MinMaxInt input_mm= new MinMaxInt(1,4);
	public static MinMaxInt output_mm= new MinMaxInt(1,4);
	public static MinMaxInt hidden_layer_mm= new MinMaxInt(1,4);
	public static MinMaxInt hidden_size_mm= new MinMaxInt(1,4);


	//Activation Functions
	public static DActivationFunction[] all_functions = {Activators.Bipolar(), Activators.ModifiedSignmoid(), Activators.Cosine(), Activators.OppositeCosine()};
  public static DActivationFunction[] continous_functions = {Activators.ModifiedSignmoid(), Activators.Cosine(), Activators.OppositeCosine()};
  public static DActivationFunction[] binary_functions = {Activators.Bipolar()};

	//Links
	public static MinMaxFloat weight_mm= new MinMaxFloat(-1f,1f);

	//InputLayerDNA
	public static ENeuralInput[] m_active_inputs = {ENeuralInput.DIRECTION, ENeuralInput.PROXPLAYER};
	public static MinMaxFloat direction_mm= new MinMaxFloat(-1f,1f);
	public static MinMaxFloat prox_mm= new MinMaxFloat(1f,2f);


	//OutputLayerDNA
	public static ENeuralOutput[] m_activate_outputs = {ENeuralOutput.NOVeloX, ENeuralOutput.NOVeloY};

	

}

