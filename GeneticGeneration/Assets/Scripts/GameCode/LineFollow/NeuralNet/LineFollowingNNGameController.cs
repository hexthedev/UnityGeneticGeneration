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
  protected override DNABasedEvolutionManager<MindBodyNNDNA<LineFollowingNNCreature>> createManager()
  {
    return new DNABasedEvolutionManager<MindBodyNNDNA<LineFollowingNNCreature>>(
			 new MindBodyNNSpecies<LineFollowingNNCreature>(0,
			 	new TraitGenesSpecies(0, new HashSet<string> {"SPEED"}, 4, new Range<float>(0.25f, 1f), 4, new Range<float>(-0.5f, 0.5f)),
				new NeuralNetSpecies<LineFollowingNNCreature>( 0, LineFollowingNNCreature.getInputFactorys(), LineFollowingNNCreature.getOutputFactorys(), 3, 3, new Range<float>(-1f, 1f), new Range<float>(0.8f, 1.2f) )
			 ), 0.1f, 50, (float p_fitness) => { return p_fitness * 0.95f; }, 1f 
		);
  }

  protected override LineFollowingNNGameController getSelf()
  {
    return this;
  }
}



