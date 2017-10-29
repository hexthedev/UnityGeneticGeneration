using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;
using Calc.Vector;

namespace GeneticBehaviourTrees
{
  public class AbsoluteDirectionGenoType : IDirectionGenoType
  {

    Vector2 m_direction;

    public AbsoluteDirectionGenoType(Vector2 p_direction)
    {
      m_direction = p_direction;
    }

    public void mutate()
    {
      m_direction = new Vector2(EvolutionVars.direction_mutation_multip() * m_direction.x, EvolutionVars.direction_mutation_multip() * m_direction.y).normalized;
    }

    public IDirection phenotype(VSequenceAction p_parent)
    {
      return new AbsoluteDirection(m_direction, p_parent);
    }

    public void randomize()
    {
      m_direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

    public IDirectionGenoType clone()
    {
      return new AbsoluteDirectionGenoType(Vector2Calc.clone(m_direction));
    }

  }
}