﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class MoveActionGenoType : IActionGenoType
{
	
	IDirectionGenoType m_direction;
	float m_timeLimit;
	float m_speed_percentage;
	bool m_sudden;

	public MoveActionGenoType(float p_timerLimit, float p_speed_percentage, bool p_sudden, IDirectionGenoType p_direction){
		m_timeLimit = p_timerLimit;
		m_speed_percentage = p_speed_percentage;
		m_direction = p_direction;
		m_sudden = p_sudden;
	}

  public void mutate()
  {
    m_direction.mutate();
    m_timeLimit = FloatCalc.mutate(m_timeLimit, 0f, 4f, EvolutionVars.action_time_limit_rand_mult() );
    m_speed_percentage = FloatCalc.mutate(m_speed_percentage, 0.2f, 1f, EvolutionVars.action_speed_rand_mult());
    //m_sudden = BoolCalc.random();
  }

  public IAction phenotype(ActionSequence p_parent)
  {
    return new MoveAction(m_timeLimit, m_speed_percentage, m_sudden, m_direction, p_parent);
  }

  public void randomize()
  {
    m_direction.randomize();
    m_timeLimit = EvolutionVars.action_time_limit_rand_value();
    m_speed_percentage = EvolutionVars.action_speed_rand_value();
    m_sudden = BoolCalc.random();
  }


  public IActionGenoType clone(ActionSequenceGeno p_parent)
  {
    return new MoveActionGenoType(m_timeLimit, m_speed_percentage, m_sudden, m_direction.clone());
  }


}