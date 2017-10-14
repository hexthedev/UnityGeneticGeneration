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
  private static DActivationFunction m_mod_signmoid = (float p_value) => { return (2/(1+Mathf.Exp(-5*p_value)))-1; };
  private static DActivationFunction m_cosine = (float p_value) => { return Mathf.Cos(p_value); };

  //Array Setups
  private static DActivationFunction[] m_all_functions = {m_bipolar, m_piece_linear, m_mod_signmoid, m_cosine};
  private static DActivationFunction[] m_continous_functions = {m_piece_linear, m_mod_signmoid, m_cosine};
  private static DActivationFunction[] m_binary_functions = {m_bipolar};



  //GETTER FUNCTIONS
  public static DActivationFunction Bipolar(){
    return m_bipolar;
  }

  public static DActivationFunction PiecewiseLinear(){
    return m_piece_linear;
  }

  public static DActivationFunction ModifiedSignmoid(){
    return m_mod_signmoid;
  }

  public static DActivationFunction Cosine(){
    return m_cosine;
  }

  public static DActivationFunction randomOutputFunction(ENeuralOutput p_output){

    if(p_output == ENeuralOutput.NOVeloX || p_output == ENeuralOutput.NOVeloY){
      return ArrayCalc.randomElement(m_all_functions);
    }

    Debug.LogError("MUST RETURN AN ACTIAVTOR");

    return null;
  }


  //RANDOM ARRAY GETTER FUNCTIONS
  public static DActivationFunction[] randomArray(int p_min_range, int p_max_range){

    DActivationFunction[] to_return = new DActivationFunction[Random.Range(p_min_range, p_max_range)];

    for(int i = 0 ; i<to_return.Length; i++){
      to_return[i] = ArrayCalc.randomElement(m_all_functions);
    }

    return to_return;

  }

  public static DActivationFunction[] randomArrayOfSize(int p_size){

    DActivationFunction[] to_return = new DActivationFunction[p_size];

    for(int i = 0 ; i<to_return.Length; i++){
      to_return[i] = ArrayCalc.randomElement(m_all_functions);
    }

    return to_return;
  }

}