using System.Collections;
using System.Collections.Generic;

using Genetic.Base;
using Genetic.Behaviour.DecisionTrees;
using Genetic.Behaviour.NeuralNets;
using Genetic.Composite;
using Genetic.Traits.Base;
using Genetic.Traits.TraitGenes;
using JTools.Calc.ActiavationFunctions;
using JTools.Calc.Base;
using JTools.Calc.Lines;
using JTools.Calc.Vectors;
using JTools.Events;
using UnityEngine;

public class HFLineFollowingNNGameController :  AHFLineFollowingGameController<
  HFLineFollowingNNCreature,
  HFLineFollowingNNGameController,
  MindBodyNNDNA<HFLineFollowingNNCreature>,
  MindBodyNN,
  NeuralNet>
{

  private string AIkey = "NeuralNet";

  protected override void setUpData(){
    if(DataCollector.Open) DataCollector.closeCSV();
    DataCollector.setTitle(AIkey + HFlineFollowCONFIG.DataSuffix);
    DataCollector.startCSV();
  }

  protected override DNABasedEvolutionManager<MindBodyNNDNA<HFLineFollowingNNCreature>> createManager()
  {
    return new DNABasedEvolutionManager<MindBodyNNDNA<HFLineFollowingNNCreature>>(
			 new MindBodyNNSpecies<HFLineFollowingNNCreature>(0,
			 	new TraitGenesSpecies(0, new HashSet<string> {"SPEED", "HEALTH"}, HFlineFollowCONFIG.TraitGeneSize, HFlineFollowCONFIG.TraitRange, 4, HFlineFollowCONFIG.TraitMutationAddition),
				new NeuralNetSpecies<HFLineFollowingNNCreature>( 0, HFLineFollowingNNCreature.getInputFactorys(), HFLineFollowingNNCreature.getOutputFactorys(), 3, 3, new Range<float>(-1f, 1f), HFlineFollowCONFIG.BehaviourMutationMultiplier )
			 ),HFlineFollowCONFIG.GenMutationChance, HFlineFollowCONFIG.GenGenepoolSize, (float p_fitness) => { return p_fitness * HFlineFollowCONFIG.GenDNAAgeingMultiplier; }, HFlineFollowCONFIG.GenDNAAgeingLatency 
		);
  }


	protected override void setUpAI(){
		Debug.Log("SET UP AI");
    Debug.Log(HFlineFollowCONFIG.AIkey);
    Debug.Log(AIkey);
    Debug.Log(HFlineFollowCONFIG.AIkey == AIkey);
    

    if(!(HFlineFollowCONFIG.AIkey == AIkey)){
			Debug.Log(HFlineFollowCONFIG.AIkey);
      Debug.Log(AIkey);

      HFlineFollowCONFIG.loadScene();
		}
	}
  protected override HFLineFollowingNNGameController getSelf()
  {
    return this;
  }
}



