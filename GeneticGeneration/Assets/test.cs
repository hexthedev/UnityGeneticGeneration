using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {

		NeuralInput input = new NeuralInput(5);

		NeuralHiddenLayer hidden1 = new NeuralHiddenLayer();

		NeuralHiddenLayer hidden2 = new NeuralHiddenLayer();

		NeuralLink link1 = new NeuralLink(input, hidden1);
		NeuralLink link2 = new NeuralLink(hidden1, hidden2);
		
		link1.propagate();
		link2.propagate();

	}

}
