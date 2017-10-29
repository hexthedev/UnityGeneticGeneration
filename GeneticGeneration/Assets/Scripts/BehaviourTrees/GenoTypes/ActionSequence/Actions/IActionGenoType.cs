using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeneticBehaviourTrees
{
  public interface IActionGenoType : IMutatable, IRandomizable
  {
    IAction phenotype(ActionSequence p_parent);

    IActionGenoType clone(ActionSequenceGeno p_parent);
  }
}