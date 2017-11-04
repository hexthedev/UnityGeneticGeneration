using System.Collections;
using System.Collections.Generic;

using Genetic.Base;
using Genetic.Behaviour.Controllers;
using Genetic.Behaviour.DecisionNets;
using Genetic.Composite;
using Genetic.Numerical.Base;
using Genetic.Numerical.TraitGenes;

using JTools.Calc.Base;
using JTools.Events;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject m_prefab;

	public DNABasedEvolutionController<MindBodyDNA<BrainController>> m_evolution;
	BrainControllerFactory factory;
	IntervalEventManager m_manager;

	void Start () {
		factory = new BrainControllerFactory();

		m_evolution = new DNABasedEvolutionController<MindBodyDNA<BrainController>>(
			 new MindBodySpecies<BrainController>(0,
			 	new TraitGenesSpecies(0, new HashSet<ETrait> {ETrait.SPEED}, 4, new Range<float>(1f, 4f), 4, new Range<float>(-0.5f, 0.5f)),
				new DecisionNetSpecies<BrainController>( 0, factory.getInputs(), factory.getOutputs(), new Range<float>(0.5f, 2f) )
			 ), 0.5f, 5
		);
	
		for(int i = 0; i<3; i++){
			m_evolution.addRandom();
		}

		spawn();

		m_manager = new IntervalEventManager();

		m_manager.addListener(2f, () => { 
			spawn();
	 } );
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		m_manager.tick(Time.fixedDeltaTime);
	}

	void spawn(){
		GameObject obj = Instantiate(m_prefab, Vector3.zero, Quaternion.identity);
		Thing thing = obj.GetComponent<Thing>();
		
		MindBodyDNA<BrainController> dna = m_evolution.birth();

		MindBody soul = dna.express(obj.GetComponent<BrainController>());

		obj.GetComponent<Thing>().Initialze( soul, dna, this );
	}
}
