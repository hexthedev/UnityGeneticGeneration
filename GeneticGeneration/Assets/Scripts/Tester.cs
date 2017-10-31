using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.Calc.Base;

using Genetic.Base;
using Genetic.Numerical.Base;
using Genetic.Numerical.TraitGenes;

public class Tester : MonoBehaviour {

	DNABasedEvolutionController<TraitGenesDNA> m_evolution;


	// Use this for initialization
	void Start () {
		m_evolution = new DNABasedEvolutionController<TraitGenesDNA>( new TraitGenesSpecies( 0, new HashSet<ETrait> {ETrait.ATTACK, ETrait.DEFENSE}, 5, new Range<float>(0, 2), 3, new Range<float>(-1, 2)), 4, 5 );
	
		for(int i = 0; i<2; i++){
			m_evolution.addRandom();
		}

		Debug.Log(m_evolution.birth());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
