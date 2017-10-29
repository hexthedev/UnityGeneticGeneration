using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

namespace GeneticBehaviourTrees
{
  public class InternalDetectorGenoType : VTreeNode<IBehaviourGenoType>, IBehaviourGenoType
  {

    private ETrait m_trait;

    //This means is current trait or is total trait;
    private bool m_is_current;

    private float m_threshold;

    private bool m_is_above;

    public InternalDetectorGenoType(ETrait p_trait, bool p_is_current, float p_threshold, bool p_is_above, VTreeNode<IBehaviourGenoType> p_parent) : base(p_parent, 2)
    {
      m_trait = p_trait;
      m_is_current = p_is_current;
      m_threshold = p_threshold;
      m_is_above = p_is_above;
      setSelf(this);
    }

    public void mutate()
    {
      // m_trait = EnumCalc.randomValue<ETrait>();
      // m_is_current = BoolCalc.random();
      // m_threshold = FloatCalc.mutate(m_threshold, 0, 10, EvolutionVars.det_trait_threshold_rand_mult());
      // m_is_above = BoolCalc.random();

      VTreeNode<IBehaviourGenoType> child = getRandomChild();

      if (child != null)
      {
        child.getSelf().mutate();
      }
    }

    public VTreeNode<IBehaviourNode> phenotype(VTreeNode<IBehaviourNode> p_parent, BehaviourTree p_tree)
    {
      return new InternalDetector(m_trait, m_is_current, m_threshold, m_is_above, p_tree, p_parent);
    }

    public void randomize()
    {
      // m_trait = EnumCalc.randomValue<ETrait>();
      // m_is_current = BoolCalc.random();
      // m_threshold = EvolutionVars.det_trait_threshold_rand_val();
      // m_is_above = BoolCalc.random();
    }

    public VTreeNode<IBehaviourGenoType> clone(VTreeNode<IBehaviourGenoType> p_parent)
    {
      VTreeNode<IBehaviourGenoType> copy = new InternalDetectorGenoType(m_trait, m_is_current, m_threshold, m_is_above, p_parent);

      for (int i = 0; i < copy.numChildren(); i++)
      {
        if (existsChild(i))
        {
          copy.addChild(getChild(i).getSelf().clone(copy), i);
        }
        else
        {
          copy.addChild(null, i);
        }
      }

      return copy;
    }
  }
}