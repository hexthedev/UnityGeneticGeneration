using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourDNA {

	VTreeNode<IBehaviourGenoType> m_root;

	public BehaviourDNA(VTreeNode<IBehaviourGenoType> p_root){
		m_root = p_root;
	}

	void random(){
		m_root = RandomGen.BehaviourDNARoot();
	}

	public VTreeNode<IBehaviourGenoType> getRoot(){
		return m_root;
	}
}
