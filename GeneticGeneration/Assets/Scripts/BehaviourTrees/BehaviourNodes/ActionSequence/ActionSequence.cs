using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSequence : VTreeNode<IBehaviourNode>, IBehaviourNode{

	private BehaviourTree m_tree;
	private List<IAction> m_actions;
	private Stack<IAction> m_sequence;

	public ActionSequence(BehaviourTree p_tree, VTreeNode<IBehaviourNode> p_parent):base(p_parent, 1){
		m_tree = p_tree;
		setSelf(this);
		m_actions = new List<IAction>();
	}

	public BehaviourTree getTree(){
		return m_tree;
	}

	//Resets the action sequence
	private void reset(){		
		foreach(IAction action in m_actions){
			action.reset();
		}

		loadSequence();
	}

	//Does an action for a frame and then return if action is complete
	private bool performAction(){

		if(m_sequence.Count == 0){
			return true;
		}

		if(m_sequence.Peek().performAction()){
			m_sequence.Pop();
		}

		return m_sequence.Count == 0;
	}

  public IBehaviourNode act()
  {
		if(performAction()){
			reset();
			
			if(existsChild(0)){
				return getChild(0).getSelf().act();
			} else {
				return getRoot(); 
			}
			
		} else {
			return this;
		}
  }

	private void loadSequence(){
		m_sequence = new Stack<IAction>();

		for(int i = m_actions.Count-1; i>= 0 ; i--){
			m_sequence.Push(m_actions[i]);
		}
	}

	public void addAction(IActionGenoType action){
		m_actions.Add(action.phenotype(this));
		loadSequence();
	}
}
