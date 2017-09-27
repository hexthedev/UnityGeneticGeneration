using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDirection {

	Vector2 direction();

	void load(BehaviourTree p_tree);
	void unload();

	IDirection clone();
}
