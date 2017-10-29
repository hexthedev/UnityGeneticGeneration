﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

using MathNet.Numerics.LinearAlgebra;

public class NeuralOutputLayer : IRecievable {
 
 //Has outputs and activation functions
  INeuralOutput[] m_outputs;
  DActivationFunction[] m_activators;

  //Create with activators, must have same number of activators to outputs
  public NeuralOutputLayer(INeuralOutput[] p_outputs, DActivationFunction[] p_activators){
    m_outputs = (INeuralOutput[])p_outputs.Clone();

    if(p_activators.Length != p_outputs.Length){
      Debug.LogError("OUTPUT WRONG NUMBER ACTIVATORS");
    }

    m_activators = (DActivationFunction[])p_activators.Clone();
  }

  //Number of outputs
  public int count()
  {
    return m_outputs.Length;
  }

  //Recieve propagation and activate
  public void recievePropagation(Matrix<float> p_propagation)
  {
    Matrix<float> prop = p_propagation.Clone();

    for(int i = 0; i<m_activators.Length; i++){
      prop[0, i] = m_activators[i]( prop[0, i] );
    }

    //Complete Output
    for(int i = 0; i<m_outputs.Length; i++){
      m_outputs[i].output(prop[0,i]);
    }
  }

  public SNeuralOutputDNA[] dnaify(){
    SNeuralOutputDNA[] outputs = new SNeuralOutputDNA[m_outputs.Length];

    for(int i = 0; i<m_outputs.Length;i++){
      outputs[i].m_output_type = m_outputs[i].dnaify();
      outputs[i].m_activate = m_activators[i];
    }

    return outputs;
  }

  public DActivationFunction[] getActivators(){
    return m_activators;
  }

}
