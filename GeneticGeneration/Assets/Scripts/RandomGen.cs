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

	public static IBehaviourNode Detector(BehaviourTree p_tree){

		int percent = Random.Range(0, 101);

		if(percent <= 25){
			return DirectionDetector.random(p_tree);
		} else if (percent <= 50){
			return InternalDetector.random(p_tree);
		}	else if (percent <= 75){
			return PointingAtDetector.random(p_tree);
		}	else{
			return ProximityDetector.random(p_tree);
		}

	}

		public static IBehaviourNode Detector(BehaviourTree p_tree, IBehaviourNode p_true_child, IBehaviourNode p_false_child){

		int percent = Random.Range(0, 101);

		if(percent <= 25){
			return DirectionDetector.random(p_tree, p_true_child, p_false_child);
		} else if (percent <= 50){
			return InternalDetector.random(p_tree, p_true_child, p_false_child);
		}	else if (percent <= 75){
			return PointingAtDetector.random(p_tree, p_true_child, p_false_child);
		}	else{
			return ProximityDetector.random(p_tree, p_true_child, p_false_child);
		}

	}

	public static IBehaviourNode IBehaviourNode(BehaviourTree p_tree){

		if(Random.Range(0,2) == 1){
			return null;
		}

		return Random.Range(0,2) == 1 ? Detector(p_tree) : ActionSequence.random(p_tree); 
	}

	public static IBehaviourNode BehaviourTreeRoot(BehaviourTree p_tree){
		return Detector(p_tree, ActionSequence.random(p_tree), ActionSequence.random(p_tree) );
	}


}
