using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA {

	private PhysicalDNA m_body;

	private NeuralDNA m_mind;

	public DNA(PhysicalDNA p_body, NeuralDNA p_mind){
		m_body = p_body;
		m_mind = p_mind;
	}

	public NeuralNet expressMind(CreatureController p_controller){
		return m_mind.expressDNA(p_controller);
	}

	public Dictionary<ETrait, StatTuple> expressBody(){
		return m_body.expressDNA();
	}


	//EVOLUTION FUNCTIONS
	public static DNA crossover(DNA dna_1, DNA dna_2){
		PhysicalDNA body = PhysicalDNA.crossover(dna_1.m_body, dna_2.m_body);
		NeuralDNA mind = NeuralDNA.crossover(dna_1.m_mind, dna_2.m_mind);

		return new DNA(body, mind);
	}
	
	public void mutate(){
		m_body.mutate();
		m_mind.mutate();
	}

	public DNA clone(){
		return new DNA(m_body.clone(), m_mind.clone());
	}
}
