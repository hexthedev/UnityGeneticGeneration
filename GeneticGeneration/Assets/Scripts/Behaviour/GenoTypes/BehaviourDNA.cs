using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

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

	public BehaviourDNA clone(){
		return new BehaviourDNA(m_root.getSelf().clone(null));
	}

	public void mutate(){
		m_root.getSelf().mutate();
	}

	public static BehaviourDNA crossover(BehaviourDNA p_dna1, BehaviourDNA p_dna2){

		BehaviourDNA master;
		BehaviourDNA lesser;
		 
		if(BoolCalc.random()){
			master = p_dna1;
			lesser = p_dna2;
		} else {
			master = p_dna2;
			lesser = p_dna1;
		}

		VTreeNode<IBehaviourGenoType>[] randomPathMaster = getRandomPath(master.getRoot()); 
		VTreeNode<IBehaviourGenoType>[] randomPathLesser = getRandomPath(lesser.getRoot());

		VTreeNode<IBehaviourGenoType> master_node = ArrayCalc.randomElement<VTreeNode<IBehaviourGenoType>>(randomPathMaster);
		VTreeNode<IBehaviourGenoType> lesser_node = ArrayCalc.randomElement<VTreeNode<IBehaviourGenoType>>(randomPathLesser);

		master_node.addRandomChild(lesser_node);

		return master.clone();
	}

	private static VTreeNode<IBehaviourGenoType>[] getRandomPath(VTreeNode<IBehaviourGenoType> m_root){

		VTreeNode<IBehaviourGenoType> testing = m_root;
		List<VTreeNode<IBehaviourGenoType>> list = new List<VTreeNode<IBehaviourGenoType>>();

		while(testing != null){
			list.Add(testing);
			testing = testing.getRandomChild();
		}

		return list.ToArray();
	}
}
