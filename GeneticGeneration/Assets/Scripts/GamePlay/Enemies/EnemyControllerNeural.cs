using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class EnemyControllerNeural : MonoBehaviour, IDamagable {

	//Global Mono Objects
	public GameController m_controller;
	private ObjectLogger m_logger;
	private EvolutionController m_evolution_controller;
	private DataCollector m_data;
	private Rigidbody2D m_rb;

	//DNA and BEHAVIOUR
	private DNA m_dna;
	private BehaviourDNA m_behav_dna;
	private Dictionary<ETrait, StatTuple> m_stats;

	private NeuralNet m_brain;

	//BASIC INFO
	private int m_creature_id;
	private float m_forward = -90;
	private float m_fitness;
	public float m_fitness_threshold;
	public bool m_debug;

	public float m_energy = 5;
	private TextMesh m_energy_text;

	private Ticker m_tick;

	


	// Use this for initialization
	void Awake () {
		m_controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		m_rb = gameObject.GetComponent<Rigidbody2D>();

		m_evolution_controller = m_controller.GetComponent<EvolutionController>();
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		m_data = GameObject.FindGameObjectWithTag("GameController").GetComponent<DataCollector>();
		m_debug = false;

		m_energy_text = gameObject.transform.GetChild(0).GetComponent<TextMesh>();
		m_energy_text.text = "" + m_energy;

		m_tick = new Ticker();
		m_tick.addListener(1f, () => { 
			m_energy -= 1; 
			m_energy_text.text = "" + Mathf.Round(m_energy); 
			if(m_energy <= 0) { death(); }
			}
		);

		m_logger = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectLogger>();

		m_brain = new NeuralNet(new NeuralDNA(), gameObject, m_logger, this);
	}
	
	// Update is called once per frame
	void Update () {
		m_brain.propagate();

		m_fitness += (1/Mathf.Pow((m_logger.getByType(EObjectTypes.PLAYER)[0].transform.position - gameObject.transform.position).magnitude,2))*Time.deltaTime*m_controller.m_game_speed;

		m_tick.tick(Time.deltaTime*m_controller.m_game_speed);

		m_energy += Time.deltaTime * m_controller.m_game_speed * gameObject.GetComponent<Rigidbody2D>().velocity.magnitude/2;
		//damage((1 - gameObject.GetComponent<Rigidbody2D>().velocity.magnitude)*Time.deltaTime);

		if(m_controller.m_reset_speed == true){
			float speed = m_dna.getTraitValue(ETrait.SPEED)*m_controller.m_game_speed;
			m_stats[ETrait.SPEED] = new StatTuple(speed, speed);
		}
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

		float speed = dna.getTraitValue(ETrait.SPEED)*m_controller.m_game_speed;
		m_stats[ETrait.SPEED] = new StatTuple(speed, speed);

		float hp = dna.getTraitValue(ETrait.HP)*2;
		m_stats[ETrait.HP] = new StatTuple(hp, hp);

		m_dna = dna;

		//Setup Logger
		// m_logger = p_logger;
		m_logger.log(gameObject, EObjectTypes.ENEMY);

		//Setup Behaviour
		m_behav_dna = p_evo.GetBehaviour();
	}

	public void damage(float p_damage){
		if(p_damage < 0){
			return;
		}

		float damage = p_damage - m_stats[ETrait.DEFENSE].m_current <= 0.5f? 0.5f: p_damage-m_stats[ETrait.DEFENSE].m_current;
		
		m_stats[ETrait.HP].m_current -= damage;
		
		if(m_stats[ETrait.HP].m_current <= 0){
			death();
		}
	}	

	public void death(){
		float speed = m_dna.getTraitValue(ETrait.SPEED);
		m_stats[ETrait.SPEED] = new StatTuple(speed, speed);

		m_evolution_controller.addDNA(new EvoObject(m_dna.clone(), m_behav_dna.clone()), m_fitness);
		m_logger.unlog(gameObject, EObjectTypes.ENEMY);

		m_data.recordData(m_dna, m_creature_id, m_fitness);

		// Debug.Log(m_fitness);

		Destroy(gameObject);
	}

	public float getTrait(ETrait p_trait, bool p_want_current ){
		return p_want_current ? m_stats[p_trait].m_current : m_stats[p_trait].m_total;
	}

	public float getForward(){
		return m_forward;
	}

	public void changeVelocity(Vector2 p_change){
		m_rb.velocity += p_change;
	}
}
