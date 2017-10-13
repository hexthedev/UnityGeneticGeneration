using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NeuralInputLayer : IInputable
{
  //Aggregation of inputs
  NeuralInput[] m_inputs;

  //Simply holds the inputs for the input layer
  public NeuralInputLayer(NeuralInput[] p_inputs){
    m_inputs = (NeuralInput[])p_inputs.Clone();
  }

  //Count is number of inputs
  public int count()
  {
    return m_inputs.Length;
  }

  //Gets input values for current frame and passes as a matrix
  public Matrix getInputs()
  {
    float[,] m_prop = new float[count(),1]; 
    
    for(int i = 0; i<m_inputs.Length;i++){
      m_prop[i, 0] = m_inputs[i].input();
    }

    Debug.Log(" Input Weights \n " + MatrixCalc.String(m_prop));
    return new Matrix(m_prop);
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
