using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {

		NeuralInput[] inputs = {new NeuralInput(), new NeuralInput(),new NeuralInput()};
		NeuralInputLayer input = new NeuralInputLayer(inputs);

		NeuralOutput[] outputs = {new NeuralOutput(), new NeuralOutput()};
		DActivationFunction[] activators = {Activators.Sqrt(), Activators.Sqrt(),};
		NeuralOutputLayer output = new NeuralOutputLayer(outputs, activators);

		int[] hiddens = {2,2};
		NeuralNet net = new NeuralNet(input, output, hiddens);

		net.propagate();

	}

}
