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

public class LineFollowingGameController : MonoBehaviour {

	public GameObject m_prefab;
	public DNABasedEvolutionManager<MindBodyDNA<LineFollowingDNCreature>> m_evolution;
	IntervalEventManager m_interval;
	Line2D m_goalLine = new Line2D(new Vector2(15,15), new Vector2(1,-0.5f));

	public float m_time;

	void Start () {
		m_evolution = new DNABasedEvolutionManager<MindBodyDNA<LineFollowingDNCreature>>(
			 new MindBodySpecies<LineFollowingDNCreature>(0,
			 	new TraitGenesSpecies(0, new HashSet<string> {"SPEED"}, 4, new Range<float>(0.25f, 1f), 4, new Range<float>(-0.5f, 0.5f)),
				new DecisionNetSpecies<LineFollowingDNCreature>( 0, LineFollowingDNCreature.getInputFactorys(), LineFollowingDNCreature.getOutputFactorys(), new Range<float>(0.8f, 1.2f) )
			 ), 0.1f, 10, (float p_fitness) => { return p_fitness * 0.95f; }, 1f 
		);
	
		for(int i = 0; i<20; i++){
			m_evolution.addRandom();
		}

		m_interval = new IntervalEventManager();

		m_interval.addListener(0.25f, () => {
			for(int i = 0; i<1; i++){
				spawn();
			}
	 	} );

		m_interval.addListener(0.1f, () => {
			m_goalLine.rotate(1f);
		});

	
	
	
	
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		m_interval.tick(Time.fixedDeltaTime);
		m_evolution.tick(Time.fixedDeltaTime);

		m_time+=Time.fixedDeltaTime;
	}

	void spawn(){
		GameObject obj = Instantiate(m_prefab, Vector3.zero, /*Quaternion.identity*/ Quaternion.Euler(0,0,Random.Range(0,360)));
		LineFollowingDNCreature cre = obj.GetComponent<LineFollowingDNCreature>();		
		cre.Initialize(m_evolution.birth(), this, m_goalLine);
	}

	public void logDNA(MindBodyDNA<LineFollowingDNCreature> dna, float fitness){
		m_evolution.addDNA(dna, fitness);
	}
}



