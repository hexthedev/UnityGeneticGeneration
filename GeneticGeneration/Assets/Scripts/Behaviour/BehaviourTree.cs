using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree {

	//State information
	private VTreeNode<IBehaviourNode> m_root;
	private IBehaviourNode m_current_behav;
	
	//Reference Infomation
	private ObjectLogger m_logger;
	private GameObject m_actor;	
	private EnemyController m_actor_controller;
	private Rigidbody2D m_actor_body;

	private BehaviourDNA m_dna;


	//CONSTRUCTORS
	//For manually constructing behaviour trees
	public BehaviourTree(ObjectLogger p_logger, GameObject p_actor, BehaviourDNA p_dna){
		m_logger = p_logger;
		m_actor = p_actor;
		m_dna = p_dna;

		m_actor_controller = m_actor.GetComponent<EnemyController>();
		m_actor_body = m_actor.GetComponent<Rigidbody2D>();

		m_root = generatePhenotype(p_dna.getRoot(), null);
		m_current_behav = m_root.getSelf();
	}

	//BEHAVIOUR
	public void act(){
		m_current_behav = m_current_behav.act();
	}

	//QUERIES
	public GameObject getActor(){
		return m_actor;
	}

	public EnemyController getActorController(){
		return m_actor_controller;
	}

	public Rigidbody2D getActorBody(){
		return m_actor_body;
	}

	public ObjectLogger GetLogger(){
		return m_logger;
	}


	private VTreeNode<IBehaviourNode> generatePhenotype(VTreeNode<IBehaviourGenoType> p_node, VTreeNode<IBehaviourNode> p_parent){

		if(p_node == null){
			return null;
		}

		//Debug.Log(p_node);
		VTreeNode<IBehaviourNode> behav = p_node.getSelf().phenotype(p_parent, this);

		for(int i = 0; i<p_node.numChildren(); i++){
			behav.addChild( generatePhenotype(p_node.getChild(i), behav), i);
		}

		return behav;
	}
}
