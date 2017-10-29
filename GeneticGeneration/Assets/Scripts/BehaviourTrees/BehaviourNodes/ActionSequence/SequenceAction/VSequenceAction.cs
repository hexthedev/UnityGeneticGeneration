using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GeneticBehaviourTrees
{
  public abstract class VSequenceAction : IAction
  {

    protected ActionSequence m_sequence;

    protected VSequenceAction(ActionSequence p_parent)
    {
      m_sequence = p_parent;
    }

    public ActionSequence getSequence()
    {
      return m_sequence;
    }

    public abstract bool performAction();

    public abstract void reset();

  }
}