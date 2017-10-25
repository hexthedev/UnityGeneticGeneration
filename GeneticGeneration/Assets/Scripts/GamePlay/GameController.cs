using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject m_enemy;
	
	[SerializeField]
	[Range(1,25)]
	private float m_game_speed;

	//Species Management
	private Dictionary<int, EvolutionController> m_species_dict;
	private int num_of_species;
	private List<EvolutionController> m_species;

	//For Testing
	public int m_spawn_amount;
	public float m_spawn_rate;
	public bool m_print_fitness;
	public float m_mutation_rate;

	//Event Management
	private IntervalEventManager m_interval;

	void Start(){
		m_species_dict = new Dictionary<int, EvolutionController>();
		num_of_species = 0;
		m_species = new List<EvolutionController>();

		addSpecies();
		massSpawn(m_spawn_amount, 0);

		m_interval = new IntervalEventManager();
		m_interval.addListener(m_spawn_rate, () => { massSpawn(m_spawn_amount,0); } );
		// m_interval.addListener(10f, () => { 
		// 		foreach(EvolutionController evo in m_species){
		// 			evo.ageFitness(0f);
		// 		} 
		// 	} 
		// );
		if(m_print_fitness) m_interval.addListener(4f, () => { Debug.Log(m_species_dict[0].print()); });
	}

	void FixedUpdate(){
		Time.timeScale = m_game_speed;
		m_interval.tick(Time.fixedDeltaTime);
	}

	//Spawning
	private void massSpawn(int p_number, int p_species_id){
		for(int i = 0; i<p_number; i++){
			spawn(p_species_id);
		}
	}

	private void spawn(int p_species_id){		
		GameObject creature =  Instantiate(m_enemy, new Vector3( Random.Range(-10, 10), Random.Range(-10, 10), 0), Quaternion.identity );
		EvolutionController evo = m_species[p_species_id];
		creature.GetComponent<CreatureController>().Initalize(evo.birth(), evo.getCreaturesBirthed(), p_species_id);
	}

	//Species Managment
	private void addSpecies(){
		EvolutionController evo = new EvolutionController(m_mutation_rate);
		m_species.Add(evo);
		m_species_dict.Add(num_of_species++, evo);
	}

	public void addDNA(DNA p_dna, float p_fitness, int p_species_id){
		m_species[p_species_id].addDNA(p_dna, p_fitness);
	}


}
