using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NeuralHiddenLayer : IRecievable, IPropagatable
{
  
  DActivationFunction[] m_activators;

  Matrix m_results;

  public NeuralHiddenLayer(int p_size){
    m_results = new Matrix(p_size, 1);

    m_activators = new DActivationFunction[p_size];

    for(int i = 0; i<m_activators.Length; i++){
      m_activators[i] = Activators.Sqrt();
    }
  }

  public NeuralHiddenLayer(int p_size, DActivationFunction[] p_activators){
    m_results = new Matrix(p_size, 1);

    
    if(p_activators.Length != p_size){
      Debug.LogError("Activators length wrong");
    }
    
    m_activators = (DActivationFunction[])p_activators.Clone();
  }

  public int count()
  {
    return m_results.numColumns();
  }

  public Matrix propagate()
  {
    Debug.Log(m_results);
    
    return m_results;
  }

  public void recievePropagation(Matrix p_propagation)
  {
    Debug.Log(p_propagation);

    m_results.activate(m_activators);

    Debug.Log(m_results);
  
  }

  public DActivationFunction[] dnaify(){
    return (DActivationFunction[]) m_activators.Clone();
  }
}
