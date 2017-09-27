using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree {

	private IBehaviourNode m_root;

	private IBehaviourNode m_current_behav;


	bool m_is_loaded;
	private ObjectLogger m_logger;
	private GameObject m_actor;	

	private EnemyController m_actor_controller;



	//CONSTRUCTORS
	//For manually constructing behaviour trees
	public BehaviourTree(IBehaviourNode p_root, ObjectLogger p_logger, GameObject p_actor){
		m_root = p_root;
		m_current_behav = m_root;
		m_logger = p_logger;
		m_actor = p_actor;

		m_actor_controller = m_actor.GetComponent<EnemyController>();

		m_root.load(this);
		m_is_loaded = true;
	}



	//Required to take apart the Behaviour Trees properly
	/*private BehaviourTree(ObjectLogger p_logger, GameObject p_actor){
		m_logger = p_logger;
		m_actor = p_actor;

		m_actor_controller = m_actor.GetComponent<EnemyController>();
	}*/

	//For creating unloaded random behaviour trees
	private BehaviourTree(IBehaviourNode p_root){
		m_root = p_root;
		m_current_behav = p_root;
		m_is_loaded = false;
	}



	//LOADING
	public void load(ObjectLogger p_logger, GameObject p_actor){
		m_logger = p_logger;
		m_actor = p_actor;
		m_actor_controller = m_actor.GetComponent<EnemyController>();
		m_is_loaded = true;
		m_root.load(this);
	}

	public void unload(){
		m_logger = null;
		m_actor = null;
		m_actor_controller = null;
		m_is_loaded = false;
		m_root.unload();
	}

	//BEHAVIOUR
	public void act(){
		if(!m_is_loaded){
			Debug.Log("CANNOT ACT: BEHAVIOUR TREE NOT LOADED");
		}

		m_current_behav = m_current_behav.act();
	}

	//QUERIES
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

	//RANDOM
	public static BehaviourTree random(){
		return new BehaviourTree( RandomGen.BehaviourTreeRoot() );
	}

	//COPYING
	public BehaviourTree clone(){
		return new BehaviourTree(m_root.clone());
	}

}
