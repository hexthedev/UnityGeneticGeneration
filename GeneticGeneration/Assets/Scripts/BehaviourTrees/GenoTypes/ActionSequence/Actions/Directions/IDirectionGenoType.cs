using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GeneticBehaviourTrees
{
  public interface IDirectionGenoType : IMutatable, IRandomizable
  {
    IDirection phenotype(VSequenceAction p_parent);

    IDirectionGenoType clone();
  }
}