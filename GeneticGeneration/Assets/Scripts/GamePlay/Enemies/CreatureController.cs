using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class CreatureController : MonoBehaviour, IDamagable {

	//Global Mono Objects
	public GameController m_game_controller;
	private ObjectLogger m_logger;
	private EvolutionController m_evolution_controller;
	private DataCollector m_data;
	
	//Enemy Specific Objects
	private Rigidbody2D m_rb;

	//Time Based Events
	private TimeoutEventManager m_timeout;
	private IntervalEventManager m_interval;

	//DNA and BEHAVIOUR (this will change a lot)
	private PhysicalDNA m_dna;
	private Dictionary<ETrait, StatTuple> m_stats;
	private NeuralNet m_brain;

	//BASIC INFO (this will change a lot)
	private int m_creature_id;
	private float m_fitness;
	
	// Use this for initialization
	void Awake () {
		//Setup global vars
		GameObject game_controller_object = GameObject.FindGameObjectWithTag("GameController");

		m_game_controller = game_controller_object.GetComponent<GameController>();
		m_logger = game_controller_object.GetComponent<ObjectLogger>();
		m_evolution_controller = game_controller_object.GetComponent<EvolutionController>();
		m_data = game_controller_object.GetComponent<DataCollector>();

		//Setup body Variables
		m_rb = gameObject.GetComponent<Rigidbody2D>();

		//Setup Time based events
		m_timeout = new TimeoutEventManager();
		m_interval = new IntervalEventManager();
	}
	
	// Update is called once per frame
	void Update () {
		m_brain.propagate();

		m_fitness += (1/Mathf.Pow((m_logger.getByType(EObjectTypes.PLAYER)[0].transform.position - gameObject.transform.position).magnitude,2))*Time.deltaTime*m_game_controller.m_game_speed;

		m_timeout.tick(Time.deltaTime*m_game_controller.m_game_speed);
		m_interval.tick(Time.deltaTime*m_game_controller.m_game_speed);
	}

	public void Initalize(DNA p_evo, int p_id){
		//Creatures are given an ID on itialization

		//Creatures are given two sets of DNA. Stats DNA and Brain DNA. PACKAGE?
		//Creatures setup up their body with stats
		//Creature set up their behaviour with brain
		//--Brains are given the EnemyController
	}


	//OCCURENT EVENTS
	public void damage(float p_damage){
		if(p_damage < 0){
			return;
		}

		float damage = p_damage - m_stats[ETrait.DEFENSE].getCurrent();
		damage = damage < 0.5f ? 0.5f : damage;
		
		m_stats[ETrait.HP].addToCurrent(-damage);
		
		if(m_stats[ETrait.HP].getCurrent() <= 0){
			death();
		}
	}	

	public void death(){
		//ON DEATH, creature pass on their fitness, and DNA only
		
		Destroy(gameObject);
	}

	//CREATURE SENSES
	public float sensetraitStatus(ETrait p_trait){
		return m_stats[p_trait].getCurrent()/m_stats[p_trait].getTotal();
	}

	public Vector3 sensePosition(){
		return gameObject.transform.position;
	}

	public Vector3 senseNearestObjectPosition(EObjectTypes p_object){
		return m_logger.getByType(p_object)[0].transform.position;
	}

	public GameObject[] senseVisibleObjects(EObjectTypes p_object){
		return m_logger.getByType(p_object);
	}

	public Quaternion senseRotation(){
		return gameObject.transform.rotation;
	}


	//CREATURE CONTROLS
	public void moveForce(Vector2 p_change){
		//SCAFF
		if(m_stats == null){
			m_stats = new Dictionary<ETrait, StatTuple>();
			m_stats.Add(ETrait.SPEED, new StatTuple(5,5));
		}
		
		float total_speed = m_stats[ETrait.SPEED].getTotal();

		m_rb.velocity += p_change*(total_speed/10f);

		if(m_rb.velocity.magnitude > total_speed){
			m_rb.velocity = m_rb.velocity.normalized * total_speed;
			m_stats[ETrait.SPEED].setCurrent(total_speed);
		} else {
			m_stats[ETrait.SPEED].setCurrent(m_rb.velocity.magnitude);
		}
	}
}
