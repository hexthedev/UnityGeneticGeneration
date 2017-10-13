using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {

		for(int i = 0; i<1; i++){
			NeuralNet x = new NeuralNet(new NeuralDNA());
			x.propagate();

			NeuralDNA y = x.dnaify();

			NeuralNet z = new NeuralNet(y);
			//z.propagate();

		}


	}

}
