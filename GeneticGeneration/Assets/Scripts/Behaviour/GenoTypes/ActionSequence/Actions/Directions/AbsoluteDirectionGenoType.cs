using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class AbsoluteDirectionGenoType : IDirectionGenoType
{
  
	Vector2 m_direction;

	public AbsoluteDirectionGenoType(Vector2 p_direction){
		m_direction = p_direction;
	}

  public void mutate()
  {
    m_direction = new Vector2( Random.Range(0.5f, 1.5f)*m_direction.x, Random.Range(0.5f, 1.5f)*m_direction.y );
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
    return new AbsoluteDirectionGenoType(VectorCalc.clone(m_direction));
  }

}
