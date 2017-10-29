using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeneticBehaviourTrees
{
  public class AbsoluteDirection : VActionDirection
  {
    private Vector2 m_direction;

    public AbsoluteDirection(Vector2 p_direction, VSequenceAction p_action) : base(p_action)
    {
      m_direction = p_direction;
    }

    //COMPUTATION
    public override Vector2 direction()
    {
      return m_direction;
    }
  }
}