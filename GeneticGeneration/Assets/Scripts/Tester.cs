using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.Calc.Base;

using Genetic.Base;
using Genetic.Numerical.Base;
using Genetic.Behaviour.DecisionNets;

using MathNet.Numerics.LinearAlgebra;

public class Tester : MonoBehaviour {

	DNABasedEvolutionController<DecisionNetDNA<float>> m_evolution;


	// Use this for initialization
	void Start () {

		DDecisionNetInputFactory<float>[] x = new DDecisionNetInputFactory<float>[] { 
			(float p_controller) => { return () => { return 1;}; },
			(float p_controller) => { return () => { return 2;}; }
		};

		DDecisionNetOutputFactory<float>[] y = new DDecisionNetOutputFactory<float>[] { 
			(float p_controller) => { return (float p_input) => { Debug.Log(p_input) ;}; },
		};

		m_evolution = new DNABasedEvolutionController<DecisionNetDNA<float>>( 
			new DecisionNetSpecies<float>(0, x, y, new Range<float>(-2, 2)), 0.2f, 5
		);
	
		for(int i = 0; i<5; i++){
			m_evolution.addRandom();
			
			DecisionNet test = m_evolution.birth().express(1);

			Debug.Log(test);
			test.brainAction();
		}

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
