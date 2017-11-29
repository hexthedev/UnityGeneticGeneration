using System.Collections;
using System.Collections.Generic;

using Genetic.Base;
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

public class LineFollowingNNGameController : ALineFollowingGameController<
  LineFollowingNNCreature,
  LineFollowingNNGameController,
  MindBodyNNDNA<LineFollowingNNCreature>,
  MindBodyNN,
  NeuralNet>
{
	private string AIkey = "NeuralNet";

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

  protected override DNABasedEvolutionManager<MindBodyNNDNA<LineFollowingNNCreature>> createManager()
  {


    return new DNABasedEvolutionManager<MindBodyNNDNA<LineFollowingNNCreature>>(
			 new MindBodyNNSpecies<LineFollowingNNCreature>(0,
			 	new TraitGenesSpecies(0, new HashSet<string> {"SPEED"}, lineFollowCONFIG.TraitGeneSize, lineFollowCONFIG.TraitRange, 4, lineFollowCONFIG.TraitMutationAddition),
				new NeuralNetSpecies<LineFollowingNNCreature>( 0, LineFollowingNNCreature.getInputFactorys(), LineFollowingNNCreature.getOutputFactorys(), 3, 3, new Range<float>(-1f, 1f), lineFollowCONFIG.BehaviourMutationMultiplier )
			 ),lineFollowCONFIG.GenMutationChance, lineFollowCONFIG.GenGenepoolSize, (float p_fitness) => { return p_fitness * lineFollowCONFIG.GenDNAAgeingMultiplier; }, lineFollowCONFIG.GenDNAAgeingLatency 
		);
  }

  protected override LineFollowingNNGameController getSelf()
  {
    return this;
  }
}



