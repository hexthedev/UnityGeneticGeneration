using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class EnemyController : MonoBehaviour, IDamagable {

	private ObjectLogger m_logger;
	private EvolutionController m_evolution_controller;

	private DNA m_dna;

	private float m_attack;
	private float m_defense;
	private float m_speed;
	private float m_hp;

	private float m_forward = -90;

	private BehaviourNode m_current_behav;

	private float m_fitness;
	public float m_fitness_threshold;


	// Use this for initialization
	void Start () {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		
		IAction[] move = {
			new MoveAction(0f, m_speed, new TowardsPlayerDirection(player, gameObject))
		};

		IAction[] rotate = { 
			new RotateAction(new TowardsPlayerDirection(player, gameObject), m_forward, m_speed) 
		};
		
		m_current_behav = new BehaviourNode( null, ActionSequence.emptySequence(), new PointingAtDetector(gameObject, m_forward, player, 10f) );

		m_current_behav.addChild(true, new ActionSequence(move), null);
		m_current_behav.addChild(false, new ActionSequence(rotate), null);

		m_evolution_controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<EvolutionController>();
	}
	
	// Update is called once per frame
	void Update () {
	
		m_current_behav = m_current_behav.act(gameObject);

		m_fitness += Time.deltaTime;

		if(m_fitness >= m_fitness_threshold){
			m_fitness = 0;
			m_evolution_controller.addDNA(m_dna.clone());
		}
	}

	public void Initalize(DNA p_dna, ObjectLogger p_logger){
		//All passed in float are between 1 and 10. They need converting for proper values
		m_attack = p_dna.getTraitValue(ETrait.ATTACK)/2;
		m_defense = p_dna.getTraitValue(ETrait.DEFENSE)/2;
		m_speed = p_dna.getTraitValue(ETrait.SPEED);
		m_hp = p_dna.getTraitValue(ETrait.HP)*2;

		m_dna = p_dna;

		m_logger = p_logger;
		m_logger.log(gameObject, EObjectTypes.ENEMY);
	}

	public void damage(float p_damage){
		float damage = p_damage - m_defense <= 0.5f? 0.5f: p_damage-m_defense;
		
		m_hp -= damage;
		if(m_hp <= 0){
			m_logger.unlog(gameObject, EObjectTypes.ENEMY);
			Destroy(gameObject);
		}
	}	
}
