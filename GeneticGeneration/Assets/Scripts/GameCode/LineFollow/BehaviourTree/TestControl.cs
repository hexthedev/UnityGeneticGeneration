using System.Collections;
using System.Collections.Generic;
using Genetic.Base;
using Genetic.Behaviour.BehaviourTrees;
using JTools.Calc.Base;
using UnityEngine;

public class TestControl : AController {
  
	BTDNATree<TestControl> m_dna;
	BTree m_control;

	DInputFactory<TestControl>[] inputs1 = { 
		(TestControl p_thing) => { return () => {return Mathf.Sin(Time.realtimeSinceStartup);}; },
		(TestControl p_thing) => { return () => {return Mathf.Cos(Time.realtimeSinceStartup);}; },
		(TestControl p_thing) => { return () => {return Mathf.Tan(Time.realtimeSinceStartup);}; }, 
		};

	DOutputFactory<TestControl>[] outputs1 = { 
		(TestControl p_thing) => { return (float p_val) => { Debug.Log("out1: " + p_val); }; },
		(TestControl p_thing) => { return (float p_val) => { Debug.Log("out2: " + p_val); }; },
		(TestControl p_thing) => { return (float p_val) => { Debug.Log("out3: " + p_val); }; }   
		};
	

  // Use this for initialization
  void Start () {
		m_dna = BTDNATree<TestControl>.random( new Range<float>(0.5f,1f), new Range<float>(0.1f,0.9f), inputs1, outputs1 );
		m_control = m_dna.expressConcrete(this);

		Debug.Log(m_dna.size());
		Debug.Log(m_control.size());
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	protected override void act()
  {
		m_control.traverse();
  }
}
