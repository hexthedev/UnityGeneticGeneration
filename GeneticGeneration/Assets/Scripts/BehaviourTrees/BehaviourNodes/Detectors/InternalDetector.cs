using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class InternalDetector : VDetector
{
  private ETrait m_trait;

	//This means is current trait or is total trait;
	private bool m_is_current;

	private float m_threshold;

	private bool m_is_above;
	
	public InternalDetector(ETrait p_trait, bool p_is_current, float p_threshold, bool p_is_above, BehaviourTree p_tree, VTreeNode<IBehaviourNode> p_parent) : base(p_tree, p_parent)
  {
		m_trait = p_trait;
		m_is_current = p_is_current;
		m_threshold = p_threshold;
		m_is_above = p_is_above;
  }

  public override bool detect()
  {
		float trait = m_tree.getActorController().getTrait(m_trait, m_is_current);

		if(m_is_above){
			if( trait >= m_threshold ){
				if(m_tree.getActorController().m_debug){
					debug(true);
				}
				
				return true;
			}
		} else {
			if( trait <= m_threshold ){
				
				if(m_tree.getActorController().m_debug){
					debug(true);
				}

				return true;
			}
		}
		
		if(m_tree.getActorController().m_debug){
			debug(false);
		}

		return false;

  }

	private void debug(bool answer){
		string cur = m_is_current ? "CURRENT" : "TOTAL";
		string above = m_is_above ? "GREATER THAN" : "LESS THAN";

		Debug.Log(cur + " " + m_trait + " " + above + " " + m_threshold + ": " + answer);
	}
}
