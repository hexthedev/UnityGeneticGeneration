using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree {

	private IBehaviourNode m_root;

	private IBehaviourNode m_current_behav;

	private ObjectLogger m_logger;
	private GameObject m_actor;	

	private EnemyController m_actor_controller;

	public BehaviourTree(IBehaviourNode p_root, ObjectLogger p_logger, GameObject p_actor){
		m_root = p_root;
		m_current_behav = m_root;
		m_logger = p_logger;
		m_actor = p_actor;

		m_actor_controller = m_actor.GetComponent<EnemyController>();

		m_root.initialize(this);
	}



	//Required to take apart the Behaviour Trees properly
	private BehaviourTree(ObjectLogger p_logger, GameObject p_actor){
		m_logger = p_logger;
		m_actor = p_actor;

		m_actor_controller = m_actor.GetComponent<EnemyController>();
	}






	public void act(){
		m_current_behav = m_current_behav.act();
	}

	public GameObject getActor(){
		return m_actor;
	}

	public EnemyController getActorController(){
		return m_actor_controller;
	}

	public IBehaviourNode getRoot(){
		return m_root;
	}

	public ObjectLogger GetLogger(){
		return m_logger;
	}







	public static BehaviourTree random(ObjectLogger p_logger, GameObject p_actor){
		
		BehaviourTree creating = new BehaviourTree(p_logger, p_actor);

		IBehaviourNode root = RandomGen.BehaviourTreeRoot(creating);

		root.initialize(creating);

		creating.m_root = root;
		creating.m_current_behav = root;

		return creating;
	}

}
