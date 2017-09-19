using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction {

	bool act(GameObject p_actor);
	void reset();
}
