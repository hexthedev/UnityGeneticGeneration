using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeneticBehaviourTrees
{
  public interface IBehaviourGenoType : IMutatable, IRandomizable
  {
    VTreeNode<IBehaviourNode> phenotype(VTreeNode<IBehaviourNode> p_parent, BehaviourTree p_tree);

    VTreeNode<IBehaviourGenoType> clone(VTreeNode<IBehaviourGenoType> p_parent);
  }

}
