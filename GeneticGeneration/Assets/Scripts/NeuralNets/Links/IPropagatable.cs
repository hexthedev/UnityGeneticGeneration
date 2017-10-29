using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathNet.Numerics.LinearAlgebra;

public interface IInputable {

	Matrix<float> getInputs();
	int count();

}
