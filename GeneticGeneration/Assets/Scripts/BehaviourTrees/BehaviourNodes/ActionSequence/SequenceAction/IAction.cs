using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeneticBehaviourTrees
{
  public interface IAction
  {
    bool performAction();
    void reset();
  }
}