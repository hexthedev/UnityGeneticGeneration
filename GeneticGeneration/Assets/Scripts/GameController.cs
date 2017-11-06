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
using JTools.Events;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject m_prefab;
	public DNABasedEvolutionManager<MindBodyDNA<DecisionNetCreature>> m_evolution;
	IntervalEventManager m_manager;

	void Start () {
		m_evolution = new DNABasedEvolutionManager<MindBodyDNA<DecisionNetCreature>>(
			 new MindBodySpecies<DecisionNetCreature>(0,
			 	new TraitGenesSpecies(0, new HashSet<ETrait> {ETrait.SPEED}, 4, new Range<float>(0.25f, 1f), 4, new Range<float>(-0.5f, 0.5f)),
				new DecisionNetSpecies<DecisionNetCreature>( 0, DecisionNetCreature.getInputFactorys(), DecisionNetCreature.getOutputFactorys(), new Range<float>(0.8f, 1.2f) )
			 ), 0.1f, 100
		);
	
		for(int i = 0; i<20; i++){
			m_evolution.addRandom();
		}

		m_manager = new IntervalEventManager();

		m_manager.addListener(0.25f, () => {
			for(int i = 0; i<10; i++){
				spawn();
			}
	 } );

	
	
	
	
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		m_manager.tick(Time.fixedDeltaTime);
	}

	void spawn(){
		GameObject obj = Instantiate(m_prefab, Vector3.zero, /*Quaternion.identity*/ Quaternion.Euler(0,0,Random.Range(0,360)));
		DecisionNetCreature cre = obj.GetComponent<DecisionNetCreature>();
		cre.Initialize(m_evolution.birth(), this);
	}

	public void logDNA(MindBodyDNA<DecisionNetCreature> dna, float fitness){
		m_evolution.addDNA(dna, fitness);
	}
}



