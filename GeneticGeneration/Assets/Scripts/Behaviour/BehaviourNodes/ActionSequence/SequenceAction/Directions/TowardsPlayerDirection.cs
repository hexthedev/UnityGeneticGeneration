using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class TowardsPlayerDirection : VActionDirection
{
  
	private GameObject m_player;

	private GameObject m_actor;
	
  public TowardsPlayerDirection(VSequenceAction p_action):base(p_action){
    m_player = m_action.getSequence().getTree().GetLogger().getByType(EObjectTypes.PLAYER)[0];
    m_actor = m_action.getSequence().getTree().getActor();
  }

	//CALCUATION
	public override Vector2 direction()
  {
    Vector3 vecToPlayer = (m_player.transform.position - m_actor.transform.position).normalized;
		return VectorCalc.CalcVec3to2(vecToPlayer);
  }
}
