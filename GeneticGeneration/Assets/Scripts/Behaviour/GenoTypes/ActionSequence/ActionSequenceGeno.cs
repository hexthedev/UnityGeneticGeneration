using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSequenceGeno :  VTreeNode<IBehaviourGenoType>, IBehaviourGenoType
{
  
	List<IActionGenoType> m_actions;

  public ActionSequenceGeno(VTreeNode<IBehaviourGenoType> p_parent) : base(p_parent, 1)
  {
		m_actions = new List<IActionGenoType>();
    setSelf(this);
  }

  public void addAction(IActionGenoType p_action){
    m_actions.Add(p_action);
  }

  public void mutate()
  {
    foreach(IBehaviourGenoType action in m_actions){
			action.mutate();
		} 
  }

  public void randomize()
  {
    foreach(IBehaviourGenoType action in m_actions){
			action.randomize();
		} 
  }

  public VTreeNode<IBehaviourNode> phenotype(VTreeNode<IBehaviourNode> p_parent, BehaviourTree p_tree)
  {
   ActionSequence sequence = new ActionSequence(p_tree, p_parent);

   foreach(IActionGenoType action in m_actions){
     sequence.addAction(action);
   }

   return sequence;
  }

}
