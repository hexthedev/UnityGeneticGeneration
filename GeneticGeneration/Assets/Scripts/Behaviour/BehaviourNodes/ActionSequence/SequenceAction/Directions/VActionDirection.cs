using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VActionDirection : IDirection
{

	protected VSequenceAction m_action;
  
	protected VActionDirection(VSequenceAction p_action){
		m_action = p_action;
	}

	public abstract Vector2 direction();
}
