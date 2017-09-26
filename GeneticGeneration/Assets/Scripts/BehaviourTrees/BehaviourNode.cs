using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourNode {

	BehaviourNode m_parent;

	ActionSequence m_actions;
	IDetector m_detector;

	BehaviourNode m_trueChild;
	BehaviourNode m_falseChild;
	
	public BehaviourNode(BehaviourNode p_parent, ActionSequence p_actions, IDetector p_detector){
		m_parent = p_parent;
		m_actions = p_actions;
		m_detector = p_detector;

		m_trueChild = null;
		m_falseChild = null;
	}

	public void addChild(bool p_parent_result, ActionSequence p_actions, IDetector p_detector){
		if(p_parent_result){
			m_trueChild = new BehaviourNode(this, p_actions, p_detector);
		} else {
			m_falseChild = new BehaviourNode(this, p_actions, p_detector);
		}
	}

	/*public BehaviourNode act(GameObject p_actor){
		//if m_action.act is true, then action sequence is over
		if(m_actions.act(p_actor)){

			//Reset the action sequence
			m_actions.reset();

			//if no detector exists, this a leaf node, restart tree
			if(m_detector == null){
				return returnToRoot();
			}

			//Now try detector. Act on the appropriate child node
			if(m_detector.detect()){
				return m_trueChild.act(p_actor);
			}
			
			return m_falseChild.act(p_actor);
		}

		return this;
	}*/

	private BehaviourNode returnToRoot(){
		if(m_parent == null){
			return this;
		} else {
			return m_parent.returnToRoot();
		}
	}

	//STATIC 
	// public static BehaviourNode evolove(BehaviourNode p_node1, BehaviourNode p_node2){
		
	// 	BehaviourNode master; 
	// 	BehaviourNode lesser;

	// 	if(Random.Range(0f,1f) > 0.5f){
	// 		master = p_node1;
	// 		lesser = p_node2;
	// 	}
	// }
	
	
}
