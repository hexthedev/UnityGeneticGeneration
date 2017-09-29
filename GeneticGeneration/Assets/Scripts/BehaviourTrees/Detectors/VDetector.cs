using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VDetector : IDetector, IBehaviourNode
{
	protected BehaviourTree m_tree;
	protected IBehaviourNode m_parent;
	protected IBehaviourNode m_true_child;
	protected IBehaviourNode m_false_child;

	public VDetector(IBehaviourNode p_true_child, IBehaviourNode p_false_child){
		m_true_child = p_true_child;
		m_false_child = p_false_child;
	}

	public void setParent(IBehaviourNode p_parent){
		m_parent = p_parent;
		if(m_true_child != null) { m_true_child.setParent(this); }
		if(m_false_child != null) { m_false_child.setParent(this); }
	}

	public IBehaviourNode returnToRoot(){
		return m_parent == null ? this : m_parent.returnToRoot();
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
		//m_tree = null;

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
		if(m_tree == null){
			Debug.LogError("VDetector cannot act, unloaded");
			return null;
		}

		IBehaviourNode toReturn = detect() ? m_true_child : m_false_child;
		return toReturn == null ? returnToRoot() : toReturn.act();
	}

	//CLONING
  public abstract IBehaviourNode clone();

  //EVOLUTION
  public IBehaviourNode getRandomNode()
  {
    int rand = Random.Range(0,2);

		if(rand == 1){
			if(m_true_child == null) { return returnToRoot().getRandomNode(); }
			return Random.Range(0,2) == 1 ? m_true_child : m_true_child.getRandomNode();		
		} else {
			if(m_false_child == null) { return returnToRoot().getRandomNode(); }
			return Random.Range(0,2) == 1 ? m_false_child : m_false_child.getRandomNode();		
		}
  }

  public void insertAtRandom(IBehaviourNode p_node)
  {
    int rand = Random.Range(0,2);

		if(rand == 1){
			if(Random.Range(0,2) == 1){ m_true_child = p_node; } else { m_false_child = p_node; }
		} else {
			if(Random.Range(0,2) == 1){ 
				if(m_true_child == null){ returnToRoot().insertAtRandom(p_node); } else { m_true_child.insertAtRandom(p_node); }
			} else { 
				if(m_false_child == null){ returnToRoot().insertAtRandom(p_node); } else { m_false_child.insertAtRandom(p_node); }
			}
		}
  }
}
