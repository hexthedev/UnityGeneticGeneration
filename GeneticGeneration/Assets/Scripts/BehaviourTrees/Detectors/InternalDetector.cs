using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternalDetector : VDetector
{
  private ETrait m_trait;

	//This means is current trait or is total trait;
	private bool m_is_current;

	private float m_threshold;

	private bool m_is_above;
	
	public InternalDetector(ETrait p_trait, bool p_is_current, float p_threshold, bool p_is_above,  IBehaviourNode p_true_child, IBehaviourNode p_false_child) : base(p_true_child, p_false_child)
  {
		m_trait = p_trait;
		m_is_current = p_is_current;
		m_threshold = p_threshold;
		m_is_above = p_is_above;
  }

  public override bool detect()
  {
    debug();
		
		if(m_is_above){
			if( m_tree.getActor().GetComponent<EnemyController>().getTrait(m_trait, m_is_current) >= m_threshold ){
				return true;
			}
		} else {
			if( m_tree.getActor().GetComponent<EnemyController>().getTrait(m_trait, m_is_current) <= m_threshold ){
				return true;
			}
		}
		
		return false;

  }

	private void debug(){
		Debug.Log(m_trait + ", CUR? " + m_is_current + ", ABO?" + m_is_above);
	}
}
