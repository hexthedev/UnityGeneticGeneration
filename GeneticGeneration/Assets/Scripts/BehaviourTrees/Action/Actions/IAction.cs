using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction {

	bool act();
	void reset();


	void load(BehaviourTree p_tree);
	void unload();

	IAction clone();
}
