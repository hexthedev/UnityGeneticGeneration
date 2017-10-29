using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;   //WARNING, requires TaskParallelle to work
using MathNet.Numerics.Random;   //WARNING, requires TaskParallelle to work


///<summary>Custom Calcuatoin functions written by James McCafferty for use in unity projects</summary>
namespace Calc{
  ///<summary>Represents a range from T min to T max</summary>
  public struct Range<T> {

    private T m_min;
    private T m_max;

    public Range(T p_min, T p_max){
      m_min = p_min;
      m_max = p_max;
    }

    public T Min{ get { return m_min; } }
    public T Max{ get { return m_max; } }
  }

  
  ///<summary>Delegate function used to convert Type 1 to Type 2</summary>
  public delegate T2 DConversion<T1, T2>(T1 p_convert);



  ///<summary>Calcuation functions for Vector2 and Vector3</summary>
  namespace Vector{
    
    public static class Vector2Calc {

      ///<summary>Turns a Vector3 into a Vector2 by ignoring the z element</summary>
      public static Vector2 fromVector3(Vector3 p_toCull){
        return new Vector2(p_toCull.x, p_toCull.y);
      }

      ///<summary>Makes all vector elements positive</summary>  
      public static Vector2 Abs(Vector2 p_vector){        
        return new Vector2(Mathf.Abs(p_vector.x), Mathf.Abs(p_vector.y));
      }

      ///<summary>Check difference in angle. Returns false if difference greater than threshold. Threshold in degrees </summary>
      public static bool checkAngle(Vector2 p_first, Vector2 p_second, float threshold_degrees){
        float check = Vector2.Angle(p_first, p_second);
        return check < threshold_degrees;
      }

      ///<summary>Returns Vector2 calcuated by angle in degrees. Angle 0 is same as Angle 0 on unit circle [Vec2(1,0)] </summary>
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

      ///<summary>Returns a vector representing forward based on local_rotation and offset of forward.
      ///Assume forward_offset of 0 = local forward of [1, 0] like unit circle </summary>
      public static Vector2 forwardVector(float p_local_rotation, float p_forward_offset){
        return fromAngle(p_local_rotation + p_forward_offset);
      }

      ///<summary>Rotates a direction vector around the origin by degrees. CounterClockwise </summary>
      public static Vector3 rotateDirectionVector(Vector3 p_direction, float p_degrees){   
        return Quaternion.Euler(0,0,p_degrees) * p_direction;
      }

      ///<summary>Rotates a normalized direction vector around the origin by degrees. CounterClockwise </summary>
      public static Vector2 randomDirection(){
        return (new Vector2(Rand.RandomCalc.Rand(new Range<float>(-1f, 1)), Rand.RandomCalc.Rand(new Range<float>(-1f, 1)))).normalized;
      }

      ///<summary>Multiplies each element of Vector2 by random number represented by range</summary>
      public static Vector2 elementwiseRandomMultiply(Vector2 p_mutate, Range<float> p_range){
        return new Vector2(p_mutate.x * Rand.RandomCalc.Rand(p_range), p_mutate.y * Rand.RandomCalc.Rand(p_range));
      }

      ///<summary>Get reference to new vector with same elements</summary>
      public static Vector2 clone(Vector2 to_clone){
        return new Vector2(to_clone.x, to_clone.y);
      }

    }

    public static class Vector3Calc {
      ///<summary>Turns a Vector3 into a Vector2 by making the z element 0</summary>
      public static Vector3 fromVec2(Vector2 p_toGrow){
        return new Vector3(p_toGrow.x, p_toGrow.y, 0);
      }

      ///<summary>Makes all vector elements positive</summary>  
      public static Vector3 VectorAbs(Vector3 p_vector){
        return new Vector3(Mathf.Abs(p_vector.x), Mathf.Abs(p_vector.y), Mathf.Abs(p_vector.z));
      }


    }
  }

  ///<summary>Calculation funtions for arrays</summary>
  namespace Array{
      
    public static class ArrayCalc {

      ///<summary>Returns sum of all floats in array </summary>
      public static float floatArraySum(float[] p_array){
        
        float to_return = 0;

        foreach(float number in p_array){
          to_return += number; 
        }

        return to_return;
      }

      ///<summary>Converts array from type T1 to type T2 </summary>
      public static T2[] map<T1, T2>(T1[] p_to_convert, DConversion<T1, T2> p_converter){
        T2[] to_return = new T2[p_to_convert.Length];

        for(int i = 0; i<p_to_convert.Length; i++){
          to_return[i] = p_converter(p_to_convert[i]);
        }

        return to_return;
      }

      ///<summary> Returns a random element from an array  </summary>
      public static T randomElement<T>(T[] p_array){
        if(p_array.Length == 0) { Debug.LogError("EMPTY ARRAY: Cannot get random element"); };
        
        return p_array[randomIndex<T>(p_array)];
      }

      ///<summary>Returns a random index that contains an element from an array </summary>
      public static int randomIndex<T>(T[] p_array){
        int rand = Rand.RandomCalc.Rand(new Range<int>(0,p_array.Length-1));
        return rand;
      }

      ///<summary>For each index of an array takes a random element from either array_1 or array_2. Arrays must be same size.</summary>
      public static T[] crossover<T>(T[] thing_1, T[] thing_2){
        if(thing_1.Length != thing_2.Length) Debug.LogError("Crossover arrays wrong size");

        T[] new_array = new T[thing_1.Length];

        for(int i = 0; i<thing_1.Length; i++){
          if(Bool.BoolCalc.random()){
            new_array[i] = thing_1[i];
          } else {
            new_array[i] = thing_2[i];
          }
        }
        return new_array;
      }
    }

  }

  ///<summary>Calculation funtions for enumerated types</summary>
  namespace Enum{
    public static class EnumCalc{

      ///<summary>Return Random enum value </summary>
      public static T randomValue<T>(){
        System.Array array = System.Enum.GetValues(typeof(T));
        return (T)array.GetValue(Rand.RandomCalc.Rand(new Range<int>(0,array.Length-1)));
      }

    }
  }

  ///<summary>Calculation funtions for boolean types</summary>
  namespace Bool{
    public static class BoolCalc{
      ///<summary>Return true or false randomly </summary>
      public static bool random(){
        return Rand.RandomCalc.Rand(new Range<int>(0,1)) == 1? true: false;
      }
    }
  }

  ///<summary>Calculation funtions for float types</summary>
  namespace Float{
    public static class FloatCalc{

      ///<summary>Multiplies a float that can only take on a value between a range </summary>
      public static float boundedMultiply(float p_value, Range<float> p_range, float p_mutation_value){
        return Mathf.Clamp(p_value * p_mutation_value, p_range.Min, p_range.Max);
      }
    }
  }

  ///<summary>Calculation funtions for integer types</summary>
  namespace Int{
    public static class IntCalc{

      ///<summary>Multiplies a int that can only take on a value between a range. Multiplication done using a float then rounding </summary>
      public static int boundedMultiply(int p_value, Range<int> p_range, float p_mutation_value){
        return (int) Mathf.RoundToInt(Float.FloatCalc.boundedMultiply(p_value, new Range<float>(p_range.Min, p_range.Max), p_mutation_value)); 
      }

    }
  }

  ///<summary>Calculation funtions for Matrices from the Math.Net.Numerics Library</summary>
  namespace Matrices{

    public static class MatrixCalc{

      ///<summary>Get a randomly populated float matrix</summary>
      public static Matrix<float> randomFloatMatrix(int p_rows, int p_cols, Range<float> p_range){
        return Matrix<float>.Build.Dense(p_rows, p_cols, (i, j) => { return Rand.RandomCalc.Rand(p_range); } );
      }

      ///<summary>Crossover two float matrices to create a new matrix</summary>
      public static Matrix<float> crossover(Matrix<float> p_matrix1, Matrix<float> p_matrix2){      
        if(!isSameSize(p_matrix1, p_matrix2)) Debug.LogError("CANNOT CROSSOVER MATRIX, NOT SAME SIZE");

        int rows = p_matrix1.RowCount;
        int cols = p_matrix2.ColumnCount;

        Matrix<float> to_return = Matrix<float>.Build.Dense(rows, cols);

        for(int i = 0; i<rows; i++){
          for(int j = 0; j<cols; j++){
            to_return[i,j] = Bool.BoolCalc.random() ? p_matrix1[i,j] : p_matrix2[i,j];
          }
        }

        return to_return;
      }
      
      public static Matrix<float> elementwiseRandomMultiply(Matrix<float> p_matrix, Range<float> p_range){

        Matrix<float> matrix = Matrix<float>.Build.Dense(p_matrix.RowCount, p_matrix.ColumnCount);

        for(int i = 0; i<p_matrix.RowCount; i++){
          for(int j = 0; j<p_matrix.ColumnCount; j++){
            matrix[i,j] = p_matrix[i,j] * Rand.RandomCalc.Rand(p_range);
          }
        }

        return matrix;
      }

      ///<summary>Check if two float matrices are the same size</summary>
      public static bool isSameSize(Matrix<float> p_matrix1, Matrix<float> p_matrix2){
        if(p_matrix1.RowCount != p_matrix2.RowCount || p_matrix1.ColumnCount != p_matrix2.ColumnCount){
          return false;
        }

        return true;
      }

    }


  }

  ///<summary>Calculation functions for random number generation</summary>
  namespace Rand{
    ///<summary>Calculation funtions for Random Generation</summary>
    public static class RandomCalc{

      private static MersenneTwister m_random;

      static RandomCalc(){
        m_random = new MersenneTwister();
      }

      ///<summary>Return random float in range</summary>
      public static float Rand(Range<float> p){        
        return  (float)m_random.NextDouble() * (p.Max-p.Min) + p.Min;
      }
      
      ///<summary>Return a random int in range</summary>
      public static int Rand(Range<int> p){
        return m_random.Next(p.Min, p.Max+1);
      }

      ///<summary>True if random number is lower than p_chance (percentage 0-100)</summary>
      public static bool ChanceRoll(float p_chance){
        return p_chance >= Rand(new Range<float>(0,100)) ? true : false;
      }

    }
  }



}
