using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitnessList
{

	FitnessObject[] m_objects;

  public FitnessList(int p_size)
  {
		m_objects = new FitnessObject[p_size];
		randomFill();
  }

	public void randomFill(){
		for(int i = 0; i<m_objects.Length; i++){
			m_objects[i] = new FitnessObject(0, EvoObject.random());
		}
	}

	public void randomHalf(){
		for(int i = m_objects.Length/2; i<m_objects.Length; i++){
			m_objects[i] = new FitnessObject(0, EvoObject.random());
		}
	}

	public void add(float p_fitness, EvoObject p_evo){
		if(!(m_objects[m_objects.Length-1].getFitness() > p_fitness)){
			m_objects[m_objects.Length-1] = new FitnessObject(p_fitness, p_evo.clone());
			System.Array.Sort(m_objects, new FitnessComparer());
		}
	}

	public EvoObject getRandomObject(){
		float indexNum = Mathf.Pow(Random.Range(0f,1f), 2);
		int index = (int)Mathf.Round(indexNum*(m_objects.Length-1));

		return m_objects[index].getDna();
	}

	public void modifyFitness(FitnessMod p_mod){
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
