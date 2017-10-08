using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NeuralHiddenLayer : IRecievable, IPropagatable
{
  
  DActivationFunction[] m_activators = new DActivationFunction[3];

  float[,] m_results = new float[3, 1];

  public NeuralHiddenLayer(){
    for(int i = 0; i<count(); i++){
      m_activators[i] = (float p_value) => { return Mathf.Round(p_value); };
    }
  }

  public int count()
  {
    return 3;
  }

  public float[,] propagate()
  {
    Debug.Log(MatrixCalc.String(m_results));
    return m_results;
  }

  public void recievePropagation(float[,] p_propagation)
  {
    Debug.Log(MatrixCalc.String(p_propagation));

    for(int i = 0; i<3; i++){
      m_results[i,0] = m_activators[i](p_propagation[i, 0]);
    }

    Debug.Log(MatrixCalc.String(m_results) );
  }
}
