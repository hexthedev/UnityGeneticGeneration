﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionController : MonoBehaviour {

	GameController m_game_controller;

	DataCollector m_data;

	List<EvoObject> m_gene_pool = new List<EvoObject>();

	float m_birth_timer = 0;

	public int m_start_spawn;	
	public float m_birth_threshold;
	public int m_mutation_percentage;

	private int m_creature = 0;

	void Start(){
		m_game_controller = gameObject.GetComponent<GameController>();
		m_data = gameObject.GetComponent<DataCollector>();

		/*for(int i = 0; i<1000; i++ ){
			Debug.Log(new BehaviourTree(m_game_controller.GetComponent<ObjectLogger>(), gameObject, new BehaviourDNA(RandomGen.BehaviourDNARoot()) ));
		}*/
		

		massSpawn(m_start_spawn);		
	}

	void Update(){
	 m_birth_timer += Time.deltaTime;

		if(m_birth_timer >= m_birth_threshold){
			birth();
			m_birth_timer = 0;
		}
	}

	public void addDNA(DNA p_dna, BehaviourDNA p_behaviour){
		m_gene_pool.Add(new EvoObject(p_dna, p_behaviour));
	} 

	private void birth(){
		
		if(m_gene_pool.Count < 2){
			return;
		}

		EvoObject evo1 = m_gene_pool[Random.Range(0, m_gene_pool.Count)];
		m_gene_pool.Remove(evo1);
		EvoObject evo2 = m_gene_pool[Random.Range(0, m_gene_pool.Count)];
		m_gene_pool.Remove(evo2);


		//DNA SEQUENCE
		DNA evolved = DNA.evolove(evo1.getDNA(), evo2.getDNA());

		//BEHAVIOUR SEQUENCE
		BehaviourDNA evo_behav = BehaviourDNA.crossover(evo1.GetBehaviour(), evo2.GetBehaviour());
		
		if(Random.Range(0,100) < m_mutation_percentage){
			Debug.Log("MUTATE");
			//evolved = evolved.mutate();
			evo_behav.mutate();
		}

		m_data.recordData(evolved, m_creature);
		m_creature++;

		m_game_controller.spawn(new EvoObject(evolved, evo_behav));
	}

	private void massSpawn(int p_amount){
		for(int i = 0; i<p_amount; i++){
			DNA rand_dna = new DNA();

			m_data.recordData(rand_dna, m_creature);
			m_creature++;

			m_game_controller.spawn(new EvoObject(rand_dna, new BehaviourDNA(RandomGen.BehaviourDNARoot())));
		}
	}










}
