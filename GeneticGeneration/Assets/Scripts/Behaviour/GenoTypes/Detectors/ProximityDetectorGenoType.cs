using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class ProximityDetectorGenoType : VTreeNode<IBehaviourGenoType>, IBehaviourGenoType {

  private EObjectTypes m_of;
	private float m_threshold;
	private int m_count;

	public ProximityDetectorGenoType(EObjectTypes p_of, float p_threshold, int p_count, VTreeNode<IBehaviourGenoType> p_parent) : base(p_parent, 2){
		m_of = p_of;
		m_count = p_count;
		m_threshold = p_threshold;
    setSelf(this);
	}

  public void mutate()
  {
    m_of = EnumCalc.randomValue<EObjectTypes>();
    m_threshold = FloatCalc.mutate(m_threshold, 0.1f, 8f);
    m_count = IntCalc.mutate(m_count, 1, 5);

    VTreeNode<IBehaviourGenoType> child = getRandomChild();

    if(child != null){
      child.getSelf().mutate();
    }
  }

  public VTreeNode<IBehaviourNode> phenotype(VTreeNode<IBehaviourNode> p_parent, BehaviourTree p_tree)
  {
    return new ProximityDetector(m_of, m_threshold, m_count, p_tree, p_parent);
  }

  public void randomize()
  {
    m_of = EnumCalc.randomValue<EObjectTypes>();
    m_threshold = Random.Range(0.1f, 8f);
    m_count = Random.Range(1, 5);
  }

  public VTreeNode<IBehaviourGenoType> clone(VTreeNode<IBehaviourGenoType> p_parent)
  {
    VTreeNode<IBehaviourGenoType> copy = new ProximityDetectorGenoType(m_of, m_threshold, m_count, p_parent);

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
