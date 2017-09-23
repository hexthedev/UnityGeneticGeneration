using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionController : MonoBehaviour {

	GameController m_game_controller;

	List<DNA> m_gene_pool = new List<DNA>();

	float m_birth_timer = 0;
	
	public float m_birth_threshold;
	public int m_mutation_percentage;

	void Start(){
		m_game_controller = gameObject.GetComponent<GameController>();

		massSpawn(10);
	}

	void Update(){
	 m_birth_timer += Time.deltaTime;

		if(m_birth_timer >= m_birth_threshold){
			birth();
			m_birth_timer = 0;
		}
	}

	public void addDNA(DNA p_dna){
		m_gene_pool.Add(p_dna);
		//Debug.Log("GenePool: " + m_gene_pool.Count);
	} 

	private void birth(){
		
		if(m_gene_pool.Count < 2){
			return;
		}

		DNA dna1 = m_gene_pool[Random.Range(0, m_gene_pool.Count)];
		m_gene_pool.Remove(dna1);
		DNA dna2 = m_gene_pool[Random.Range(0, m_gene_pool.Count)];
		m_gene_pool.Remove(dna2);


		DNA evolved = DNA.evolove(dna1, dna2);

		string debug = "";

		if(Random.Range(0,100) < m_mutation_percentage){
			debug += "MUTATION :: ";
			evolved = evolved.mutate();
		}

		m_game_controller.spawn(evolved);
	}

	private void massSpawn(int p_amount){
		for(int i = 0; i<p_amount; i++){
			m_game_controller.spawn(new DNA());
		}
	}










}
