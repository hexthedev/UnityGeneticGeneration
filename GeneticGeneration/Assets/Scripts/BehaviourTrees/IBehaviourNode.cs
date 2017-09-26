using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehaviourNode {

	IBehaviourNode act();
	BehaviourTree GetTree();
	void initialize(BehaviourTree p_tree);
	
}
