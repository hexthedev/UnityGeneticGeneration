using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalDNA {
	
	private Dictionary<ETrait, Chromo> m_chromos = new Dictionary<ETrait, Chromo>();
	
	public PhysicalDNA(){
		foreach(ETrait trait in System.Enum.GetValues(typeof(ETrait))){
			m_chromos.Add(trait, new Chromo());
		}
	}

	public PhysicalDNA(Dictionary<ETrait, Chromo> p_chromos){
		m_chromos = p_chromos;
	}

	//This is the stat representation
	private Chromo getChromo(ETrait p_trait){
		return m_chromos[p_trait];
	}

	public float getTraitValue(ETrait p_trait){
		return m_chromos[p_trait].getGeneValue();
	}

	//Clonable
	public PhysicalDNA clone(){
		Dictionary<ETrait, Chromo> clone_chromos = new Dictionary<ETrait, Chromo>();

		foreach(ETrait trait in System.Enum.GetValues(typeof(ETrait))){
			clone_chromos.Add(trait, m_chromos[trait].clone());
		}

		return new PhysicalDNA(clone_chromos);
	}

	
	//ToString
	public override string ToString(){
		return "DNA -- At: " + m_chromos[ETrait.ATTACK].getGeneValue() + ", De: " + m_chromos[ETrait.DEFENSE].getGeneValue() + ", Sp: " + m_chromos[ETrait.SPEED].getGeneValue() + ", Hp: " + m_chromos[ETrait.HP].getGeneValue();
	}

	//DNA Expression Functions
	public Dictionary<ETrait, StatTuple> expressDNA(){
		Dictionary<ETrait, StatTuple> to_return = new Dictionary<ETrait, StatTuple>();

		foreach(ETrait stat in m_chromos.Keys){
			float value = getTraitValue(stat);
			to_return.Add(stat, new StatTuple(value, value));
		}

		return to_return;
	}

	//Evolution Functions
	public PhysicalDNA mutate(){
		PhysicalDNA clone = this.clone();

		foreach(ETrait trait in System.Enum.GetValues(typeof(ETrait))){
			clone.m_chromos[trait] = clone.m_chromos[trait].mutate();
		}
		
		return clone;
	}

	public static PhysicalDNA crossover(PhysicalDNA p_dna1, PhysicalDNA p_dna2){
		Dictionary<ETrait, Chromo> chromos = new Dictionary<ETrait, Chromo>();

		foreach(ETrait trait in System.Enum.GetValues(typeof(ETrait))){
			chromos.Add(trait, Chromo.crossover(p_dna1.getChromo(trait), p_dna2.getChromo(trait)));
		}
		
		return new PhysicalDNA(chromos);
	}

	//Data Collection
	public string[] getStatsCSV(int creature, float p_fitness){
		string[] stats = {creature.ToString(), m_chromos[ETrait.ATTACK].getGeneValue().ToString(),  m_chromos[ETrait.DEFENSE].getGeneValue().ToString(), m_chromos[ETrait.SPEED].getGeneValue().ToString(), m_chromos[ETrait.HP].getGeneValue().ToString(), p_fitness + ""};
		return stats;
	}



}
