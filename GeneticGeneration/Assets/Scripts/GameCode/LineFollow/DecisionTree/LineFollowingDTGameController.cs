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

public class LineFollowingDTGameController : ALineFollowingGameController<
  LineFollowingDTCreature,
  LineFollowingDTGameController,
  MindBodyDTDNA<LineFollowingDTCreature>,
  MindBodyDT,
  DecisionTreeWrapper>
{

  private string AIkey = "DecisionTree";

  protected override void setUpData(){
    if(DataCollector.Open) DataCollector.closeCSV();
    DataCollector.setTitle(AIkey + lineFollowCONFIG.DataSuffix);
    DataCollector.startCSV();
  }

  protected override DNABasedEvolutionManager<MindBodyDTDNA<LineFollowingDTCreature>> createManager()
  {
    return new DNABasedEvolutionManager<MindBodyDTDNA<LineFollowingDTCreature>>(
			 new MindBodySpeciesDT<LineFollowingDTCreature>(0,
			 	new TraitGenesSpecies(0, new HashSet<string> {"SPEED"}, lineFollowCONFIG.TraitGeneSize, lineFollowCONFIG.TraitRange, 4, lineFollowCONFIG.TraitMutationAddition),
				new DecisionTreeSpecies<LineFollowingDTCreature>( 0, LineFollowingDTCreature.getInputFactorys(), LineFollowingDTCreature.getOutputFactorys(), lineFollowCONFIG.BehaviourMutationMultiplier, new Range<float>(0f, 1f), new Range<float>(0.2f, 0.8f)  )
			 ),lineFollowCONFIG.GenMutationChance, lineFollowCONFIG.GenGenepoolSize, (float p_fitness) => { return p_fitness * lineFollowCONFIG.GenDNAAgeingMultiplier; }, lineFollowCONFIG.GenDNAAgeingLatency 
		);
  }

	protected override void setUpAI(){
		if(!(lineFollowCONFIG.AIkey == AIkey)){
			lineFollowCONFIG.loadScene();
		}
	}
  protected override LineFollowingDTGameController getSelf()
  {
    return this;
  }
}



