using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionController : MonoBehaviour {

	GameController m_game_controller;

	DataCollector m_data;

	FitnessList m_gene_pool;

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
		m_gene_pool = new FitnessList(25);

		massSpawn(m_start_spawn);
	}

	void Update(){
	 m_birth_timer += Time.deltaTime;

		if(m_birth_timer >= m_seconds_per_birth){
			massSpawn(m_start_spawn);
			m_birth_timer = 0;
		}
	}

	public void addDNA(EvoObject p_dna, float fitness){
		m_gene_pool.add(fitness, p_dna);
	} 

	public void playerChangeFitMod(){
		m_gene_pool.modifyFitness( (float p_to_mod) => { return p_to_mod/2; } );
		m_gene_pool.randomHalf();
	}

	private void birth(){

		EvoObject evo1 = m_gene_pool.getRandomObject();
		EvoObject evo2 = m_gene_pool.getRandomObject();

		// Debug.Log(m_gene_pool.print());


		//DNA SEQUENCE
		DNA evolved = DNA.crossover(evo1.getDNA(), evo2.getDNA());

		//BEHAVIOUR SEQUENCE
		BehaviourDNA evo_behav = BehaviourDNA.crossover(evo1.GetBehaviour(), evo2.GetBehaviour());
		
		if(Random.Range(0,100) < m_mutation_chance_percentage){
			// Debug.Log("MUTATE");
			evolved = evolved.mutate();
			evo_behav.mutate();
		}

		m_game_controller.spawn(new EvoObject(evolved, evo_behav), m_creature);
		m_creature++;
	}

	private void massSpawn(int p_amount){
		for(int i = 0; i<p_amount; i++){
			birth();
		}
	}







}
