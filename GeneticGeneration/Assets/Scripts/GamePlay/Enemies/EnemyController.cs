using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class EnemyController : MonoBehaviour, IDamagable {

	private ObjectLogger m_logger;
	private EvolutionController m_evolution_controller;

	private DNA m_dna;

	private Dictionary<ETrait, StatTuple> m_stats;

	private float m_forward = -90;

	private BehaviourTree m_behav_tree;

	private float m_fitness;
	public float m_fitness_threshold;


	// Use this for initialization
	void Start () {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		

		m_behav_tree = BehaviourTree.random(m_logger, gameObject);

		
	}
	
	// Update is called once per frame
	void Update () {

		m_behav_tree.act();

		m_fitness += Time.deltaTime;

		if(m_fitness >= m_fitness_threshold){
			m_fitness = 0;
			m_evolution_controller.addDNA(m_dna.clone());
		}
	}

	public void Initalize(DNA p_dna, ObjectLogger p_logger){
		m_stats = new Dictionary<ETrait, StatTuple>();
		
		//All passed in float are between 1 and 10. They need converting for proper values
		float attack = p_dna.getTraitValue(ETrait.ATTACK)/2;
		m_stats[ETrait.ATTACK] = new StatTuple(attack, attack);

		float defense = p_dna.getTraitValue(ETrait.DEFENSE)/2;
		m_stats[ETrait.DEFENSE] = new StatTuple(defense, defense);

		float speed = p_dna.getTraitValue(ETrait.SPEED);
		m_stats[ETrait.SPEED] = new StatTuple(speed, speed);

		float hp = p_dna.getTraitValue(ETrait.HP)*2;
		m_stats[ETrait.HP] = new StatTuple(hp, hp);

		m_dna = p_dna;

		m_logger = p_logger;
		m_logger.log(gameObject, EObjectTypes.ENEMY);
	}

	public void damage(float p_damage){
		float damage = p_damage - m_stats[ETrait.DEFENSE].m_current <= 0.5f? 0.5f: p_damage-m_stats[ETrait.DEFENSE].m_current;
		
		m_stats[ETrait.HP].m_current -= damage;
		
		if(m_stats[ETrait.HP].m_current <= 0){
			m_logger.unlog(gameObject, EObjectTypes.ENEMY);
			Destroy(gameObject);
		}
	}	

	public float getTrait(ETrait p_trait, bool p_want_current ){
		return p_want_current ? m_stats[p_trait].m_current : m_stats[p_trait].m_total;
	}
}
