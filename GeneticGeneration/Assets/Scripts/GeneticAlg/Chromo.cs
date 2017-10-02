using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class Chromo {

	private float[] m_genes;

	public Chromo(){
		m_genes = new float[EvolutionVars.chromo_genes];

		for(int i = 0; i<m_genes.Length; i++){
			m_genes[i] = EvolutionVars.chromo_start_value();
		}
	}

	public Chromo(float[] p_genes){
		m_genes = p_genes;
	}

	public float getGeneValue(){
		return ArrayCalc.floatArraySum(m_genes);
	}

	private float getGene(int index){
		return m_genes[index];
	}

	public Chromo clone(){
		float[] clone = { m_genes[0], m_genes[1], m_genes[2], m_genes[3], m_genes[4]};
		return new Chromo(clone);
	}

	public Chromo mutate(){
		
		Chromo clone = this.clone();

		for(int i = 0; i<EvolutionVars.chromo_mutation_iterations; i++){
			int index = Random.Range(0,5);
			clone.m_genes[index] += EvolutionVars.chromo_mutation_value();
		}
		
		return clone;
	}

	public static Chromo crossover(Chromo p_chromo1, Chromo p_chromo2){

		float[] new_gene = new float[5];

		for(int i=0; i < new_gene.Length; i++){
			new_gene[i] = Random.Range(0,2) == 0 ? p_chromo1.getGene(i): p_chromo2.getGene(i);
		}

		return new Chromo(new_gene);
	}


}
