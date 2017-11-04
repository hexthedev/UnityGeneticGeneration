using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.Calc.Base;
using JTools.Events;

using Genetic.Base;
using Genetic.Numerical.Base;
using Genetic.Numerical.TraitGenes;

using Genetic.Behaviour.DecisionNets;
using Genetic.Behaviour.Controllers;

using Genetic.Composite;

using MathNet.Numerics.LinearAlgebra;

public class Tester : MonoBehaviour {

	public BrainController obj;

	DNABasedEvolutionController<MindBodyDNA<BrainController>> m_evolution;

	BrainControllerFactory factory;

	IntervalEventManager m_manger;

	void Start () {
		factory = new BrainControllerFactory();

		m_evolution = new DNABasedEvolutionController<MindBodyDNA<BrainController>>(
			 new MindBodySpecies<BrainController>(0,
			 	new TraitGenesSpecies(0, new HashSet<ETrait> {ETrait.ATTACK, ETrait.HP}, 4, new Range<float>(1f, 2f), 1, new Range<float>(2f, 2f)),
				new DecisionNetSpecies<BrainController>( 0, factory.getInputs(), factory.getOutputs(), new Range<float>(0.5f, 2f) )
			 ), 0f, 5
		);
	
		for(int i = 0; i<3; i++){
			m_evolution.addRandom();
		}

		m_manger = new IntervalEventManager();

		m_manger.addListener(1f, () => { 
			obj.gameObject.transform.position = Vector3.zero;
			obj.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		//	m_evolution.addRandom();
			MindBody stuff = m_evolution.birth().express(obj);
			obj.InitializeBrain(stuff.m_mind);
			Debug.Log(stuff.m_body[0]);
		 } );
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		m_manger.tick(Time.fixedDeltaTime);
	}
}
