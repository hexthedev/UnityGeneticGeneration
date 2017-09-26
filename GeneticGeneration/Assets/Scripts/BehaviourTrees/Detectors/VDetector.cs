using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VDetector : IDetector, IBehaviourNode
{
	protected BehaviourTree m_tree;
	private IBehaviourNode m_true_child;
	private IBehaviourNode m_false_child;

	public VDetector(IBehaviourNode p_true_child, IBehaviourNode p_false_child){
		m_true_child = p_true_child;
		m_false_child = p_false_child;
	}

	public abstract bool detect();

	public IBehaviourNode act()
	{
		//Debug.Log("got:" + detect());
		
		IBehaviourNode toReturn = detect() ? m_true_child : m_false_child;
		return toReturn == null ? m_tree.getRoot() : toReturn.act();
	}

	public BehaviourTree GetTree()
	{
		return m_tree;
	}

	public void initialize(BehaviourTree p_tree)
	{
		m_tree = p_tree;

		if(m_true_child != null){
			//Debug.Log("INITALIZE TRUE");
			m_true_child.initialize(p_tree);
		}
		
		if(m_false_child != null){
			//Debug.Log("INITALIZE FALSE");
			m_false_child.initialize(p_tree);
		}
	}

}
