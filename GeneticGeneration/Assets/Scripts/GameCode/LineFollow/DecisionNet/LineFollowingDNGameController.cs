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

  protected override DNABasedEvolutionManager<MindBodyDNDNA<LineFollowingDNCreature>> createManager()
  {
    return new DNABasedEvolutionManager<MindBodyDNDNA<LineFollowingDNCreature>>(
			 new MindBodySpeciesDN<LineFollowingDNCreature>(0,
			 	new TraitGenesSpecies(0, new HashSet<string> {"SPEED"}, 4, new Range<float>(0.25f, 1f), 4, new Range<float>(-0.5f, 0.5f)),
				new DecisionNetSpecies<LineFollowingDNCreature>( 0, LineFollowingDNCreature.getInputFactorys(), LineFollowingDNCreature.getOutputFactorys(), new Range<float>(0.8f, 1.2f) )
			 ), 0.1f, 50, (float p_fitness) => { return p_fitness * 0.95f; }, 1f 
		);
  }

  protected override LineFollowingDNGameController getSelf()
  {
    return this;
  }

}



