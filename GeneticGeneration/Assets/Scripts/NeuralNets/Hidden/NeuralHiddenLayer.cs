using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NeuralHiddenLayer : IRecievable, IInputable
{
  //Holds activator array. Must be same Length as result columns
  DActivationFunction[] m_activators;

  Matrix m_results;

  //Construct with predetermined activators
  public NeuralHiddenLayer(int p_size){
    m_results = new Matrix(p_size, 1);

    m_activators = new DActivationFunction[p_size];

    m_activators = Activators.randomArrayOfSize(p_size);
  }

  //Construct by giving activators. Must be right length
  public NeuralHiddenLayer(int p_size, DActivationFunction[] p_activators){
    m_results = new Matrix(p_size, 1);

    
    if(p_activators.Length != p_size){
      Debug.LogError("Activators length wrong");
    }
    
    m_activators = (DActivationFunction[])p_activators.Clone();
  }

  //Columns = length
  public int count()
  {
    return m_results.numColumns();
  }

  //results are input
  public Matrix getInputs()
  {
//    Debug.Log("Hidden Input \n" + m_results);
    
    return m_results;
  }

  //On recieving p_pagation make results a activate
  public void recievePropagation(Matrix p_propagation)
  {
//    Debug.Log("Pre Activation \n" + p_propagation);
    m_results = p_propagation.clone();
    m_results.activate(m_activators);
//    Debug.Log("Activated \n" + m_results);

  }

  //HiddenLayers are arrays of activators
  public DActivationFunction[] dnaify(){
    return (DActivationFunction[]) m_activators.Clone();
  }
}
