using System.Collections;
using System.Collections.Generic;

using Genetic.Base;
using Genetic.Behaviour.DecisionTrees;
using Genetic.Composite;
using Genetic.Traits.Base;
using Genetic.Traits.TraitGenes;
using JTools.Calc.ActiavationFunctions;
using JTools.Calc.Base;
using JTools.Calc.Lines;
using JTools.Calc.Vectors;
using JTools.Events;
using UnityEngine;

public class HFLineFollowingDTGameController : AHFLineFollowingGameController<
  HFLineFollowingDTCreature,
  HFLineFollowingDTGameController,
  MindBodyDTDNA<HFLineFollowingDTCreature>,
  MindBodyDT,
  DecisionTreeWrapper>
{

  private string AIkey = "DecisionTree";

  protected override void setUpData(){
    if(DataCollector.Open) DataCollector.closeCSV();
    DataCollector.setTitle(AIkey + HFlineFollowCONFIG.DataSuffix);
    DataCollector.startCSV();
  }

  protected override DNABasedEvolutionManager<MindBodyDTDNA<HFLineFollowingDTCreature>> createManager()
  {
    return new DNABasedEvolutionManager<MindBodyDTDNA<HFLineFollowingDTCreature>>(
			 new MindBodySpeciesDT<HFLineFollowingDTCreature>(0,
			 	new TraitGenesSpecies(0, new HashSet<string> {"SPEED", "HEALTH"}, HFlineFollowCONFIG.TraitGeneSize, HFlineFollowCONFIG.TraitRange, 4, HFlineFollowCONFIG.TraitMutationAddition),
				new DecisionTreeSpecies<HFLineFollowingDTCreature>( 0, HFLineFollowingDTCreature.getInputFactorys(), HFLineFollowingDTCreature.getOutputFactorys(), HFlineFollowCONFIG.BehaviourMutationMultiplier, new Range<float>(0f, 1f), new Range<float>(0.2f, 0.8f)  )
			 ),HFlineFollowCONFIG.GenMutationChance, HFlineFollowCONFIG.GenGenepoolSize, (float p_fitness) => { return p_fitness * HFlineFollowCONFIG.GenDNAAgeingMultiplier; }, HFlineFollowCONFIG.GenDNAAgeingLatency 
		);
  }

	protected override void setUpAI(){
    if(!(HFlineFollowCONFIG.AIkey == AIkey)){
      HFlineFollowCONFIG.loadScene();
		}
	}
  protected override HFLineFollowingDTGameController getSelf()
  {
    return this;
  }
}



