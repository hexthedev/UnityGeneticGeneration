using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionController {

	GameController m_game_controller;
	FitnessList m_gene_pool;
	float m_mutation_chance_percentage;
	int m_creatures_birthed = 0;

	public EvolutionController(float p_mutation_chance_percentage){
		m_gene_pool = new FitnessList(25);
		m_mutation_chance_percentage = p_mutation_chance_percentage;
	}

	public void addDNA(DNA p_dna, float fitness){
		m_gene_pool.add(fitness, p_dna);
	} 

	public void ageFitness(float p_age_mod){
		m_gene_pool.modifyFitness( (float p_to_mod) => { return p_to_mod-p_age_mod; } );
	}

	public DNA birth(){

		DNA evo1 = m_gene_pool.getRandomObject();
		DNA evo2 = m_gene_pool.getRandomObject();

		DNA birth = DNA.crossover(evo1, evo2);
		
		if(Random.Range(0,100) < m_mutation_chance_percentage){
			birth.mutate();
		}

		m_creatures_birthed++;
		return birth;
	}

	public int getCreaturesBirthed(){
		return m_creatures_birthed;
	}

	public string print(){
		return m_gene_pool.print();
	}

}
