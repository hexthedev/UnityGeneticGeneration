using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {

		float[,] mat1 = { {1,1,1,1} };
		float[,] mat2 = { {1, 5}, {2,6}, {3,7}, {4,8} };
		

		float[ , ] result = MatrixCalc.matrixMultiply(mat1, mat2);

		Debug.Log(MatrixCalc.matrixString(mat1));
		Debug.Log(MatrixCalc.matrixString(mat2));
		Debug.Log(MatrixCalc.matrixString(result));

	}

}
