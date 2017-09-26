using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction {

	bool act(BehaviourTree p_tree);
	void reset();


}
