using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

using MathNet.Numerics.LinearAlgebra;

public class NeuralInputLayer : IInputable
{
  //Aggregation of inputs
  INeuralInput[] m_inputs;

  //Simply holds the inputs for the input layer
  public NeuralInputLayer(INeuralInput[] p_inputs){
    m_inputs = (INeuralInput[])p_inputs.Clone();
  }

  //Count is number of inputs
  public int count()
  {
    return m_inputs.Length;
  }

  //Gets input values for current frame and passes as a matrix
  public Matrix<float> getInputs()
  {
    Matrix<float> inputs = Matrix<float>.Build.Dense(1, count());
        
    for(int i = 0; i<m_inputs.Length;i++){
     inputs[0, i] = m_inputs[i].input();
    }

    return inputs;
  }

  //Returns an array of dnaified NeuralInputs. The represents input layer
  public SNeuralInputDNA[] dnaify(){
    SNeuralInputDNA[] to_return = new SNeuralInputDNA[m_inputs.Length];

    for(int i = 0; i<m_inputs.Length; i++){
      to_return[i] = m_inputs[i].dnaify();
    }

    return to_return;

  }
}
