using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionController : MonoBehaviour {

	GameController m_game_controller;

	List<EvoObject> m_gene_pool = new List<EvoObject>();

	float m_birth_timer = 0;

	public int m_start_spawn;	
	public float m_birth_threshold;
	public int m_mutation_percentage;

	void Start(){
		m_game_controller = gameObject.GetComponent<GameController>();

		massSpawn(m_start_spawn);
	}

	void Update(){
	 m_birth_timer += Time.deltaTime;

		if(m_birth_timer >= m_birth_threshold){
			birth();
			m_birth_timer = 0;
		}
	}

	public void addDNA(DNA p_dna, BehaviourTree p_behaviour){
		m_gene_pool.Add(new EvoObject(p_dna, p_behaviour));
		//Debug.Log("GenePool: " + m_gene_pool.Count);
	} 

	private void birth(){
		
		if(m_gene_pool.Count < 2){
			return;
		}

		EvoObject evo1 = m_gene_pool[Random.Range(0, m_gene_pool.Count)];
		m_gene_pool.Remove(evo1);
		EvoObject evo2 = m_gene_pool[Random.Range(0, m_gene_pool.Count)];
		m_gene_pool.Remove(evo2);


		DNA evolved = DNA.evolove(evo1.getDNA(), evo2.getDNA());

		string debug = "";

		if(Random.Range(0,100) < m_mutation_percentage){
			debug += "MUTATION :: ";
			evolved = evolved.mutate();
		}

		m_game_controller.spawn(new EvoObject(evolved, null));
	}

	private void massSpawn(int p_amount){
		for(int i = 0; i<p_amount; i++){
			m_game_controller.spawn(new EvoObject(new DNA(), null));
		}
	}










}
