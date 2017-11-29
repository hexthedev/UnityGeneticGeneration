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

public class LineFollowingDNGameController : ALineFollowingGameController<
  LineFollowingDNCreature,
  LineFollowingDNGameController,
  MindBodyDNDNA<LineFollowingDNCreature>,
  MindBodyDN,
  DecisionNet>
{

	private string AIkey = "DecisionNet";

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
  protected override DNABasedEvolutionManager<MindBodyDNDNA<LineFollowingDNCreature>> createManager()
  {
    return new DNABasedEvolutionManager<MindBodyDNDNA<LineFollowingDNCreature>>(
			 new MindBodySpeciesDN<LineFollowingDNCreature>(0,
			 	new TraitGenesSpecies(00, new HashSet<string> {"SPEED"}, lineFollowCONFIG.TraitGeneSize, lineFollowCONFIG.TraitRange, 4, lineFollowCONFIG.TraitMutationAddition),
				new DecisionNetSpecies<LineFollowingDNCreature>( 0, LineFollowingDNCreature.getInputFactorys(), LineFollowingDNCreature.getOutputFactorys(), lineFollowCONFIG.BehaviourMutationMultiplier )
			 ),lineFollowCONFIG.GenMutationChance, lineFollowCONFIG.GenGenepoolSize, (float p_fitness) => { return p_fitness * lineFollowCONFIG.GenDNAAgeingMultiplier; }, lineFollowCONFIG.GenDNAAgeingLatency 
		);
  }

  protected override LineFollowingDNGameController getSelf()
  {
    return this;
  }

}



