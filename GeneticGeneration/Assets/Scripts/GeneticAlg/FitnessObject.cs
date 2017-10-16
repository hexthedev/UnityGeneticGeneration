using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	/*public DNA getDna(){
		return m_dna.clone();
	}*/

	public void modifyFitness(FitnessMod p_del_mod){
		m_fitness = p_del_mod(m_fitness);
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

public delegate float FitnessMod(float p_mod);