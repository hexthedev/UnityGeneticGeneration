using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate float DActivationFunction(float value);

public static class Activators{

  public static DActivationFunction Round(){
    return (float p_value) => { return Mathf.Round(p_value); };
  }

  public static DActivationFunction Sqrt(){
    return (float p_value) => { return Mathf.Sqrt(Mathf.Abs(p_value)); };
  }

}