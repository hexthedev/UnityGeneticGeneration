using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree {

	private IBehaviourNode m_root;

	private IBehaviourNode m_current_behav;

	private ObjectLogger m_logger;
	private GameObject m_actor;	

	public BehaviourTree(IBehaviourNode p_root, ObjectLogger p_logger, GameObject p_actor){
		m_root = p_root;
		m_current_behav = m_root;
		m_logger = p_logger;
		m_actor = p_actor;

		m_root.initialize(this);
	}

	public void act(){
		m_current_behav = m_current_behav.act();
	}

	public GameObject getActor(){
		return m_actor;
	}

	public IBehaviourNode getRoot(){
		return m_root;
	}

	public ObjectLogger GetLogger(){
		return m_logger;
	}

}
