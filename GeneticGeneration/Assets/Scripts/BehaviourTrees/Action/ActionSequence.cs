using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSequence : IBehaviourNode{

	BehaviourTree m_tree;
	IBehaviourNode m_child;
	IAction[] m_actions;

	Stack<IAction> m_sequence;

	public ActionSequence(IAction[] p_actions, IBehaviourNode p_child){
		m_actions = new IAction[p_actions.Length];
		p_actions.CopyTo(m_actions, 0);
		
		loadSequence();

		m_child = p_child;
	}

	public void reset(){		
		foreach(IAction action in m_actions){
			action.reset();
		}

		loadSequence();
	}

	public bool performAction(){
		if(m_sequence.Count == 0){
			return true;
		}

		//Debug.Log(m_tree);

		if(m_sequence.Peek().act(m_tree.getActor())){
			m_sequence.Pop();
		}

		return m_sequence.Count == 0;
	}


	private void loadSequence(){
		m_sequence = new Stack<IAction>();
		Stack<IAction> sequence = new Stack<IAction>(m_actions);

		while(sequence.Count != 0){
			m_sequence.Push(sequence.Pop());
		}
	}

  public IBehaviourNode act()
  {
    if(performAction()){
			
			reset();

			if(m_child != null){
				return m_child.act();
			} else {
				return m_tree.getRoot(); 
			}
			
		} else {
			return this;
		}
  }


  public BehaviourTree GetTree()
  {
    return m_tree;
  }

  public void initialize(BehaviourTree p_tree)
  {
		
		m_tree = p_tree;

		//Debug.Log("Get Initiated");

		//Debug.Log(m_tree);

		if(m_child != null){
			m_child.initialize(p_tree);
		}
		
	}

}
