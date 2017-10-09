using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {

		for(int i = 0; i<1000; i++){
			NeuralNet x = new NeuralNet(new NeuralDNA());
			x.propagate();
		}


	}

}
