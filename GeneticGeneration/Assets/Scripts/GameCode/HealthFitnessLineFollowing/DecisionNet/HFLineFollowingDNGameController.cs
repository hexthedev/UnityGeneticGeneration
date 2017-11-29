using System.Collections;
using System.Collections.Generic;

using Genetic.Base;
using Genetic.Behaviour.DecisionNets;
using Genetic.Composite;
using Genetic.Traits.Base;
using Genetic.Traits.TraitGenes;
using JTools.Calc.ActiavationFunctions;
using JTools.Calc.Base;
using JTools.Calc.Lines;
using JTools.Calc.Vectors;
using JTools.Events;
using UnityEngine;

public class HFLineFollowingDNGameController : AHFLineFollowingGameController<
  HFLineFollowingDNCreature,
  HFLineFollowingDNGameController,
  MindBodyDNDNA<HFLineFollowingDNCreature>,
  MindBodyDN,
  DecisionNet>
{

	private string AIkey = "DecisionNet";

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
  protected override DNABasedEvolutionManager<MindBodyDNDNA<HFLineFollowingDNCreature>> createManager()
  {
    return new DNABasedEvolutionManager<MindBodyDNDNA<HFLineFollowingDNCreature>>(
			 new MindBodySpeciesDN<HFLineFollowingDNCreature>(0,
			 	new TraitGenesSpecies(00, new HashSet<string> {"SPEED", "HEALTH"}, HFlineFollowCONFIG.TraitGeneSize, HFlineFollowCONFIG.TraitRange, 4, HFlineFollowCONFIG.TraitMutationAddition),
				new DecisionNetSpecies<HFLineFollowingDNCreature>( 0, HFLineFollowingDNCreature.getInputFactorys(), HFLineFollowingDNCreature.getOutputFactorys(), HFlineFollowCONFIG.BehaviourMutationMultiplier )
			 ),HFlineFollowCONFIG.GenMutationChance, HFlineFollowCONFIG.GenGenepoolSize, (float p_fitness) => { return p_fitness * HFlineFollowCONFIG.GenDNAAgeingMultiplier; }, HFlineFollowCONFIG.GenDNAAgeingLatency 
		);
  }

  protected override HFLineFollowingDNGameController getSelf()
  {
    return this;
  }

}



