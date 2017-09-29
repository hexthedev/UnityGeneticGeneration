using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomGen {

	public static IDirection IDirection(){
		
		int percent = Random.Range(0,101);

		if(percent <= 50){
			return AbsoluteDirection.random();
		} else {
			return TowardsPlayerDirection.random();
		}
	}

	public static IAction IAction(){
		return MoveAction.random();
	}

	public static IBehaviourNode Detector(){

		int percent = Random.Range(0, 101);

		if(percent <= 25){
			return DirectionDetector.random();
		} else if (percent <= 50){
			return InternalDetector.random();
		}	else if (percent <= 75){
			return PointingAtDetector.random();
		}	else{
			return ProximityDetector.random();
		}

	}

	public static IBehaviourNode Detector(IBehaviourNode p_true_child, IBehaviourNode p_false_child){

		int percent = Random.Range(0, 101);

		if(percent <= 25){
			return DirectionDetector.random(p_true_child, p_false_child);
		} else if (percent <= 50){
			return InternalDetector.random(p_true_child, p_false_child);
		}	else if (percent <= 75){
			return PointingAtDetector.random(p_true_child, p_false_child);
		}	else{
			return ProximityDetector.random(p_true_child, p_false_child);
		}

	}

	public static IBehaviourNode IBehaviourNode(){

		if(Random.Range(0,2) == 1){
			return null;
		}

		return Random.Range(0,2) == 1 ? Detector() : ActionSequence.random(); 
	}

	public static IBehaviourNode BehaviourTreeRoot(){
		return Detector(ActionSequence.random(), ActionSequence.random() );
	}


}
