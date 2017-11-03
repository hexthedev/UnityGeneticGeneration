using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.Calc.Base;

using Genetic.Base;
using Genetic.Numerical.Base;
using Genetic.Behaviour.DecisionNets;
using Genetic.Behaviour.Controllers;

using MathNet.Numerics.LinearAlgebra;

public class Tester : MonoBehaviour {

	public BrainController obj;

	DNABasedEvolutionController<DecisionNetDNA<BrainController>> m_evolution;

	BrainControllerFactory factory;


	void Start () {
		factory = new BrainControllerFactory();

		m_evolution = new DNABasedEvolutionController<DecisionNetDNA<BrainController>>( 
			new DecisionNetSpecies<BrainController>(0, factory.getInputs(), factory.getOutputs(), new Range<float>(0.5f, 2)), 0.2f, 5
		);
	
		for(int i = 0; i<5; i++){
			m_evolution.addRandom();
		}

		DecisionNet net = m_evolution.birth().express(obj);

		obj.InitializeBrain(net);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
