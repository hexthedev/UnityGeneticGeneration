using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public delegate float DActivationFunction(float value);

public static class Activators{

  private static DActivationFunction m_round = (float p_value) => { return Mathf.Round(p_value); };

  private static DActivationFunction m_sqrt = (float p_value) => { 
    float sign = p_value/Mathf.Abs(p_value);
    
  Debug.Log("HERE------------------------    " + sign);

    return sign * Mathf.Sqrt(Mathf.Abs(p_value)); 
  };



  public static DActivationFunction Round(){
    return m_round;
  }

  public static DActivationFunction Sqrt(){
    return m_sqrt;
  }


  public static DActivationFunction[] randomArray(){

    DActivationFunction[] to_return = new DActivationFunction[Random.Range(1,3)];

    for(int i = 0 ; i<to_return.Length; i++){
      to_return[i] = BoolCalc.random() ? Round() : Sqrt();
    }

    return to_return;

  }

  public static DActivationFunction[] randomArrayOfSize(int p_size){

    DActivationFunction[] to_return = new DActivationFunction[p_size];

    for(int i = 0 ; i<to_return.Length; i++){
      to_return[i] = BoolCalc.random() ? Round() : Sqrt();
    }

    return to_return;

  }

}