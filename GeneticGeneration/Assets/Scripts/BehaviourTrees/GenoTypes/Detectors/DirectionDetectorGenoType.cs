using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class DirectionDetectorGenoType : VTreeNode<IBehaviourGenoType>, IBehaviourGenoType
{
	private EObjectTypes m_of;
	private Vector2 m_direction;
	private float m_angle_threshold;
	private int m_count;

  public DirectionDetectorGenoType(EObjectTypes p_of, Vector2 p_direction, float p_angle_threshold, int p_count, VTreeNode<IBehaviourGenoType> p_parent) : base(p_parent, 2)
  {
    m_of = p_of;
		m_direction = p_direction;
		m_angle_threshold = p_angle_threshold;
		m_count = p_count;
    setSelf(this);
  }

  public void mutate()
  {
    m_of = EnumCalc.randomValue<EObjectTypes>();
    m_direction = VectorCalc.mutateVector2(m_direction);
    m_angle_threshold = FloatCalc.mutate(m_angle_threshold, 0, 180f, EvolutionVars.det_angle_rand_mult());
    m_count = IntCalc.mutate(m_count, 1, 7, EvolutionVars.det_count_rand_mult());

    VTreeNode<IBehaviourGenoType> child = getRandomChild();

    if(child != null){
      child.getSelf().mutate();
    }
  }

  public VTreeNode<IBehaviourNode> phenotype(VTreeNode<IBehaviourNode> p_parent, BehaviourTree p_tree)
  {
    return new DirectionDetector(m_of, m_direction, m_angle_threshold, m_count, p_tree, p_parent);
  }

  public void randomize()
  {
    m_of = EnumCalc.randomValue<EObjectTypes>();
    m_direction = VectorCalc.randomDirection();
    m_angle_threshold = Random.Range(0f, 180f);
    m_count = EvolutionVars.det_count_rand_val();
  }

  public VTreeNode<IBehaviourGenoType> clone(VTreeNode<IBehaviourGenoType> p_parent)
  {
    VTreeNode<IBehaviourGenoType> copy = new DirectionDetectorGenoType(m_of, VectorCalc.clone(m_direction), m_angle_threshold, m_count, p_parent);

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
