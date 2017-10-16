using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalDNA {

	private Dictionary<ETrait, Chromo> m_chromos = new Dictionary<ETrait, Chromo>();
	
	public PhysicalDNA(){
		m_chromos.Add(ETrait.ATTACK, new Chromo());
		m_chromos.Add(ETrait.DEFENSE, new Chromo());
		m_chromos.Add(ETrait.HP, new Chromo());
		m_chromos.Add(ETrait.SPEED, new Chromo());
	}

	public PhysicalDNA(Dictionary<ETrait, Chromo> p_chromos){
		m_chromos = p_chromos;
	}

	private Chromo getChromo(ETrait p_trait){
		return m_chromos[p_trait];
	}

	public float getTraitValue(ETrait p_trait){
		return m_chromos[p_trait].getGeneValue();
	}

	public PhysicalDNA clone(){
		Dictionary<ETrait, Chromo> clone_chromos = new Dictionary<ETrait, Chromo>();

		clone_chromos.Add(ETrait.ATTACK, m_chromos[ETrait.ATTACK].clone());
		clone_chromos.Add(ETrait.DEFENSE, m_chromos[ETrait.DEFENSE].clone());
		clone_chromos.Add(ETrait.SPEED, m_chromos[ETrait.SPEED].clone());
		clone_chromos.Add(ETrait.HP, m_chromos[ETrait.HP].clone());

		return new PhysicalDNA(clone_chromos);
	}

	public string print(){
		return "DNA -- At: " + m_chromos[ETrait.ATTACK].getGeneValue() + ", De: " + m_chromos[ETrait.DEFENSE].getGeneValue() + ", Sp: " + m_chromos[ETrait.SPEED].getGeneValue() + ", Hp: " + m_chromos[ETrait.HP].getGeneValue();
	}


	public PhysicalDNA mutate(){
		PhysicalDNA clone = this.clone();

		clone.m_chromos[ETrait.ATTACK] = clone.m_chromos[ETrait.ATTACK].mutate();
		clone.m_chromos[ETrait.DEFENSE] = clone.m_chromos[ETrait.DEFENSE].mutate();
		clone.m_chromos[ETrait.SPEED] = clone.m_chromos[ETrait.SPEED].mutate();
		clone.m_chromos[ETrait.HP] = clone.m_chromos[ETrait.HP].mutate();
		
		return clone;
	}

	public static PhysicalDNA crossover(PhysicalDNA p_dna1, PhysicalDNA p_dna2){
		Dictionary<ETrait, Chromo> chromos = new Dictionary<ETrait, Chromo>();

		chromos.Add(ETrait.ATTACK, Chromo.crossover(p_dna1.getChromo(ETrait.ATTACK), p_dna2.getChromo(ETrait.ATTACK)));
		chromos.Add(ETrait.DEFENSE, Chromo.crossover(p_dna1.getChromo(ETrait.DEFENSE), p_dna2.getChromo(ETrait.DEFENSE)));
		chromos.Add(ETrait.SPEED, Chromo.crossover(p_dna1.getChromo(ETrait.SPEED), p_dna2.getChromo(ETrait.SPEED)));
		chromos.Add(ETrait.HP, Chromo.crossover(p_dna1.getChromo(ETrait.HP), p_dna2.getChromo(ETrait.HP)));
		
		return new PhysicalDNA(chromos);
	}

	public string[] getStatsCSV(int creature, float p_fitness){
		string[] stats = {creature.ToString(), m_chromos[ETrait.ATTACK].getGeneValue().ToString(),  m_chromos[ETrait.DEFENSE].getGeneValue().ToString(), m_chromos[ETrait.SPEED].getGeneValue().ToString(), m_chromos[ETrait.HP].getGeneValue().ToString(), p_fitness + "\n"};
		return stats;
	}



}
