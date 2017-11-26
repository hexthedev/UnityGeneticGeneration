using System.Collections;
using System.Collections.Generic;

using Genetic.Base;
using Genetic.Behaviour.BehaviourTrees;
using Genetic.Composite;
using Genetic.Traits.Base;
using Genetic.Traits.TraitGenes;
using JTools.Calc.ActiavationFunctions;
using JTools.Calc.Base;
using JTools.Calc.Lines;
using JTools.Calc.Vectors;
using JTools.Events;
using JTools.Interfaces;
using UnityEngine;

//T1 is concrete creature, T2 is the gamecontroller, T3 is the MindBodyDNA, T4 is the Concrete Mindbody, T5 is the AI datastructure
public abstract class ALineFollowingGameController<T1, T2, T3, T4, T5> : MonoBehaviour
  where T1:ALineFollowingCreature<T1, T2, T3, T4, T5>
  where T2:ALineFollowingGameController<T1, T2, T3, T4, T5>
	where T3:ADNA<T3>, IControllerExpressable<T1, T4>, ICloneable<T3>
	where T4:AMindBody<T5>, IBrain
	where T5:IBrain {

	public GameObject m_prefab;

	public DNABasedEvolutionManager<T3> m_evolution;

	IntervalEventManager m_interval;
	
	Line2D m_goalLine = new Line2D(new Vector2(15,15), new Vector2(1,-0.5f));

	public float m_time;

	[Range(1,5)]
	public float m_time_scale;

	void Start () {
		m_evolution = createManager();
	
		for(int i = 0; i<10; i++){
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

  protected abstract DNABasedEvolutionManager<T3> createManager();
	
	// Update is called once per frame
	void FixedUpdate () {
		Time.timeScale = m_time_scale;	

		m_interval.tick(Time.fixedDeltaTime);
		m_evolution.tick(Time.fixedDeltaTime);

		//Draw the line we're following
    Debug.DrawLine(m_goalLine.Point, m_goalLine.Point + m_goalLine.Direction * 100f, Color.green, Time.fixedDeltaTime);
    Debug.DrawLine(m_goalLine.Point, m_goalLine.Point + m_goalLine.Direction * -100f, Color.green, Time.fixedDeltaTime);
	}

	void Update(){
		m_time+=Time.deltaTime;
	}

  protected abstract T2 getSelf();

	void spawn(){
		GameObject obj = Instantiate(m_prefab, Vector3.zero, /*Quaternion.identity*/ Quaternion.Euler(0,0,Random.Range(0,360)));
		T1 cre = obj.GetComponent<T1>();		
		cre.Initialize(m_evolution.birth(), getSelf(), m_goalLine);
	}

	public void logDNA(T3 dna, float fitness){
		m_evolution.addDNA(dna, fitness);
	}
}



