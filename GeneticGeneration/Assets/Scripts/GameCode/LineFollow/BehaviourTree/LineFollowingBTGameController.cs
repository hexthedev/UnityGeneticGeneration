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

	private string AIkey = "BehaviourTree";

  protected override void setUpData(){
    if(DataCollector.Open) DataCollector.closeCSV();
    DataCollector.setTitle(AIkey + lineFollowCONFIG.DataSuffix);
    DataCollector.startCSV();
  }

	protected override void setUpAI(){
		if(!(lineFollowCONFIG.AIkey == AIkey)){
			lineFollowCONFIG.loadScene();
		}
	}
	
	protected override DNABasedEvolutionManager<MindBodyBTDNA<LineFollowingBTCreature>> createManager(){

		return new DNABasedEvolutionManager<MindBodyBTDNA<LineFollowingBTCreature>>(
			 	new MindBodyBTSpecies<LineFollowingBTCreature>(0,
			 	new TraitGenesSpecies(0, new HashSet<string> {"SPEED"}, lineFollowCONFIG.TraitGeneSize, lineFollowCONFIG.TraitRange, 4, lineFollowCONFIG.TraitMutationAddition),
				new BehaviourTreeSpecies<LineFollowingBTCreature>(0, LineFollowingBTCreature.getInputFactorys(), LineFollowingBTCreature.getOutputFactorys(), lineFollowCONFIG.BehaviourMutationMultiplier)
			 ),lineFollowCONFIG.GenMutationChance, lineFollowCONFIG.GenGenepoolSize, (float p_fitness) => { return p_fitness * lineFollowCONFIG.GenDNAAgeingMultiplier; }, lineFollowCONFIG.GenDNAAgeingLatency 
		);

	}
	protected override LineFollowingBTGameController getSelf(){
		return this;
	}

}



