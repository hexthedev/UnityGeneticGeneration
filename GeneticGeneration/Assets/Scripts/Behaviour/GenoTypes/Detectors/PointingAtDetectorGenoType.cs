using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class PointingAtDetectorGenoType : VTreeNode<IBehaviourGenoType>, IBehaviourGenoType {

	float m_angle_threshold;
	EObjectTypes m_pointing_at;
	int m_count;

  public PointingAtDetectorGenoType(EObjectTypes p_pointing_at, float p_angle_threshold, int p_count, VTreeNode<IBehaviourGenoType> p_parent) : base(p_parent, 2)
  {
		m_pointing_at = p_pointing_at;
		m_angle_threshold = p_angle_threshold;
		m_count = p_count;
    setSelf(this);
  }

  public void mutate()
  {
    m_angle_threshold = FloatCalc.mutate(m_angle_threshold, 0f, 108f);
    m_pointing_at = EnumCalc.randomValue<EObjectTypes>();
    m_count = IntCalc.mutate(m_count, 1, 5);
  }

  public VTreeNode<IBehaviourNode> phenotype(VTreeNode<IBehaviourNode> p_parent, BehaviourTree p_tree)
  {
    return new PointingAtDetector(m_pointing_at, m_angle_threshold, m_count, p_tree, p_parent);
  }

  public void randomize()
  {
    m_angle_threshold = Random.Range(0f, 180f);
    m_pointing_at = EnumCalc.randomValue<EObjectTypes>();
    m_count = Random.Range(1, 5);
  }

  public VTreeNode<IBehaviourGenoType> clone(VTreeNode<IBehaviourGenoType> p_parent)
  {
    VTreeNode<IBehaviourGenoType> copy = new PointingAtDetectorGenoType(m_pointing_at, m_angle_threshold, m_count, p_parent);

    for(int i = 0; i<copy.numChildren(); i++){
      if(existsChild(i)){ 
        copy.addChild(getChild(i).getSelf().clone(copy) , i);
      } else {
        copy.addChild(null, i);
      }
    }

    return copy;
  }
}
