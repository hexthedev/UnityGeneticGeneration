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
  protected override DNABasedEvolutionManager<MindBodyDTDNA<LineFollowingDTCreature>> createManager()
  {
    return new DNABasedEvolutionManager<MindBodyDTDNA<LineFollowingDTCreature>>(
			 new MindBodySpeciesDT<LineFollowingDTCreature>(0,
			 	new TraitGenesSpecies(0, new HashSet<string> {"SPEED"}, 4, new Range<float>(0.25f, 1f), 4, new Range<float>(-0.5f, 0.5f)),
				new DecisionTreeSpecies<LineFollowingDTCreature>( 0, LineFollowingDTCreature.getInputFactorys(), LineFollowingDTCreature.getOutputFactorys(), new Range<float>(0.8f, 1.2f), new Range<float>(0f, 1f), new Range<float>(0.2f, 0.8f)  )
			 ), 0.1f, 50, (float p_fitness) => { return p_fitness * 0.95f; }, 1f 
		);
  }

  protected override LineFollowingDTGameController getSelf()
  {
    return this;
  }
}



