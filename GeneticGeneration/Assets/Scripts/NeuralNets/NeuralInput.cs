using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NeuralInput : IPropagatable
{

  float[,] test;

  public NeuralInput(int size){
    test = new float[size, 1];

    for(int i = 0; i<size; i++){
      test[i, 0] = i;
    }
  }


  public int count()
  {
    return test.Length;
  }

  public float[,] propagate()
  {
    Debug.Log(MatrixCalc.String(test));
    return test;
  }
}
