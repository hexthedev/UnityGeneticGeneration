using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomGen {

	public static IDirection IDirection(BehaviourTree p_tree){
		
		int percent = Random.Range(0,101);

		if(percent <= 50){
			return AbsoluteDirection.random();
		} else {
			return TowardsPlayerDirection.random(p_tree);
		}
	}

	public static IAction IAction(BehaviourTree p_tree){
		return MoveAction.random(p_tree);
	}



}
