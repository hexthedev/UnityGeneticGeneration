using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class TowardsPlayerDirection : IDirection
{
  
	GameObject m_player;

	GameObject m_actor;
	
	public TowardsPlayerDirection(GameObject p_player, GameObject p_actor){
		m_player = p_player;
		m_actor = p_actor;
	}
	
	public Vector2 direction()
  {
    Vector3 vecToPlayer = (m_player.transform.position - m_actor.transform.position).normalized;
		return VectorCalc.CalcVec3to2(vecToPlayer);
  }

	public static IDirection random(BehaviourTree p_tree){
		return new TowardsPlayerDirection(p_tree.GetLogger().getByType(EObjectTypes.PLAYER)[0], p_tree.getActor());
	}

}
