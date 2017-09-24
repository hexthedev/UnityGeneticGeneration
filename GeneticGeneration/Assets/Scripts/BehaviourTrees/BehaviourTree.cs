using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree {

	private BehaviourNode m_root;

	private BehaviourNode m_current_behav;


	private ObjectLogger m_logger;
	private GameObject m_actor;

	public BehaviourTree(BehaviourNode p_root, ObjectLogger p_logger, GameObject p_actor){
		m_root = p_root;
		m_current_behav = m_root;
		m_logger = p_logger;
		m_actor = p_actor;

		m_root.setTree(this);
	}

	public void act(){
		m_current_behav = m_current_behav.act();
	}

}
