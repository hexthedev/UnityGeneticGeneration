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

	private BehaviourTree(ObjectLogger p_logger, GameObject p_actor){
		m_logger = p_logger;
		m_actor = p_actor;
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

	public static BehaviourTree random(ObjectLogger p_logger, GameObject p_actor){
		
		BehaviourTree creating = new BehaviourTree(p_logger, p_actor);

		IBehaviourNode root = new DirectionDetector(EObjectTypes.PLAYER, Vector2.one, 90f, 1,  
		ActionSequence.random(creating, null), 
		ActionSequence.random(creating, null));

		root.initialize(creating);

		creating.m_root = root;
		creating.m_current_behav = root;

		return creating;
	}

}
