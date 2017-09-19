using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSequence {

	IAction[] m_actions;

	Stack<IAction> m_sequence;

	public ActionSequence(IAction[] p_actions){
		m_actions = new IAction[p_actions.Length];
		p_actions.CopyTo(m_actions, 0);
		
		m_sequence = new Stack<IAction>(p_actions);
	}

	public void reset(){
		m_sequence = new Stack<IAction>(m_actions);
		
		foreach(IAction action in m_actions){
			action.reset();
		}
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


	//STATIC METHODS
	public static ActionSequence emptySequence(){
		return new ActionSequence(new IAction[0]);
	} 


}
