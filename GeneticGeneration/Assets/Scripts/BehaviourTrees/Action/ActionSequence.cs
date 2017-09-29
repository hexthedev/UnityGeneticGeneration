using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSequence : IBehaviourNode{


	BehaviourTree m_tree;
	IBehaviourNode m_parent;

	IBehaviourNode m_child;
	IAction[] m_actions;

	Stack<IAction> m_sequence;

	public ActionSequence(IAction[] p_actions, IBehaviourNode p_child){
		m_actions = new IAction[p_actions.Length];
		p_actions.CopyTo(m_actions, 0);
		m_child = p_child;
	}

	public void setParent(IBehaviourNode p_parent){
		m_parent = p_parent;
		if(m_child != null) { m_child.setParent(this); }
	}

	public IBehaviourNode returnToRoot(){
		return m_parent == null ? this : m_parent.returnToRoot(); 
	}

	//Resets the action sequence
	public void reset(){		
		foreach(IAction action in m_actions){
			action.reset();
		}

		loadSequence();
	}

	//Does an action for a frame and then return if action is complete
	public bool performAction(){
		if(!m_tree.isLoaded()){
			Debug.LogError("CANNOT HAPPEN: ACTION SEQUENCE NOT LOADED");
			return true;
		}

		if(m_sequence.Count == 0){
			return true;
		}

		if(m_sequence.Peek().act()){
			m_sequence.Pop();
		}

		return m_sequence.Count == 0;
	}

  public IBehaviourNode act()
  {
		if(m_tree == null){
			Debug.LogError("ActionSequence cannot act, unloaded");
			return null;
		}

		if(performAction()){
			
			reset();

			if(m_child != null){
				return m_child.act();
			} else {
				return returnToRoot(); 
			}
			
		} else {
			return this;
		}
  }


	//LOADING
  public void load(BehaviourTree p_tree)
  {
		
		m_tree = p_tree;

		if(m_child != null){
			m_child.load(p_tree);
		}

		foreach(IAction action in m_actions){
			action.load(p_tree);
		}

		loadSequence();
		
	}

	public void unload()
  {
		
		//m_tree = null;

		if(m_child != null){
			m_child.unload();
		}

		foreach(IAction action in m_actions){
			action.unload();
		}

		m_sequence = null;
	}


	//RANDOM
	public static ActionSequence random(){

		List<IAction> actions = new List<IAction>();
		actions.Add(RandomGen.IAction());

		int rand = Random.Range(0,2);

		while(rand != 0){
			actions.Add(RandomGen.IAction());
			rand = Random.Range(0,2);
		}

		return new ActionSequence(actions.ToArray(), RandomGen.IBehaviourNode());

	}

	//HELPER
	private void loadSequence(){
		m_sequence = new Stack<IAction>();
		Stack<IAction> sequence = new Stack<IAction>(m_actions);

		while(sequence.Count != 0){
			m_sequence.Push(sequence.Pop());
		}
	}

	//CLONING
  public IBehaviourNode clone()
  {
    if(m_child != null){
			return new ActionSequence(cloneActions(), m_child.clone());
		}

		return new ActionSequence(cloneActions(), null);		
  }

	private IAction[] cloneActions(){
		List<IAction> actions = new List<IAction>();

		foreach(IAction action in m_actions){
			actions.Add(action.clone());
		}

		return actions.ToArray();
	}

	//EVOLUTION
  public IBehaviourNode getRandomNode()
  {
    if(m_child == null){
			return Random.Range(0,2) == 1 ? returnToRoot() : returnToRoot().getRandomNode();
		} else {
			return Random.Range(0,2) == 1 ? m_child : m_child.getRandomNode();
		}
  }

  public void insertAtRandom(IBehaviourNode p_node)
  {
    if(Random.Range(0,2) == 1){
			m_child = p_node;
		} else{
			if(m_child == null) { returnToRoot().insertAtRandom(p_node); } else { m_child.insertAtRandom(p_node); };
		}
  }






}
