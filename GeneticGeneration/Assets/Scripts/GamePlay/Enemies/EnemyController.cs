using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class EnemyController : MonoBehaviour, IDamagable {

	private ObjectLogger m_logger;
	private EvolutionController m_evolution_controller;
	private DataCollector m_data;

	private DNA m_dna;
	private BehaviourDNA m_behav_dna;
	private Dictionary<ETrait, StatTuple> m_stats;
	private BehaviourTree m_behav_tree;

	private int m_creature_id;

	private float m_forward = -90;
	private float m_fitness;
	public float m_fitness_threshold;
	public bool m_debug;


	// Use this for initialization
	void Start () {
		m_evolution_controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<EvolutionController>();
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		m_data = GameObject.FindGameObjectWithTag("GameController").GetComponent<DataCollector>();
		m_debug = false;
	}
	
	// Update is called once per frame
	void Update () {
		m_behav_tree.act();

		m_fitness += (1/(m_logger.getByType(EObjectTypes.PLAYER)[0].transform.position - gameObject.transform.position).magnitude)*Time.deltaTime;
	}

	public void Initalize(EvoObject p_evo, ObjectLogger p_logger, int p_id){
		m_creature_id = p_id;

		//Setup stats with DNA
		m_stats = new Dictionary<ETrait, StatTuple>();
		
		DNA dna = p_evo.getDNA();

		//All passed in float are between 1 and 10. They need converting for proper values
		float attack = dna.getTraitValue(ETrait.ATTACK)/2;
		m_stats[ETrait.ATTACK] = new StatTuple(attack, attack);

		float defense = dna.getTraitValue(ETrait.DEFENSE)/5;
		m_stats[ETrait.DEFENSE] = new StatTuple(defense, defense);

		float speed = dna.getTraitValue(ETrait.SPEED);
		m_stats[ETrait.SPEED] = new StatTuple(speed, speed);

		float hp = dna.getTraitValue(ETrait.HP)*2;
		m_stats[ETrait.HP] = new StatTuple(hp, hp);

		m_dna = dna;

		//Setup Logger
		m_logger = p_logger;
		m_logger.log(gameObject, EObjectTypes.ENEMY);

		//Setup Behaviour
		m_behav_tree = new BehaviourTree(p_logger, gameObject, p_evo.GetBehaviour());
		m_behav_dna = p_evo.GetBehaviour();
	}

	public void damage(float p_damage){
		float damage = p_damage - m_stats[ETrait.DEFENSE].m_current <= 0.5f? 0.5f: p_damage-m_stats[ETrait.DEFENSE].m_current;
		
		m_stats[ETrait.HP].m_current -= damage;
		
		if(m_stats[ETrait.HP].m_current <= 0){
			//m_evolution_controller.addDNA(new EvoObject(m_dna.clone(), m_behav_dna.clone()), m_fitness);
			m_logger.unlog(gameObject, EObjectTypes.ENEMY);

			m_data.recordData(m_dna, m_creature_id, m_fitness);

			Debug.Log(m_fitness);

			Destroy(gameObject);
		}
	}	

	public float getTrait(ETrait p_trait, bool p_want_current ){
		return p_want_current ? m_stats[p_trait].m_current : m_stats[p_trait].m_total;
	}

	public float getForward(){
		return m_forward;
	}
}
