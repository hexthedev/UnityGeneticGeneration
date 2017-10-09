using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NeuralOutputLayer : IRecievable {
 
  NeuralOutput[] m_outputs;
  DActivationFunction[] m_activators;

  public NeuralOutputLayer(NeuralOutput[] p_outputs, DActivationFunction[] p_activators){
    m_outputs = (NeuralOutput[])p_outputs.Clone();

    if(p_activators.Length != p_outputs.Length){
      Debug.LogError("EACH OUTPUT NEEDS ACTIVATOR");
    }

    m_activators = (DActivationFunction[])p_activators.Clone();
  }

 
  public int count()
  {
    return m_outputs.Length;
  }

  public void recievePropagation(Matrix p_propagation)
  {
    Debug.Log(p_propagation);

    p_propagation.activate(m_activators);

    Debug.Log(p_propagation);

    for(int i = 0; i<m_outputs.Length; i++){
      m_outputs[i].output(p_propagation.get(i, 0));
    }
  }

  public SNeuralOutputDNA[] dnaify(){
    SNeuralOutputDNA[] outputs = new SNeuralOutputDNA[m_outputs.Length];

    for(int i = 0; i<m_outputs.Length;i++){
      outputs[i].m_output = m_outputs[i].dnaify();
      outputs[i].m_activate = m_activators[i];
    }

    return outputs;
  }

}
