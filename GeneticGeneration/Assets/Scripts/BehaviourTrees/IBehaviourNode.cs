using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehaviourNode {

	IBehaviourNode act();
	//BehaviourTree GetTree();
	void load(BehaviourTree p_tree);
	void unload();

	IBehaviourNode clone();

	
}
