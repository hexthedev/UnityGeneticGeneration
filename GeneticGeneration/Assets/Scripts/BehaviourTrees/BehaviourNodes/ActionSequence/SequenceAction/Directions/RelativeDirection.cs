﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Calc;
using Calc.Array;
using Calc.Vector;

namespace GeneticBehaviourTrees
{
  public class RelativeDirection : VActionDirection
  {
    private GameObject m_actor;

    private int m_index;

    private EObjectTypes m_to;

    private float m_degrees;



    public RelativeDirection(int p_index, EObjectTypes p_to, float p_degrees, VSequenceAction p_action) : base(p_action)
    {
      m_actor = m_action.getSequence().getTree().getActor();
      m_index = p_index;
      m_to = p_to;
      m_degrees = p_degrees;
    }

    //CALCUATION
    public override Vector2 direction()
    {
      GameObject[] objects = ObjectLogger.getByType(m_to);

      if (objects.Length == 0) { return Vector2.zero; }

      GameObject relative_object = m_index >= objects.Length ? ArrayCalc.randomElement(objects) : objects[m_index];

      Vector3 to_direction = (relative_object.transform.position - m_actor.transform.position).normalized;
      Vector3 move_direction = Vector2Calc.rotateDirectionVector(to_direction, m_degrees);

      // return VectorCalc.CalcVec3to2(move_direction);
      return Vector2.zero;
    }
  }
}