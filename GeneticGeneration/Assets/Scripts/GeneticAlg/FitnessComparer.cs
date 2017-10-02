using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitnessComparer : IComparer<float>
{
  public int Compare(float x, float y)
  {
    float p_x = (float)x;
		float p_y = (float)y;

		if(p_x < p_y){
			return -1;
		} else if (p_x > p_y){
			return 1;
		} else {
			return 1;
		}
  }
}
