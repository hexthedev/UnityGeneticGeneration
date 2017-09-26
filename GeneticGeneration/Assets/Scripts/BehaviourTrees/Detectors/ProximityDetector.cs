using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class ProximityDetector : VDetector {
  
	private EObjectTypes m_type;
	private float m_threshold;

	public ProximityDetector(EObjectTypes p_type, float p_threshold, IBehaviourNode p_true_node, IBehaviourNode p_false_node):base(p_true_node, p_false_node){
		m_type = p_type;
		m_threshold = p_threshold;
	}

	public override bool detect()
  {
    GameObject[] objects = m_tree.GetLogger().getByType(m_type);

		for(int i = 0; i< objects.Length; i++){
			Vector3 object_position = objects[i].transform.position;
			
			bool is_close = Vector3.Magnitude(object_position - m_tree.getActor().transform.position) < m_threshold;
			
			debug(object_position, is_close);

			if(is_close){
				return true;
			}
		}

		return false;		
  }

	private void debug( Vector3 p_tested_position, bool p_answer ){

		if(p_answer){
			Debug.DrawLine( m_tree.getActor().transform.position, p_tested_position, Color.green, 2f );
		} else {
			Debug.DrawLine( m_tree.getActor().transform.position, p_tested_position, Color.red, 2f );
		}

	}

}
