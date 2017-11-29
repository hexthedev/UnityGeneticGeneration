using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Genetic.Base;
using Genetic.Composite;
using Genetic.Traits.Base;

using JTools.Calc.Vectors;
using JTools.Events;
using JTools.DataStructures.PriorityList;
using JTools.Calc.Lines;
using JTools.Calc.ActiavationFunctions;
using Genetic.Behaviour.DecisionTrees;
using Genetic.Behaviour.NeuralNets;

public class HFLineFollowingNNCreature : AHFLineFollowingCreature<
  HFLineFollowingNNCreature,
  HFLineFollowingNNGameController,
  MindBodyNNDNA<HFLineFollowingNNCreature>,
  MindBodyNN,
  NeuralNet>
{
  protected override HFLineFollowingNNCreature getSelf()
  {
    return this;
  }
}
