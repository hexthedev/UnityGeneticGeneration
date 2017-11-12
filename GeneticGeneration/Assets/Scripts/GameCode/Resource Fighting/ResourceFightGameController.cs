using System.Collections;
using System.Collections.Generic;

using Genetic.Base;
using Genetic.Behaviour.DecisionNets;
using Genetic.Composite;
using Genetic.Traits.Base;
using Genetic.Traits.TraitGenes;

using JTools.Calc.ActiavationFunctions;
using JTools.Calc.Base;
using JTools.Calc.Lines;
using JTools.Calc.Vectors;
using JTools.Calc.Rand;

using JTools.Events;
using JTools.DataStructures.ObjectLogger;

using UnityEngine;

public class ResourceFightGameController : MonoBehaviour {

	//Prefab representing creature that gets spawned
	public GameObject m_creature_prefab;
	public GameObject m_resource_prefab;
	

	//Evolution Manager
	public DNABasedEvolutionManager<MindBodyDNA<ResourceFightDNCreature>> m_evolution;

	//Other Tools
	IntervalEventManager m_interval;
	public float m_time;

	void Start () {
		//Instantiate controller
		m_evolution = new DNABasedEvolutionManager<MindBodyDNA<ResourceFightDNCreature>>(
			 new MindBodySpecies<ResourceFightDNCreature>(0,
			 	new TraitGenesSpecies(0, new HashSet<string> {"SPEED", "HEALTH", "DAMAGE", "ENERGY", "ATTACKSPEED"}, 4, new Range<float>(0.25f, 1f), 4, new Range<float>(-0.5f, 0.5f)),
				new DecisionNetSpecies<ResourceFightDNCreature>( 0, ResourceFightDNCreature.getInputFactorys(), ResourceFightDNCreature.getOutputFactorys(), new Range<float>(0.8f, 1.2f) )
			 ), 0.1f, 10, (float p_fitness) => { return p_fitness * 0.95f; }, 1f 
		);
	
		//Fill with 20 random
		for(int i = 0; i<10; i++){
			m_evolution.addRandom();
		}

		//Instantaite Interval and Listeners
		m_interval = new IntervalEventManager();
		
		for(int i = 0; i<5; i++){
			spawnResource();
		}
		
		//spawnCreature();
		spawnCreature();
		

		// m_interval.addListener(1f, () => {
		// 	for(int i = 0; i<1; i++){
		// 		spawn();
		// 	}
	 	// } );
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		m_interval.tick(Time.fixedDeltaTime);
		m_evolution.tick(Time.fixedDeltaTime);

		m_time+=Time.fixedDeltaTime;
	}

	//Spawning: Controls location, initialization, etc. 
	void spawnCreature(){
		GameObject obj = Instantiate(
			m_creature_prefab, 
			new Vector3( 
				/*RandomCalc.Rand(new Range<float>(-10f, 10)),
				RandomCalc.Rand(new Range<float>(-10f, 10)),*/ 0,0,
				0 
			), 
			Quaternion.Euler(0,0,Random.Range(0,360))
		);

		ResourceFightDNCreature cre = obj.GetComponent<ResourceFightDNCreature>();		
		cre.Initialize(m_evolution.birth(), this);
	}

	void spawnResource(){
		GameObject obj = Instantiate( m_resource_prefab, Vector3Calc.randomDirection()*5, Quaternion.identity);
		obj.GetComponent<Resource>().Initalize( RandomCalc.Rand(new Range<float>(1f,2f) ) );
		ObjectLogger.log(obj, "RESOURCE");
	}

	//Logs DNA and fitness in evolution controller
	public void logDNA(MindBodyDNA<ResourceFightDNCreature> dna, float fitness){
		m_evolution.addDNA(dna, fitness);
	}
}



