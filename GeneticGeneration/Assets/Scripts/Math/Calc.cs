using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Calc{
  public static class VectorCalc {

    ///<summary>Turns a Vector3 into a Vector2 by ignoring the z element</summary>
    public static Vector2 CalcVec3to2(Vector3 p_toCull){
      return new Vector2(p_toCull.x, p_toCull.y);
    }

    ///<summary>Turns a Vector3 into a Vector2 by making the z element 0</summary>
    public static Vector3 CalcVec2to3(Vector2 p_toGrow){
      return new Vector3(p_toGrow.x, p_toGrow.y, 0);
    }

    ///<summary>Makes all vector elements positive</summary>  
    public static Vector2 VectorAbs(Vector2 p_vector){
      return new Vector2(Mathf.Abs(p_vector.x), Mathf.Abs(p_vector.y));
    }

    ///<summary>Check difference in angle.  Returns false if difference greater than threshold. Threshold in degrees </summary>
    public static bool checkVec2Angle(Vector2 p_first, Vector2 p_second, float threshold_degrees){
      float check = Vector2.Angle(p_first, p_second);
      return check < threshold_degrees;
    }

    ///<summary>Returns Vector2 calcuated by angle, assuming angles by unit circle  </summary>
    public static Vector2 fromAngle(float p_deg){
      float radians = p_deg * Mathf.Deg2Rad;
      return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;
    }

    ///<summary>Returns Angle between two vectors. Returns a signed angle showing direction from from_angle  </summary>
    public static float getAngle(Vector2 p_from_angle, Vector2 p_to_angle){ 
      float angle = Vector2.Angle(p_from_angle, p_to_angle);
			Vector3 cross = Vector3.Cross(p_from_angle, p_to_angle);
			angle = cross.z > 0 ? -angle : angle;
      return angle;
    } 

    ///<summary>Returns a vectors representing forward based on local_rotation and offset of forward.
    ///Assume forward_offset of 0 = local forward of [1, 0] like unit circle </summary>
    public static Vector2 forwardVector(float p_local_rotation, float p_forward_offset){
      return fromAngle(p_local_rotation + p_forward_offset);
    }

    ///<summary>Rotates a direction vector around the origin by degrees.!-- returns new vector </summary>
    public static Vector3 rotateDirectionVector(Vector3 p_direction, float p_degrees){   
      return Quaternion.Euler(0,0,p_degrees) * p_direction;
    }

    public static Vector2 randomDirection(){
      return new Vector2(RandomCalc.Rand(new MinMaxFloat(-1f, 1)), RandomCalc.Rand(new MinMaxFloat(-1f, 1)));
    }

    public static Vector2 mutateVector2(Vector2 p_mutate){
      return new Vector2(p_mutate.x * EvolutionVars.direction_mutation_multip(), p_mutate.y * EvolutionVars.direction_mutation_multip());
    }

    public static Vector2 clone(Vector2 to_clone){
      return new Vector2(to_clone.x, to_clone.y);
    }


  }

  public static class ArrayCalc {

    ///<summary>Returns sum of all floats in array </summary>
    public static float floatArraySum(float[] p_array){
      
      float to_return = 0;

      foreach(float number in p_array){
        to_return += number; 
      }

      return to_return;
    }

    public static T2[] map<T1, T2>(T1[] p_to_convert, DConversion<T1, T2> p_converter){
      T2[] to_return = new T2[p_to_convert.Length];

      for(int i = 0; i<p_to_convert.Length; i++){
        to_return[i] = p_converter(p_to_convert[i]);
      }

      return to_return;
    }

    public static T randomElement<T>(T[] p_array){
      if(p_array.Length == 0) { Debug.Log("EMPTY ARRAY"); };
      
      return p_array[randomIndex<T>(p_array)];
    }

    public static int randomIndex<T>(T[] p_array){
      int rand = RandomCalc.Rand(new MinMaxInt(0,p_array.Length-1));
      return rand;
    }
  }

  public static class EnumCalc{

    ///<summary>Return Random enum value </summary>
    public static T randomValue<T>(){
      System.Array array = System.Enum.GetValues(typeof(T));
      return (T)array.GetValue(RandomCalc.Rand(new MinMaxInt(0,array.Length-1)));
    }

  }

  public static class BoolCalc{

    ///<summary>Return Random enum value </summary>
    public static bool random(){
      return RandomCalc.Rand(new MinMaxInt(0,1)) == 1? true: false;
    }

  }

  public static class FloatCalc{

    public static float mutate(float p_value, float p_min, float p_max, float p_mutation_value){
      float to_return =  p_value * p_mutation_value;

      if(to_return > p_max){
        return p_max;
      } else if (to_return < p_min){
        return p_min;
      } else {
        return to_return;
      }
    }

  }

  public static class IntCalc{

    public static int mutate(int p_value, int p_min, int p_max, float p_mutation_value){
      float to_return = FloatCalc.mutate((float)p_value, (float)p_min, (float)p_max, p_mutation_value);
      return (int) to_return;
    }

  }

  public static class MatrixCalc{

    public static float[ , ] Multiply(float[ , ] input, float[ , ] output){

      int[] inputSize = {input.GetLength(0), input.GetLength(1)};
      int[] outputSize = {output.GetLength(0), output.GetLength(1)};

      if(inputSize[0] != outputSize[1]){
        Debug.LogError("Invalid Matrix multiplication");
      }

      float[ , ] result = new float[outputSize[0] , inputSize[1]];

      for(int y = 0; y < inputSize[1]; y++){
        for(int x = 0; x < outputSize[0]; x++){
          for(int i = 0; i<inputSize[0]; i++){
            result[x, y] += input[i, y] * output[x,i];
          }
        }
      }
      
      return result;

    }

    public static string String(float[,] p_matrix){

      string to_return = "";

      for(int i = 0; i<p_matrix.GetLength(1); i++){
        to_return += "[";

        for(int j = 0; j<p_matrix.GetLength(0); j++){
          to_return += p_matrix[j,i] + " ";
        }

        to_return += "]\n";
      }

      return to_return;
    }

    public static void randomPopulate(float[,] p_matrix){

      for(int x = 0; x<p_matrix.GetLength(0); x++){
       for(int y = 0; y<p_matrix.GetLength(1); y++){
         p_matrix[x,y] = RandomCalc.Rand(new MinMaxFloat(0f,1f));
       } 
      }
    }

    public static float[,] clone(float[,] p_matrix){
      float[,] to_return = new float[ p_matrix.GetLength(0), p_matrix.GetLength(1) ];

      for(int i = 0; i<p_matrix.GetLength(0);i++){
        for(int j = 0; j<p_matrix.GetLength(1);j++){
          to_return[i,j] = p_matrix[i,j];
        }
      }

      return to_return;
    }

    public static float[,] crossover(float[,] p_matrix1, float[,] p_matrix2){      
      if(!isSameSize(p_matrix1, p_matrix2)){
        Debug.LogError("CANNOT CROSSOVER MATRIX, NOT SAME SIZE");
      }
      
      float[,] to_return = new float[ p_matrix1.GetLength(0), p_matrix2.GetLength(1) ];

      for(int i = 0; i<to_return.GetLength(0);i++){
        for(int j = 0; j<to_return.GetLength(1);j++){
          to_return[i,j] = BoolCalc.random() ? p_matrix1[i,j] : p_matrix2[i,j];
        }
      }

      return to_return;
    }

    public static bool isSameSize(float[,] p_matrix1, float[,] p_matrix2){
      return p_matrix1.GetLength(0) == p_matrix2.GetLength(0) && p_matrix1.GetLength(1) == p_matrix2.GetLength(1);
    }

  }
  

  public static class RandomCalc{

    public static float Rand(MinMaxFloat p){
      return Random.Range(p.getMin(), p.getMax());
    }
    
    public static int Rand(MinMaxInt p){
      return Random.Range(p.getMin(), p.getMax()+1);
    }

  }
  

}
