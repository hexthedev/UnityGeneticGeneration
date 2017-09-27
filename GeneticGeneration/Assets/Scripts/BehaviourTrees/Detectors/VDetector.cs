using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VDetector : IDetector, IBehaviourNode
{
	protected BehaviourTree m_tree;
	protected IBehaviourNode m_true_child;
	protected IBehaviourNode m_false_child;

	public VDetector(IBehaviourNode p_true_child, IBehaviourNode p_false_child){
		m_true_child = p_true_child;
		m_false_child = p_false_child;
	}


	//LOADING
	public void load(BehaviourTree p_tree)
	{
		m_tree = p_tree;

		if(m_true_child != null){
			m_true_child.load(p_tree);
		}
		
		if(m_false_child != null){
			m_false_child.load(p_tree);
		}
	}

	public void unload()
	{
		m_tree = null;

		if(m_true_child != null){
			m_true_child.unload();
		}
		
		if(m_false_child != null){
			m_false_child.unload();
		}
	}

	//ACTIONS
	public abstract bool detect();

	public IBehaviourNode act()
	{		
		IBehaviourNode toReturn = detect() ? m_true_child : m_false_child;
		return toReturn == null ? m_tree.getRoot() : toReturn.act();
	}

	//CLONING
  public abstract IBehaviourNode clone();

}
