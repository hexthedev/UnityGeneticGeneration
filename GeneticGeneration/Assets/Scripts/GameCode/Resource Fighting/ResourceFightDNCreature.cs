using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Genetic.Base;
using Genetic.Composite;
using Genetic.Traits.Base;

using JTools.Events;

using JTools.Calc.Base;
using JTools.Calc.Vectors;
using JTools.Calc.ActiavationFunctions;

using JTools.DataStructures.ObjectLogger;
using JTools.DataStructures.CooldownLogger;
using JTools.DataStructures.LimitedNumber;
using JTools.DataStructures.PriorityList;

using JTools.Prototyping.LineCreator;

public class ResourceFightDNCreature : AController, IBrainInit, IDamagable {

	//Required Outside Objects
	public GameObject m_bullet;


	//Global References
	private ResourceFightGameController m_controller;

	//DNA, Brain and Traits
	private MindBodyDNA<ResourceFightDNCreature> m_dna;
	private Dictionary<string, float> m_traits;
	private IBrain m_brain;

	//Fitness
	private float m_fitness = 0;

	//Instance Objects and values
	private Vector2 m_forward;
	private Rigidbody2D m_rb;
	private TimeoutEventManager m_tm;
	private IntervalEventManager m_im;
	private PriorityList m_actions;
	private CooldownLogger m_cooldowns;
	private LimitedNumber m_health;
	private LimitedNumber m_energy;

	private float m_sense_angle;		//Senses
	private float m_sense_proximity;


	private bool m_brain_stop;	//Stops Decision net


	bool m_is_initialized = false;

	//Goal
 

	//----------------------------------------------------------
	//Unity Callbacks

  // Use this for initialization
  void Start () {
		m_rb = gameObject.GetComponent<Rigidbody2D>();
		m_tm = new TimeoutEventManager();
		m_im = new IntervalEventManager();
		m_actions = new PriorityList();
		m_cooldowns = new CooldownLogger();
		m_sense_angle = 30f;
		m_sense_proximity = 6f;
		m_brain_stop = false;

		m_cooldowns.activate("SENSE", 0);
	}
	
	// Using Fixed update because it's easy to speed up. When in real time, should probably split decision making into Update() from action activation in FixedUpdate()
	protected override void FixedUpdate () {
		
		if(!m_is_initialized) Debug.LogError("Creatures require initilization after Instantiation");

		//Tick the timeout event manager
		m_tm.tick(Time.fixedDeltaTime);
		m_im.tick(Time.fixedDeltaTime);
		m_cooldowns.tickAll(Time.fixedDeltaTime);

		//Set forward to correct forward vector
		m_forward = Vector2Calc.fromAngle(gameObject.transform.rotation.eulerAngles.z+90);
		
		// if(m_cooldowns.isCooldownOver("SHOOT")) shoot();
		// if(m_cooldowns.isCooldownOver("SENSE")) sense("BULLET");
		if(m_cooldowns.isCooldownOver("GATHER")) gather();

		if(m_energy.isMin()) damage(Time.fixedDeltaTime);
		
		if(!m_brain_stop){
			//Call fixed update of Controller to do one brain iteration
			base.FixedUpdate();

			//Actiavte and flush the actions priority list
			m_actions.activate();
			m_actions.flush();
		}
		
		//Update fitness for this frame
		m_fitness += fitnessUpdate();
	}

	//----------------------------------------------------------
	//Construction

	public void Initialize(MindBodyDNA<ResourceFightDNCreature> p_dna, ResourceFightGameController p_controller){
		m_is_initialized = true;
		ObjectLogger.log( gameObject, "CREATURE" );

		m_dna = p_dna.Clone();

		MindBody mindbody = p_dna.express(this);

		m_traits = mindbody.m_body;
		InitializeBrain(mindbody.m_mind);

		m_health = new LimitedNumber( m_traits["HEALTH"]*10, new Range<float>(0, m_traits["HEALTH"]*10) );
		m_energy = new LimitedNumber( m_traits["ENERGY"]*20, new Range<float>(0, m_traits["ENERGY"]*20) );

		m_controller = p_controller;
	}

  public void InitializeBrain(IBrain p_brain)
  {
    m_brain = p_brain;
  }


	//----------------------------------------------------------
	// Lifetime Functions

	protected override void act()
  {
    m_brain.brainAction();
  }

  public void damage(float p_damage)
  {
		if(p_damage > 0) m_health.add(-p_damage);
		if(m_health.isMin()) die();
  }

	//Log fitness and Destroy the game object
	private void die(){
		m_controller.logDNA(m_dna, m_fitness);
		ObjectLogger.unlog( gameObject, "CREATURE" );
		Destroy(gameObject);
	}


	//----------------------------------------------------------
	// Concrete Actions
	private void shoot(){
		if(m_traits["ATTACKSPEED"] < 0) {
			m_cooldowns.activate("SHOOT", float.PositiveInfinity);
			return;
		}

		m_cooldowns.activate("SHOOT", m_traits["ATTACKSPEED"]/2);
		Bullet bullet =  Instantiate(m_bullet, transform.position + Vector3Calc.fromVec2(m_forward)*0.24f, transform.rotation ).GetComponent<Bullet>();
		bullet.Initalize(m_forward, m_traits["DAMAGE"], gameObject);

		m_energy.add(-10f);
		Debug.Log(m_energy);
	}

	private GameObject[] sense(string p_type){
		GameObject[] all_of_type = ObjectLogger.getByType(p_type);

		List<GameObject> in_arc = new List<GameObject>();

		foreach(GameObject ob in all_of_type){
			
			Vector3 diff = ob.transform.position - gameObject.transform.position;
			
			if(Vector2Calc.checkAngle(diff, m_forward, m_sense_angle) && diff.magnitude <= m_sense_proximity){
				in_arc.Add(ob);
			}
		}

		// foreach(GameObject ob in in_arc){
		// 	Debug.DrawLine(gameObject.transform.position, ob.transform.position, Color.cyan, 1f);
		// }

		return in_arc.ToArray();
	}

	private bool canGather(){
		GameObject[] obs = sense("RESOURCE");

		foreach(GameObject o in obs){
			if( Vector2Calc.proximity( gameObject.transform.position, o.transform.position ) < 1f ){
				return true;
			}
		}

		return false;
	}

	private void gather(){
		GameObject[] obs = sense("RESOURCE");
		
		List<GameObject> resources = new List<GameObject>();
		Vector3 this_position = gameObject.transform.position;

		foreach(GameObject o in obs){
			if( Vector2Calc.proximity( this_position, o.transform.position ) < 4f ){
				resources.Add(o);
			}
		}

		if(resources.Count == 0) return;

		float closeness = float.PositiveInfinity;
		GameObject to_harvest = null;

		foreach(GameObject o in resources){
			float proximity = Vector2Calc.proximity( this_position, o.transform.position );
			if(!(proximity < closeness)) continue;
			closeness = proximity;
			to_harvest = o;
		}

		m_brain_stop = true;
		m_cooldowns.activate("GATHER", 2f);

		GameObject line = LineCreator.createLine( this_position, to_harvest.transform.position, Color.green, 0.01f );
		DIntervalListener energy_suck = () => {			m_energy.add(0.2f); Debug.Log(m_energy);		};

		m_im.addListener(0.1f, energy_suck );

		m_tm.addTimeout(1f, ()=> {
			m_im.removeListener(0.1f, energy_suck);
			m_brain_stop = false;
			Destroy(line);
		});

	}






	//----------------------------------------------------------
	// Fitness

	private float fitnessUpdate(){
		return 0;
	}












	//----------------------------------------------------------
	//INPUTS
	public static DInputFactory<ResourceFightDNCreature>[] getInputFactorys(){
		return new DInputFactory<ResourceFightDNCreature>[] {one_input};
	}

	public static DInput zero_input(ResourceFightDNCreature p_cre){
			return () => {
				return 0;
			};
	}

	public static DInput one_input(ResourceFightDNCreature p_cre){
			return () => {
				return 0;
			};
	}




	//----------------------------------------------------------
	//OUTPUTS
	public static DOutputFactory<ResourceFightDNCreature>[] getOutputFactorys(){
		return new DOutputFactory<ResourceFightDNCreature>[] {rotate_left};
	}

	public static DOutput zero_output(ResourceFightDNCreature p_cre){
			return (float p_input) => {
				return;
			};
	}

	public static DOutput rotate_left(ResourceFightDNCreature p_cre){
		return (float p_value) => { 			
			p_cre.m_actions.add("Rotate", p_value, () => { p_cre.transform.Rotate(0,0,-1);});
		};
	}


}
