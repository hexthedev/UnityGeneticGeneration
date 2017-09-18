using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Calc{
  public static class VectorCalc {

    public static Vector2 CalcVec3to2(Vector3 p_toCull){
      return new Vector2(p_toCull.x, p_toCull.y);
    }

    public static Vector2 VectorAbs(Vector2 p_vector){
      return new Vector2(Mathf.Abs(p_vector.x), Mathf.Abs(p_vector.y));
    }

    public static bool vectorGreaterThan(Vector2 p_vec1, Vector2 p_vec2){
      return !(p_vec1.x > p_vec2.x  || p_vec1.y > p_vec2.y);
    }

    public static bool checkVec2(Vector2 p_first, Vector2 p_second, Vector2 threshold){
      Vector2 check = VectorAbs(p_first-p_second);
      return vectorGreaterThan(check, threshold);
    }

    public static Vector2 fromAngle(float p_deg){
      float radians = p_deg * Mathf.Deg2Rad;
      return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;
    }

    public static float getAngle(Vector2 p_from_angle, Vector2 p_to_angle){ 
      float angle = Vector2.Angle(p_from_angle, p_to_angle);
			Vector3 cross = Vector3.Cross(p_from_angle, p_to_angle);
			angle = cross.z > 0 ? -angle : angle;
      return angle;
    } 


  }

}
