using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class TowardsPlayerDirection : IDirection
{
  
	BehaviourTree m_tree;

	GameObject m_player;

	GameObject m_actor;
		

	//LOADING
  public void load(BehaviourTree p_tree)
  {
    m_tree = p_tree;
		m_player = m_tree.GetLogger().getByType(EObjectTypes.PLAYER)[0];
		m_actor = m_tree.getActor();
  }

  public void unload()
  {
//  m_tree = null;
		m_player = null;
		m_actor = null;
  }


	//CALCUATION
	public Vector2 direction()
  {
    Vector3 vecToPlayer = (m_player.transform.position - m_actor.transform.position).normalized;
		return VectorCalc.CalcVec3to2(vecToPlayer);
  }

	public static IDirection random(){
		return new TowardsPlayerDirection();
	}

  public IDirection clone()
  {
    return new TowardsPlayerDirection();
  }
}
