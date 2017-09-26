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
	
	public InternalDetector(ETrait p_trait, bool p_is_current, float p_threshold, bool p_is_above,  IBehaviourNode p_true_child, IBehaviourNode p_false_child) : base(p_true_child, p_false_child)
  {
		m_trait = p_trait;
		m_is_current = p_is_current;
		m_threshold = p_threshold;
		m_is_above = p_is_above;
  }

  public override bool detect()
  {

		
		if(m_is_above){
			if( m_tree.getActor().GetComponent<EnemyController>().getTrait(m_trait, m_is_current) >= m_threshold ){
				if(m_tree.getActor().GetComponent<EnemyController>().m_debug){
					debug(true);
				}
				
				return true;
			}
		} else {
			if( m_tree.getActor().GetComponent<EnemyController>().getTrait(m_trait, m_is_current) <= m_threshold ){
				
				if(m_tree.getActor().GetComponent<EnemyController>().m_debug){
					debug(true);
				}

				return true;
			}
		}
		
		if(m_tree.getActor().GetComponent<EnemyController>().m_debug){
			debug(false);
		}

		return false;

  }

	private void debug(bool answer){
		string cur = m_is_current ? "CURRENT" : "TOTAL";
		string above = m_is_above ? "GREATER THAN" : "LESS THAN";

		Debug.Log(cur + " " + m_trait + " " + above + " " + m_threshold + ": " + answer);
	}


	public static InternalDetector random(BehaviourTree p_tree){
		ETrait trait = EnumCalc.randomValue<ETrait>();
		bool is_current = BoolCalc.random();
		float threshold = Random.Range(0f, 10f);
		bool is_above = BoolCalc.random();

		return new InternalDetector(trait, is_current, threshold, is_above, RandomGen.IBehaviourNode(p_tree), RandomGen.IBehaviourNode(p_tree));
	}

	public static InternalDetector random(BehaviourTree p_tree, IBehaviourNode p_true_child, IBehaviourNode p_false_child){
		ETrait trait = EnumCalc.randomValue<ETrait>();
		bool is_current = BoolCalc.random();
		float threshold = Random.Range(0f, 10f);
		bool is_above = BoolCalc.random();

		return new InternalDetector(trait, is_current, threshold, is_above, p_true_child, p_false_child);
	}
}
