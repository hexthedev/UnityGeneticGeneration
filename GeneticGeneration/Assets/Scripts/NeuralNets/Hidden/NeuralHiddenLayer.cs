using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Calc;
using MathNet.Numerics.LinearAlgebra;

public class NeuralHiddenLayer : IRecievable, IInputable
{
  //Holds activator array. Must be same Length as result columns
  DActivationFunction[] m_activators;

  Matrix<float> m_results;

  //Construct with predetermined activators
  public NeuralHiddenLayer(int p_size){
    m_results = Matrix<float>.Build.Dense(1, p_size);

    m_activators = new DActivationFunction[p_size];

    m_activators = Activators.randomArrayOfSize(p_size);
  }

  //Construct by giving activators. Must be right length
  public NeuralHiddenLayer(int p_size, DActivationFunction[] p_activators){
    m_results = Matrix<float>.Build.Dense(1, p_size);
    
    if(p_activators.Length != p_size){
      Debug.LogError("Activators length wrong");
    }
    
    m_activators = (DActivationFunction[])p_activators.Clone();
  }

  //Columns = length
  public int count()
  {
    return m_results.ColumnCount;
  }

  //results are input
  public Matrix<float> getInputs()
  {  
    return m_results;
  }

  //On recieving p_pagation make results a activate
  public void recievePropagation(Matrix<float> p_propagation)
  {
    m_results = p_propagation.Clone();

    for(int i = 0; i<m_activators.Length; i++){
      m_results[0,i] = m_activators[i](m_results[0,i]);
    }
  }

  //HiddenLayers are arrays of activators
  public DActivationFunction[] dnaify(){
    return (DActivationFunction[]) m_activators.Clone();
  }
}
