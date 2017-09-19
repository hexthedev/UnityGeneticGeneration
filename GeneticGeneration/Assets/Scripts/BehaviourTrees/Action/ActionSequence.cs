using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSequence {

	IAction[] m_actions;

	Stack<IAction> m_sequence;

	public ActionSequence(IAction[] p_actions){
		m_actions = new IAction[p_actions.Length];
		p_actions.CopyTo(m_actions, 0);
		
		loadSequence();
	}

	public void reset(){		
		foreach(IAction action in m_actions){
			action.reset();
		}

		loadSequence();
	}

	public bool act(GameObject p_actor){
		if(m_sequence.Count == 0){
			return true;
		}

		if(m_sequence.Peek().act(p_actor)){
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


	//STATIC METHODS
	public static ActionSequence emptySequence(){
		return new ActionSequence(new IAction[0]);
	} 


}
