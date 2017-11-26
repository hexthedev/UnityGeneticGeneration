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

public class LineFollowingDTCreature : ALineFollowingCreature<
  LineFollowingDTCreature,
  LineFollowingDTGameController,
  MindBodyDTDNA<LineFollowingDTCreature>,
  MindBodyDT,
  DecisionTreeWrapper>
{
  protected override LineFollowingDTCreature getSelf()
  {
    return this;
  }
}
