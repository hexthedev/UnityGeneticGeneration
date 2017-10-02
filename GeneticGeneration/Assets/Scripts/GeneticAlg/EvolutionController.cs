using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionController : MonoBehaviour {

	GameController m_game_controller;

	DataCollector m_data;

	SortedList<float , EvoObject> m_gene_pool = new SortedList<float , EvoObject>(new FitnessComparer());

	float m_birth_timer = 0;


	[Header("Spawning")]
	public int m_start_spawn;	
	public float m_seconds_per_birth;


	[Header("Mutation")]
	public int m_mutation_chance_percentage;




	private int m_creature = 0;

	void Start(){
		m_game_controller = gameObject.GetComponent<GameController>();
		m_data = gameObject.GetComponent<DataCollector>();
		massSpawn(m_start_spawn);

		for(int i = 0; i<101; i++){
			m_gene_pool.Add(0, EvoObject.random());
		}
	}

	void Update(){
	 m_birth_timer += Time.deltaTime;

		if(m_birth_timer >= m_seconds_per_birth){
			birth();
			m_birth_timer = 0;
		}
	}

	public void addDNA(EvoObject p_dna, float fitness){
		/*m_gene_pool.Add(fitness, p_dna);
		m_gene_pool.RemoveAt(m_gene_pool.Count-1);

		Debug.Log(m_gene_pool.Count);*/
	} 

	private void birth(){
		
		if(m_gene_pool.Count < 2){
			return;
		}

		EvoObject evo1 = m_gene_pool.Values[fitnessChoiceIndex()];
		EvoObject evo2 = m_gene_pool.Values[fitnessChoiceIndex()];


		//DNA SEQUENCE
		DNA evolved = DNA.crossover(evo1.getDNA(), evo2.getDNA());

		//BEHAVIOUR SEQUENCE
		BehaviourDNA evo_behav = BehaviourDNA.crossover(evo1.GetBehaviour(), evo2.GetBehaviour());
		
		if(Random.Range(0,100) < m_mutation_chance_percentage){
			Debug.Log("MUTATE");
			evolved = evolved.mutate();
			evo_behav.mutate();
		}

		m_game_controller.spawn(new EvoObject(evolved, evo_behav), m_creature);
		m_creature++;
	}

	private void massSpawn(int p_amount){
		for(int i = 0; i<p_amount; i++){
			m_game_controller.spawn(EvoObject.random(), m_creature);
			m_creature++;
		}
	}


	private int fitnessChoiceIndex(){
		return (int)Mathf.Round(Mathf.Pow(Random.Range(0f, 1f), 2)*100);
	}







}
