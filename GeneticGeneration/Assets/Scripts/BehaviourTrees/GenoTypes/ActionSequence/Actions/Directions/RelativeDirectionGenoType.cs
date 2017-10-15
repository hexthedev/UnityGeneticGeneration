using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class RelativeDirectionGenoType : IDirectionGenoType
{
  int m_index;
  EObjectTypes m_to;
  float m_degrees;

  public RelativeDirectionGenoType(int p_index, EObjectTypes p_to, float p_degrees){
    m_index = p_index;
    m_to = p_to;
    m_degrees = p_degrees;
  }

  public void mutate()
  {
    m_index = IntCalc.mutate(m_index, 0, 10, EvolutionVars.index_mutation_multip());
    m_to = EnumCalc.randomValue<EObjectTypes>();
    m_degrees = FloatCalc.mutate(m_degrees, 0, 360, EvolutionVars.vector_rotation_mutation_multip());
    return;
  }

  public IDirection phenotype(VSequenceAction p_parent)
  {
    return new RelativeDirection(m_index, m_to, m_degrees, p_parent);
  }

  public void randomize()
  {
    m_index = EvolutionVars.index_rand_val();
    m_to = EnumCalc.randomValue<EObjectTypes>();
    m_degrees = EvolutionVars.vector_rotation_rand_val();
    return;
  }

  public IDirectionGenoType clone()
  {
    return new RelativeDirectionGenoType(m_index, m_to, m_degrees);
  }
}
