using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class VTreeNode<T> {

	private VTreeNode<T> m_parent;
	private T m_self;
	private VTreeNode<T>[] m_children;

	protected VTreeNode(VTreeNode<T> p_parent, int p_children){
		m_parent = p_parent;
		m_children = new VTreeNode<T>[p_children];
	}

	protected void setSelf(T p_self){
		m_self = p_self;
	}

	public T getSelf(){
		return m_self;
	}

	protected T getRoot(){
		if(m_parent == null){
			return m_self;
		} else {
			return m_parent.getRoot();
		}
	}

	protected bool existsChild(int p_index){
		return m_children[p_index] != null;
	}

	public int numChildren(){
		return m_children.Length;
	}

	public VTreeNode<T> getChild(int p_index){
		return m_children[p_index];
	}

	public void addChild(T p_child, int p_index, int p_children){
		VTreeNode<T> to_add = new VTreeNode<T>(this, p_children);
		to_add.setSelf(p_child);
		m_children[p_index] = to_add;
	}

	public void addChild(VTreeNode<T> p_child, int p_index){
		if(p_child != null){
			p_child.m_parent = this;
		}
		
		m_children[p_index] = p_child;
	}

	public VTreeNode<T> getRandomChild(){
		return ArrayCalc.randomElement(m_children);
	}

	public void addRandomChild(VTreeNode<T> p_child){
		p_child.m_parent = this;
		m_children[ArrayCalc.randomIndex<VTreeNode<T>>(m_children)] = p_child;
	}

}
