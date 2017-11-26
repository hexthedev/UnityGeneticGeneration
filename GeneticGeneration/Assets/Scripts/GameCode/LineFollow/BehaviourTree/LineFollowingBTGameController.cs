using System.Collections;
using System.Collections.Generic;

using Genetic.Base;
using Genetic.Behaviour.BehaviourTrees;
using Genetic.Composite;
using Genetic.Traits.Base;
using Genetic.Traits.TraitGenes;
using JTools.Calc.ActiavationFunctions;
using JTools.Calc.Base;
using JTools.Calc.Lines;
using JTools.Calc.Vectors;
using JTools.Events;
using UnityEngine;

public class LineFollowingBTGameController : ALineFollowingGameController<
	LineFollowingBTCreature, 
	LineFollowingBTGameController, 
	MindBodyBTDNA<LineFollowingBTCreature>, 
	MindBodyBT, 
	BehaviourTree>{

	protected override DNABasedEvolutionManager<MindBodyBTDNA<LineFollowingBTCreature>> createManager(){

		return new DNABasedEvolutionManager<MindBodyBTDNA<LineFollowingBTCreature>>(
			 new MindBodyBTSpecies<LineFollowingBTCreature>(0,
			 	new TraitGenesSpecies(0, new HashSet<string> {"SPEED"}, 4, new Range<float>(0.25f, 1f), 4, new Range<float>(-0.5f, 0.5f)),
				new BehaviourTreeSpecies<LineFollowingBTCreature>(0, LineFollowingBTCreature.getInputFactorys(), LineFollowingBTCreature.getOutputFactorys(), new Range<float>(0.5f, 1.1f))
			 ), 0.1f, 100, (float p_fitness) => { return p_fitness * 0.95f; }, 1f 
		);

	}
	protected override LineFollowingBTGameController getSelf(){
		return this;
	}

}



