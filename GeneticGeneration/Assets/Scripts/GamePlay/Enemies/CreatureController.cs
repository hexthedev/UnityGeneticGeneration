using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class CreatureController : MonoBehaviour, IDamagable {

	//Global Mono Objects
	private GameController m_game_controller;
	
	//Enemy Specific Objects
	private Rigidbody2D m_rb;

	//Time Based Events
	private TimeoutEventManager m_timeout;
	private IntervalEventManager m_interval;

	//DNA and BEHAVIOUR (this will change a lot)
	private DNA m_dna;
	private Dictionary<ETrait, StatTuple> m_stats;
	private NeuralNet m_brain;

	//BASIC INFO (this will change a lot)
	private int m_creature_id;
	private int m_species_id;
	private float m_fitness;
	
	// Use this for initialization
	void Awake () {
		//Setup global vars
		GameObject game_controller_object = GameObject.FindGameObjectWithTag("GameController");

		m_game_controller = game_controller_object.GetComponent<GameController>();

		ObjectLogger.log(gameObject, EObjectTypes.ENEMY);

		//Setup body Variables
		m_rb = gameObject.GetComponent<Rigidbody2D>();

		//Setup Time based events
		m_timeout = new TimeoutEventManager();
		m_interval = new IntervalEventManager();
		m_interval.addListener(3f, ()=> { m_game_controller.addDNA(m_dna, m_fitness, m_species_id); });

		gameObject.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f));
	}
	
	// Update is called once per frame
	void FixedUpdate () {		
		m_brain.propagate();

		m_fitness += Time.fixedDeltaTime * 1/(((ObjectLogger.getByTypeByDistance(EObjectTypes.PLAYER, gameObject.transform.position)[0].transform.position - gameObject.transform.position).magnitude)/2);

		m_timeout.tick(Time.fixedDeltaTime);
		m_interval.tick(Time.fixedDeltaTime);
	}

	public void Initalize(DNA p_dna, int p_creature_id, int p_species_id){
		m_dna = p_dna;

		m_stats = p_dna.expressBody();
		m_brain = p_dna.expressMind(this);

		m_creature_id = p_creature_id;
		m_species_id = p_species_id;
	}

	//OCCURENT EVENTS
	public void damage(float p_damage){
				
		if(p_damage < 0){
			return;
		}

		float damage = p_damage - m_stats[ETrait.DEFENSE].getCurrent();
		damage = damage < 0.5f ? 0.5f : damage;
		
//		Debug.Log(damage);

		m_stats[ETrait.HP].addToCurrent(-damage);
		
		if(m_stats[ETrait.HP].getCurrent() <= 0){
			death();
		}
	}	

	public void death(){
		//ON DEATH, creature pass on their fitness, and DNA only
		ObjectLogger.unlog(gameObject, EObjectTypes.ENEMY);

		m_game_controller.addDNA(m_dna, m_fitness, m_species_id);

		DataCollector.recordData(m_dna.dataCSVlog(m_creature_id, m_fitness));

		Destroy(gameObject);
	}

	//CREATURE SENSES
	public float sensetraitStatus(ETrait p_trait){
		return m_stats[p_trait].getCurrent()/m_stats[p_trait].getTotal();
	}

	public Vector3 sensePosition(){
		return gameObject.transform.position;
	}

	public bool senseExistsObject(EObjectTypes p_object, int p_order_by_proximity){
		return !(ObjectLogger.getByType(p_object).Length <= p_order_by_proximity);
	}

	public Vector3 senseNearestObjectPosition(EObjectTypes p_object){
				
		GameObject[] objects = ObjectLogger.getByTypeByDistance(p_object, gameObject.transform.position);
		
		if(objects.Length == 0)  return gameObject.transform.position; 

		if(p_object == EObjectTypes.ENEMY && objects.Length > 1) return objects[1].transform.position;

		return objects[0].transform.position;
	}

	public GameObject[] senseVisibleObjects(EObjectTypes p_object){
		return ObjectLogger.getByTypeByDistance(p_object, gameObject.transform.position);
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
