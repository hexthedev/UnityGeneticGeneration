using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathNet.Numerics.LinearAlgebra;

public interface IRecievable {

	void recievePropagation( Matrix<float> p_propagation);
	int count();

}
