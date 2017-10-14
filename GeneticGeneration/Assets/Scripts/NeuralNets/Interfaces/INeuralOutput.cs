using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INeuralOutput {

	//Gives to enemy output
	void output(float p_value);

	ENeuralOutput dnaify();

}
