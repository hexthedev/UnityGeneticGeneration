using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GeneticBehaviourTrees
{
  public abstract class VDetector : VTreeNode<IBehaviourNode>, IDetector, IBehaviourNode
  {
    protected BehaviourTree m_tree;

    public VDetector(BehaviourTree p_tree, VTreeNode<IBehaviourNode> p_parent) : base(p_parent, 2)
    { //True child = index 1, false 0
      m_tree = p_tree;
      setSelf(this);
    }

    //ACTIONS
    public abstract bool detect();

    public IBehaviourNode act()
    {
      int returnIndex = detect() ? 1 : 0;
      return existsChild(returnIndex) ? getChild(returnIndex).getSelf() : getRoot();
    }
  }
}