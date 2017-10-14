using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public delegate float DActivationFunction(float value);

public static class Activators{

  //Functions
  private static DActivationFunction m_bipolar = (float p_value) => { return p_value > 0 ? 1 : -1; };
  private static DActivationFunction m_piece_linear = (float p_value) => { 
    if(p_value > -1 && p_value < 1){
      return p_value;
    } else {
      return p_value > 1 ? 1 : -1; 
    }
  };
  private static DActivationFunction m_mod_sigmoid = (float p_value) => { return (2/(1+Mathf.Exp(-5*p_value)))-1; };
  private static DActivationFunction m_cosine = (float p_value) => { return Mathf.Cos(p_value*Mathf.PI); };

  private static DActivationFunction m_oppposite_cosine = (float p_value) => { return -1*Mathf.Cos(p_value*Mathf.PI); };



  //GETTER FUNCTIONS
  public static DActivationFunction Bipolar(){
    return m_bipolar;
  }

  public static DActivationFunction PiecewiseLinear(){
    return m_piece_linear;
  }

  public static DActivationFunction ModifiedSignmoid(){
    return m_mod_sigmoid;
  }

  public static DActivationFunction Cosine(){
    return m_cosine;
  }

  public static DActivationFunction OppositeCosine(){
    return m_oppposite_cosine;
  }

  public static DActivationFunction randomOutputFunction(ENeuralOutput p_output){

    if(p_output == ENeuralOutput.NOVeloX || p_output == ENeuralOutput.NOVeloY){
      return ArrayCalc.randomElement(NeuralNetConfig.all_functions);
    }

    Debug.LogError("MUST RETURN AN ACTIAVTOR");

    return null;
  }


  //RANDOM ARRAY GETTER FUNCTIONS
  public static DActivationFunction[] randomArray(MinMaxInt p_range){

    DActivationFunction[] to_return = new DActivationFunction[RandomCalc.Rand(p_range)];

    for(int i = 0 ; i<to_return.Length; i++){
      to_return[i] = ArrayCalc.randomElement(NeuralNetConfig.all_functions);
    }

    return to_return;

  }

  public static DActivationFunction[] randomArrayOfSize(int p_size){

    DActivationFunction[] to_return = new DActivationFunction[p_size];

    for(int i = 0 ; i<to_return.Length; i++){
      to_return[i] = ArrayCalc.randomElement(NeuralNetConfig.all_functions);
    }

    return to_return;
  }

}