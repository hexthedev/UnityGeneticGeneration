using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GeneticBehaviourTrees
{
  public interface IBehaviourNode
  {
    IBehaviourNode act();
  }
}