using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		Matrix<float> x = Matrix<float>.Build.Dense(3, 3, (i,j) => { return 2f; } );
		Matrix<float> y = Matrix<float>.Build.Dense(1,4, (i,j) => { return 2f; } );

		Debug.Log(x.ToString());
		Debug.Log(y.ToString());
		//Debug.Log((x*y).ToString());

		Debug.Log(y.RowCount);

		x[1,2] = 4;
		Debug.Log(x[1,2]);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
