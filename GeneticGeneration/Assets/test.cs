using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {

		NeuralNet x = new NeuralNet(new NeuralDNA());

		x.propagate();

	}

}
