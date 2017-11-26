using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Genetic.Base;
using Genetic.Composite;
using Genetic.Traits.Base;

using JTools.Calc.Vectors;
using JTools.Events;
using JTools.DataStructures.PriorityList;
using JTools.Calc.Lines;
using JTools.Calc.ActiavationFunctions;
using Genetic.Behaviour.BehaviourTrees;

public class LineFollowingBTCreature : ALineFollowingCreature<
	LineFollowingBTCreature, 
	LineFollowingBTGameController, 
	MindBodyBTDNA<LineFollowingBTCreature>, 
	MindBodyBT, 
	BehaviourTree> {
	
	protected override LineFollowingBTCreature getSelf(){
		return this;
	}
}
