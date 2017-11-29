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

public class HFLineFollowingBTGameController : AHFLineFollowingGameController<
	HFLineFollowingBTCreature, 
	HFLineFollowingBTGameController, 
	MindBodyBTDNA<HFLineFollowingBTCreature>, 
	MindBodyBT, 
	BehaviourTree>{

	private string AIkey = "BehaviourTree";

  protected override void setUpData(){
    if(DataCollector.Open) DataCollector.closeCSV();
    DataCollector.setTitle(AIkey + HFlineFollowCONFIG.DataSuffix);
    DataCollector.startCSV();
  }

	protected override void setUpAI(){
		if(!(HFlineFollowCONFIG.AIkey == AIkey)){
			HFlineFollowCONFIG.loadScene();
		}
	}
	
	protected override DNABasedEvolutionManager<MindBodyBTDNA<HFLineFollowingBTCreature>> createManager(){

		return new DNABasedEvolutionManager<MindBodyBTDNA<HFLineFollowingBTCreature>>(
			 	new MindBodyBTSpecies<HFLineFollowingBTCreature>(0,
			 	new TraitGenesSpecies(0, new HashSet<string> {"SPEED", "HEALTH"}, HFlineFollowCONFIG.TraitGeneSize, HFlineFollowCONFIG.TraitRange, 4, HFlineFollowCONFIG.TraitMutationAddition),
				new BehaviourTreeSpecies<HFLineFollowingBTCreature>(0, HFLineFollowingBTCreature.getInputFactorys(), HFLineFollowingBTCreature.getOutputFactorys(), HFlineFollowCONFIG.BehaviourMutationMultiplier)
			 ),HFlineFollowCONFIG.GenMutationChance, HFlineFollowCONFIG.GenGenepoolSize, (float p_fitness) => { return p_fitness * HFlineFollowCONFIG.GenDNAAgeingMultiplier; }, HFlineFollowCONFIG.GenDNAAgeingLatency 
		);

	}
	protected override HFLineFollowingBTGameController getSelf(){
		return this;
	}

}



