using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehaviourNode {

	IBehaviourNode act();

	//CONSTRUCTION
	void setParent(IBehaviourNode p_parent);

	IBehaviourNode returnToRoot();

	//LOADING
	void load(BehaviourTree p_tree);
	void unload();

	//CLONING
	IBehaviourNode clone();

	//Evoloving
	IBehaviourNode getRandomNode();

	void insertAtRandom(IBehaviourNode p_node);
	
}
