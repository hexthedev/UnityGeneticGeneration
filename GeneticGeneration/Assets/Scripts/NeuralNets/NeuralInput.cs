using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INeuralInput {

	//Return input as a -1 to 1 float
	float input();

	SNeuralInputDNA dnaify();

}
