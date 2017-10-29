using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeneticBehaviourTrees
{
  public class ActionSequenceGeno : VTreeNode<IBehaviourGenoType>, IBehaviourGenoType
  {

    List<IActionGenoType> m_actions;

    public ActionSequenceGeno(VTreeNode<IBehaviourGenoType> p_parent) : base(p_parent, 1)
    {
      m_actions = new List<IActionGenoType>();
      setSelf(this);
    }

    public void addAction(IActionGenoType p_action)
    {
      m_actions.Add(p_action);
    }

    public void mutate()
    {
      foreach (IActionGenoType action in m_actions)
      {
        action.mutate();
      }

      VTreeNode<IBehaviourGenoType> child = getRandomChild();

      if (child != null)
      {
        child.getSelf().mutate();
      }

    }

    public void randomize()
    {
      foreach (IBehaviourGenoType action in m_actions)
      {
        action.randomize();
      }
    }

    public VTreeNode<IBehaviourNode> phenotype(VTreeNode<IBehaviourNode> p_parent, BehaviourTree p_tree)
    {
      ActionSequence sequence = new ActionSequence(p_tree, p_parent);

      foreach (IActionGenoType action in m_actions)
      {
        sequence.addAction(action);
      }

      return sequence;
    }

    public VTreeNode<IBehaviourGenoType> clone(VTreeNode<IBehaviourGenoType> p_parent)
    {
      ActionSequenceGeno sequence = new ActionSequenceGeno(p_parent);

      foreach (IActionGenoType action in m_actions)
      {
        sequence.addAction(action.clone(sequence));
      }

      if (existsChild(0))
      {
        sequence.addChild(getChild(0).getSelf().clone(sequence), 0);
      }
      else
      {
        sequence.addChild(null, 0);
      }

      return sequence;
    }
  }
}