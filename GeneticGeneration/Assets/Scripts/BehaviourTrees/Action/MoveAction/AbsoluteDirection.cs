﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsoluteDirection : IDirection
{
	
	Vector2 m_direction;

  public AbsoluteDirection(Vector2 p_direction)
  {
		m_direction = p_direction;
  }

  public Vector2 direction()
  {
    return m_direction;
  }

  public static IDirection random(){
    return new AbsoluteDirection(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
  }

}
