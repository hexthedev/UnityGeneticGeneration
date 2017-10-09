using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralInput {

	public float input(){
		return Random.Range(-1f, 1f);
	}

	public SNeuralInputDNA dnaify(){
		return new SNeuralInputDNA();
	}

}
