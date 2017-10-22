using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitnessList
{
	NeuralDNA m_species;
	FitnessObject[] m_objects;

	public FitnessList(int p_size)
	{
		m_objects = new FitnessObject[p_size];
		
		m_species = new NeuralDNA();
		
		randomFill();
	}

	public void randomFill(){			
		for(int i = 0; i<m_objects.Length; i++){
			m_objects[i] = new FitnessObject(0, new DNA(new PhysicalDNA(), new NeuralDNA(m_species)));
		}
	}

	public void randomHalf(){	
		for(int i = m_objects.Length/2; i<m_objects.Length; i++){
			m_objects[i] = new FitnessObject(0, new DNA(new PhysicalDNA(), new NeuralDNA(m_species)));
		}
	}

	public void add(float p_fitness, DNA p_dna){
		if(!(m_objects[m_objects.Length-1].getFitness() > p_fitness)){
			m_objects[m_objects.Length-1] = new FitnessObject(p_fitness, p_dna.clone());
			System.Array.Sort(m_objects, new FitnessComparer());
		}
	}

	public DNA getRandomObject(){
		float indexNum = Mathf.Pow(Random.Range(0f,1f), 2);
		int index = (int)Mathf.Round(indexNum*(m_objects.Length-1));

		return m_objects[index].getDna();
	}

	public void modifyFitness(DFitnessMod p_mod){
		foreach(FitnessObject fitobj in m_objects){
			fitobj.modifyFitness(p_mod);
		}
	}

	public string print(){
		string ret = "[";

		foreach(FitnessObject x in m_objects){
			ret += x.getFitness() + ",";
		}

		ret += "]";

		return ret;
	}
	

}

public class FitnessObject {

	private float m_fitness;
	private DNA m_dna;

  public FitnessObject(float p_fitness, DNA p_dna)
  {
		m_fitness = p_fitness;
		m_dna = p_dna;	
  }

	public float getFitness(){
		return m_fitness;
	}

	public DNA getDna(){
		return m_dna.clone();
	}

	public void modifyFitness(DFitnessMod p_mod){
		m_fitness = p_mod(m_fitness);
	}

}

public class FitnessComparer : IComparer<FitnessObject>
{
  public int Compare(FitnessObject x, FitnessObject y)
  {
    int to_return = 0;

		if(x.getFitness() < y.getFitness()){
			to_return = 1;
		} else if(x.getFitness() > y.getFitness()) {
			to_return = -1;
		}

		return to_return;
  }
}

public delegate float DFitnessMod(float p_mod);
