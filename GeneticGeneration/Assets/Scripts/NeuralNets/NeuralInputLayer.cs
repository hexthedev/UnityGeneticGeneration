using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NeuralInputLayer : IPropagatable
{

  NeuralInput[] m_inputs;

  public NeuralInputLayer(NeuralInput[] p_inputs){
    m_inputs = (NeuralInput[])p_inputs.Clone();
  }

  public int count()
  {
    return m_inputs.Length;
  }

  public Matrix propagate()
  {
    float[,] m_prop = new float[count(),1]; 
    
    for(int i = 0; i<m_inputs.Length;i++){
      m_prop[i, 0] = m_inputs[i].input();
    }

    Debug.Log(MatrixCalc.String(m_prop));
    return new Matrix(m_prop);
  }

  public SNeuralInputDNA[] dnaify(){
    SNeuralInputDNA[] to_return = new SNeuralInputDNA[m_inputs.Length];

    for(int i = 0; i<m_inputs.Length; i++){
      to_return[i] = m_inputs[i].dnaify();
    }

    return to_return;

  }
}
