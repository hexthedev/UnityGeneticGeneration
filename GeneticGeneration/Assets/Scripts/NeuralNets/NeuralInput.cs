using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralInput {

	//Return input as a -1 to 1 float
	public float input(){
		return Random.Range(-1f, 1f);
	}

	public SNeuralInputDNA dnaify(){
		return new SNeuralInputDNA();
	}

}
