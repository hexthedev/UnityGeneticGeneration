using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class ProximityDetector : VDetector {
  
	private EObjectTypes m_of;
	private float m_threshold;
	private int m_count;

	public ProximityDetector(EObjectTypes p_of, float p_threshold, int p_count, BehaviourTree p_tree, VTreeNode<IBehaviourNode> p_parent) : base(p_tree, p_parent){
		m_of = p_of;
		m_count = p_count;
		m_threshold = p_threshold;
	}

	public override bool detect()
  {
		GameObject[] objects = m_tree.GetLogger().getByType(m_of);
		int count = 0;

		for(int i = 0; i< objects.Length; i++){
			Vector3 object_position = objects[i].transform.position;
			
			bool is_close = Vector3.Magnitude(object_position - m_tree.getActor().transform.position) < m_threshold;
			
			if(m_tree.getActorController().m_debug){
				debug(object_position, is_close);
			}

			if(is_close){
				count++;
			}

			if(count >= m_count){
				return true;
			}
		}

		return false;		
  }

	private void debug( Vector3 p_tested_position, bool p_answer ){

		if(p_answer){
			Debug.DrawLine( m_tree.getActor().transform.position, p_tested_position, Color.green, Time.deltaTime );
		} else {
			Debug.DrawLine( m_tree.getActor().transform.position, p_tested_position, Color.red, Time.deltaTime );
		}

	}
}
