using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Genetic.Base;
using Genetic.Composite;
using Genetic.Traits.Base;

using JTools.Interfaces;

using JTools.Events;

using JTools.Calc.Base;
using JTools.Calc.Vectors;
using JTools.Calc.Bool;
using JTools.Calc.ActiavationFunctions;

using JTools.DataStructures.ObjectLogger;
using JTools.DataStructures.CooldownLogger;
using JTools.DataStructures.LimitedNumber;
using JTools.DataStructures.PriorityList;

using JTools.Prototyping.LineCreator;

public class ResourceFightDNCreature : AController, IBrainInit, IDamagable
{

  //Required Outside Objects
  public GameObject m_bullet;
  public SpriteRenderer m_renderer;


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

  //Trait translations and useful info
  private LimitedNumber m_health;
  private LimitedNumber m_energy;

  private float m_speed;
  private float m_damage;
  private float m_attack_speed;

  private float m_sense_angle;    //Senses
  private float m_sense_proximity;
  private bool m_brain_stop;  //Stops Decision net


  bool m_is_initialized = false;
  bool m_decision_time = true;

  //Goal


  //----------------------------------------------------------
  //Unity Callbacks

  // Use this for initialization
  void Start()
  {
    m_rb = gameObject.GetComponent<Rigidbody2D>();
    m_renderer = gameObject.GetComponent<SpriteRenderer>();
    m_tm = new TimeoutEventManager();
    m_im = new IntervalEventManager();
    m_actions = new PriorityList();
    m_cooldowns = new CooldownLogger();
    m_sense_angle = 30f;
    m_sense_proximity = 6f;
    m_brain_stop = false;

    m_cooldowns.activate("SENSE", 0);

    m_im.addListener(0.1f, () => {
      m_decision_time = true;
    });
  }

  // Using Fixed update because it's easy to speed up. When in real time, should probably split decision making into Update() from action activation in FixedUpdate()
  protected override void FixedUpdate()
  {

    if (!m_is_initialized) Debug.LogError("Creatures require initilization after Instantiation");

    //Tick the timeout event manager
    m_tm.tick(Time.fixedDeltaTime);
    m_im.tick(Time.fixedDeltaTime);
    m_cooldowns.tickAll(Time.fixedDeltaTime);

    //Set forward to correct forward vector
    m_forward = Vector2Calc.fromAngle(gameObject.transform.rotation.eulerAngles.z + 90);

    if (!m_brain_stop)
    {

      if(m_decision_time){
        m_actions.flush();
        //Call fixed update of Controller to do one brain iteration
        base.FixedUpdate();
        m_decision_time = false;
      }

      //Actiavte and flush the actions priority list
      m_actions.activate();
    }

    //Update fitness for this frame
    m_fitness += fitnessUpdate();

    energyConsumer(Time.fixedDeltaTime * 0.2f);
    consumeMoveEnergy(Time.fixedDeltaTime);

    if(m_energy.isMin()){
      m_renderer.color = Color.red;
    } else {
      m_renderer.color = Color.green;
    }
  }

  //----------------------------------------------------------
  //Construction

  public void Initialize(MindBodyDNA<ResourceFightDNCreature> p_dna, ResourceFightGameController p_controller)
  {
    m_is_initialized = true;
    ObjectLogger.log(gameObject, "CREATURE");

    m_dna = p_dna.Clone();

    MindBody mindbody = p_dna.express(this);

    m_traits = mindbody.m_body;
    InitializeBrain(mindbody.m_mind);

    m_health = new LimitedNumber(m_traits["HEALTH"] * 5);
    m_energy = new LimitedNumber(m_traits["ENERGY"] * 10);
    m_speed = m_traits["SPEED"];
    m_damage = m_traits["DAMAGE"];
    m_attack_speed = m_traits["ATTACKSPEED"];

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
    if (p_damage > 0) m_health.add(-p_damage);
    if (m_health.isMin()) die();
  }

  //Log fitness and Destroy the game object
  private void die()
  {
    logFitness();
    ObjectLogger.unlog(gameObject, "CREATURE");
    Destroy(gameObject);
  }

  public void logFitness(){
    m_controller.logDNA(m_dna, m_fitness);
  }

  //----------------------------------------------------------
  // Concrete Actions
  private void shoot()
  {
    if(!m_cooldowns.isCooldownOver("SHOOT")) return;

    if (m_attack_speed < 0)
    {
      m_cooldowns.activate("SHOOT", float.PositiveInfinity);
      return;
    }

    m_cooldowns.activate("SHOOT", m_attack_speed);
    Bullet bullet = Instantiate(m_bullet, transform.position + Vector3Calc.fromVec2(m_forward) * 0.24f, transform.rotation).GetComponent<Bullet>();
    bullet.Initalize(m_forward, m_damage, gameObject);

    energyConsumer(2f);
  }

  private GameObject[] sense(string p_type)
  {
    GameObject[] all_of_type = ObjectLogger.getByType(p_type);

    List<GameObject> in_arc = new List<GameObject>();

    foreach (GameObject ob in all_of_type)
    {
			if(ob == null) continue;

      Vector3 diff = ob.transform.position - gameObject.transform.position;

      if (Vector2Calc.checkAngle(diff, m_forward, m_sense_angle) && diff.magnitude <= m_sense_proximity)
      {
        in_arc.Add(ob);
      }
    }

    // foreach(GameObject ob in in_arc){
    // 	Debug.DrawLine(gameObject.transform.position, ob.transform.position, Color.cyan, 1f);
    // }

    return in_arc.ToArray();
  }

  private GameObject senseClosest(string p_type)
  {
    GameObject[] sensed = sense(p_type);

    if (sensed.Length == 0) return null;

    GameObject closest = null;
    float closeness = float.PositiveInfinity;

    foreach (GameObject ob in sensed)
    {
      float prox = Vector2Calc.proximity(ob.transform.position, gameObject.transform.position);

      if (prox < closeness)
      {
        closest = ob;
        closeness = prox;
      }
    }

    return closest;
  }

  private bool canGather()
  {
    GameObject[] obs = sense("RESOURCE");

    foreach (GameObject o in obs)
    {
      if (Vector2Calc.proximity(gameObject.transform.position, o.transform.position) < 1f)
      {
        return true;
      }
    }

    return false;
  }

  private void gather()
  {
    //Get list of gatherable resources
    GameObject[] obs = sense("RESOURCE");

    List<GameObject> resources = new List<GameObject>();
    Vector3 this_position = gameObject.transform.position;

    foreach (GameObject o in obs)
    {
      if (Vector2Calc.proximity(this_position, o.transform.position) < 1f)
      {
        resources.Add(o);
      }
    }

    //If there are none, return
    if (resources.Count == 0) return;

    //Get the closest resource in gather arc
    float closeness = float.PositiveInfinity;
    GameObject to_harvest = null;

    foreach (GameObject o in resources)
    {
      float proximity = Vector2Calc.proximity(this_position, o.transform.position);
      if (!(proximity < closeness)) continue;
      closeness = proximity;
      to_harvest = o;
    }

    //Gather the resource
    m_brain_stop = true;
		m_rb.velocity = Vector3.zero;
		m_rb.angularVelocity = 0;
		m_actions.flush();
		
    m_cooldowns.activate("GATHER", 0.5f);

    GameObject line = LineCreator.createLine(this_position + (Vector3Calc.fromVec2(m_forward) * 0.1f), to_harvest.transform.position, Color.green, 0.05f , 0.5f);

    Resource harvesting = to_harvest.GetComponent<Resource>();
    DIntervalListener energy_suck = () =>
    {
      float harvest_power = m_energy.Max - m_energy.Value;
      if (harvest_power > 10f) harvest_power = 10f;
      if (to_harvest != null) m_energy.add(harvesting.collect(harvest_power));
    };

    m_im.addListener(0.1f, energy_suck);

    m_tm.addTimeout(0.5f, () =>
    {
      m_im.removeListener(0.1f, energy_suck);
      m_brain_stop = false;
    });
  }

  private void move(Vector2 p_move)
  {
    m_rb.velocity += p_move * Time.fixedDeltaTime;
    speedCorrection();
  }

  private void burstMove(Vector2 p_move, float p_burst_time)
  {
    m_rb.velocity = p_move;
    m_brain_stop = true;

    m_tm.addTimeout(p_burst_time, () =>
    {
      m_brain_stop = false;
    });
  }

  private void speedCorrection()
  {
    if (m_rb.velocity.magnitude > m_speed)
    {
      m_rb.velocity = m_rb.velocity.normalized * m_speed;
    }
  }

  public void consumeMoveEnergy(float p_time)
  {
    if (m_speed == 0) return;

    if (m_rb.velocity.magnitude <= m_speed)
    {
      energyConsumer((m_rb.velocity.magnitude / m_speed) * p_time);
    }
    else
    {
      energyConsumer(Mathf.Pow((m_rb.velocity.magnitude / m_speed), 2) * p_time);
    }
  }

  private void energyConsumer(float p_consume)
  {

    float over_depletion = m_energy.Value - p_consume;
    m_energy.add(-p_consume);

    if (over_depletion < 0)
    {
      damage(-over_depletion);
    }

  }




  //----------------------------------------------------------
  // Fitness

  private float fitnessUpdate()
  {
    return Time.fixedDeltaTime;
  }












  //----------------------------------------------------------
  //INPUTS
  public static DInputFactory<ResourceFightDNCreature>[] getInputFactorys()
  {
    return new DInputFactory<ResourceFightDNCreature>[] {
      creatureInShotline,
      isHungry, resourceInRange, resourceInSight,
      creatureInShotline, canShoot, isHurt, isStronger,
      bulletCollisionImminent, bulletsUpAhead, bulletInSight
    };
  }

  public static DInput zero_input(ResourceFightDNCreature p_cre)
  {
    return () =>
    {
      return 0;
    };
  }

  //Resource
  public static DInput isHungry(ResourceFightDNCreature p_cre)
  {
    DActivationFunction activate = ActivationFactory.generateSigmoid(1, 1, false, true, true);

    return () =>
    {
      //Debug.Log(p_cre.m_energy.Value + " : " + activate(p_cre.m_energy.Value/p_cre.m_energy.Max));
      return activate(p_cre.m_energy.Value / p_cre.m_energy.Max);
    };
  }

  public static DInputFactory<ResourceFightDNCreature> resourceInRange = isInRangeFactory("RESOURCE", 1f);
  public static DInputFactory<ResourceFightDNCreature> resourceInSight = isSightFactory("RESOURCE");


  //Shooting

  public static DInput creatureInShotline(ResourceFightDNCreature p_cre)
  {
    return () =>
    {
      RaycastHit2D hit = Physics2D.Raycast(p_cre.transform.position + Vector3Calc.fromVec2(p_cre.m_forward * 0.13f), p_cre.m_forward, 10f, 1 << 10);
      //Debug.DrawLine(p_cre.transform.position + Vector3Calc.fromVec2(p_cre.m_forward*0.13f) ,p_cre.transform.position+ Vector3Calc.fromVec2(p_cre.m_forward)*3, Color.red, 10f);
      return hit ? 1f : 0f;
    };
  }

  public static DInput canShoot(ResourceFightDNCreature p_cre)
  {
    return () =>
    {
      return p_cre.m_cooldowns.isCooldownOver("SHOOT") ? 1f : 0f;
    };
  }

  public static DInput isHurt(ResourceFightDNCreature p_cre)
  {
    DActivationFunction activate = ActivationFactory.generateSigmoid(1, 1, false, true, true);

    return () =>
    {
      return activate(p_cre.m_health.Value / p_cre.m_health.Max);
    };
  }

  public static DInput isStronger(ResourceFightDNCreature p_cre)
  {
    DActivationFunction activate = ActivationFactory.generateSigmoid(1f, 1, false, true, false);

    return () =>
    {
      GameObject closest = p_cre.senseClosest("CREATURE");

      if (closest == null) return 0;

      ResourceFightDNCreature enemy = closest.GetComponent<ResourceFightDNCreature>();

      float strength_check = (p_cre.m_health.Value + (p_cre.m_damage * 5)) / (enemy.m_health.Value + (enemy.m_damage * 5));

      //Debug.Log( "[" + strength_check +"] " +  activate( strength_check-0.5f ) );

      return activate(strength_check - 0.5f); //Your mesure of toughness/ enemy mesure of toughness set up so double enemy gives 0 and half enemy 1
    };
  }

  public static DInputFactory<ResourceFightDNCreature> creatureInSight = isSightFactory("CREATURE");







  //Dodging
  public static DInput bulletCollisionImminent(ResourceFightDNCreature p_cre)
  {
    DActivationFunction prox_activate = ActivationFactory.generateSigmoid(1.5f, 2, false, true, true);
    DActivationFunction angle_activate = ActivationFactory.generateSigmoid(10f, 2, false, true, true);
    DActivationFunction full_activate = ActivationFactory.generateSigmoid(1, 1, false, true, false);


    return () =>
    {
      GameObject[] bullets = p_cre.sense("BULLET");

      List<GameObject> bullets_close_and_coming_towards = new List<GameObject>();

      foreach (GameObject bul in bullets)
      {
        if (Vector2Calc.proximity(bul.transform.position, p_cre.transform.position) < 2 && Vector2Calc.checkAngle(p_cre.transform.position - bul.transform.position, bul.GetComponent<Rigidbody2D>().velocity, 10))
        {
          bullets_close_and_coming_towards.Add(bul);
        }
      }

      float activation = 0;

      foreach (GameObject bul in bullets_close_and_coming_towards)
      {

        float bul_prox = prox_activate(Vector2Calc.proximity(bul.transform.position, p_cre.transform.position) - 0.5f);
        float bul_angle = angle_activate(Mathf.Abs(Vector2Calc.getAngle(p_cre.transform.position - bul.transform.position, bul.GetComponent<Rigidbody2D>().velocity)));

        float bul_active = full_activate(bul_prox * bul_angle);
        if (bul_active > activation) activation = bul_active;
      }
      //if(activation >0 ) Debug.Log(activation);

      return activation;
    };
  }


  public static DInput bulletsUpAhead(ResourceFightDNCreature p_cre)
  {
    DActivationFunction activate = ActivationFactory.generateSigmoid(5, 1, false, true, false);

    return () =>
    {
      GameObject[] bullets = p_cre.sense("BULLET");
      // Debug.Log(bullets.Length + " : " + activate(bullets.Length));
      return activate(bullets.Length);
    };
  }

  public static DInputFactory<ResourceFightDNCreature> bulletInSight = isSightFactory("BULLET");






  //FACTORIES
  public static DInputFactory<ResourceFightDNCreature> isSightFactory(string p_type)
  {
    return (ResourceFightDNCreature p_cre) =>
    {
      return () =>
      {
        float isSight = p_cre.sense(p_type).Length > 0 ? 1f : 0f;
        // if(isSight == 1) Debug.Log( "Sighted " + p_type);
        return isSight;
      };
    };
  }

  public static DInputFactory<ResourceFightDNCreature> isInRangeFactory(string p_type, float p_range)
  {
    return (ResourceFightDNCreature p_cre) =>
    {
      return () =>
      {
        GameObject[] sensed = p_cre.sense(p_type);

        bool any_in_range = false;

        foreach (GameObject ob in sensed)
        {
          if (Vector2Calc.proximity(ob.transform.position, p_cre.transform.position) <= p_range)
          {
            any_in_range = true;
            break;
          }
        }

        // if (any_in_range) Debug.Log("InRange " + p_type);
        return any_in_range ? 1f : 0f;
      };
    };
  }









  //----------------------------------------------------------
  //OUTPUTS
  public static DOutputFactory<ResourceFightDNCreature>[] getOutputFactorys()
  {
    return new DOutputFactory<ResourceFightDNCreature>[] { 
      search_resource, gather_resource, dont_gather_resource,
			move_forward, 
			move_backwards,
			rotate_left, 
			rotate_right, 
      stop_rotation, 
			shoot, 
			dont_shoot,
      aim_at_nearest_creature,
      flee_from_nearest_creature};
  }

  public static DOutput move_forward(ResourceFightDNCreature p_cre)
  {
    return (float p_input) =>
    {
      p_cre.m_actions.add("MOVE", p_input, () => { p_cre.move(p_cre.m_forward*5f); });
    };
  }

  public static DOutput move_backwards(ResourceFightDNCreature p_cre)
  {
    return (float p_input) =>
    {
      p_cre.m_actions.add("MOVE", p_input, () => { p_cre.move(-p_cre.m_forward*5f); });
    };
  }

  public static DOutput rotate_left(ResourceFightDNCreature p_cre)
  {
    return (float p_input) =>
    {
      p_cre.m_actions.add("ROTATE", p_input, () => { p_cre.transform.Rotate(0, 0, -2); });
    };
  }

	public static DOutput rotate_right(ResourceFightDNCreature p_cre)
  {
    return (float p_input) =>
    {
      p_cre.m_actions.add("ROTATE", p_input, () => { p_cre.transform.Rotate(0, 0, 2); });
    };
  }

  public static DOutput stop_rotation(ResourceFightDNCreature p_cre)
  {
    return (float p_input) =>
    {
      p_cre.m_actions.add("ROTATE", p_input, () => { p_cre.m_rb.angularVelocity *= 0.85f; });
    };
  }

	public static DOutput shoot(ResourceFightDNCreature p_cre)
  {
    return (float p_input) =>
    {
      if(p_cre.m_cooldowns.isCooldownOver("SHOOT")){
				p_cre.m_actions.add("SHOOT", p_input, () => { p_cre.shoot(); });
			}			
    };
  }

	public static DOutput dont_shoot(ResourceFightDNCreature p_cre)
  {
    return (float p_input) =>
    {
			p_cre.m_actions.add("SHOOT", p_input, () => { return; });
    };
  }


	public static DOutput search_resource(ResourceFightDNCreature p_cre)
  {
    return (float p_input) =>
    {
			GameObject resource = p_cre.senseClosest("RESOURCE");

			if(resource){
				float angle = Vector2Calc.getAngle(p_cre.m_forward, resource.transform.position - p_cre.transform.position);
				p_cre.m_actions.add("ROTATE", p_input, () => { p_cre.transform.Rotate(0, 0, -2 * Mathf.Sign(angle)); });

				if(angle < 10f){
					p_cre.m_actions.add("MOVE", p_input, () => { p_cre.move( p_cre.m_forward ); });
				}
			} else {
				p_cre.m_actions.add("ROTATE", p_input, () => { p_cre.transform.Rotate(0, 0, 1); });
			}
				
    };
  }

	public static DOutput gather_resource(ResourceFightDNCreature p_cre)
  {
    return (float p_input) =>
    {
			p_cre.m_actions.add("GATHER", p_input, () => { p_cre.gather(); });
		};
  }

	public static DOutput dont_gather_resource(ResourceFightDNCreature p_cre)
  {
    return (float p_input) =>
    {
			p_cre.m_actions.add("GATHER", p_input, () => { return; });
		};
  }


  public static DOutput aim_at_nearest_creature(ResourceFightDNCreature p_cre)
  {
    return (float p_input) =>
    {
			GameObject creature = p_cre.senseClosest("CREATURE");

			if(creature){
				float angle = Vector2Calc.getAngle(p_cre.m_forward, creature.transform.position - p_cre.transform.position);
				p_cre.m_actions.add("ROTATE", p_input, () => { p_cre.transform.Rotate(0, 0, -2 * Mathf.Sign(angle)); });
			} else {
				return;
			}
				
    };
  }

  public static DOutput flee_from_nearest_creature(ResourceFightDNCreature p_cre)
  {
    return (float p_input) =>
    {
			GameObject creature = p_cre.senseClosest("CREATURE");

			if(creature){
				float angle = Vector2Calc.getAngle(p_cre.m_forward, creature.transform.position - p_cre.transform.position);
				p_cre.m_actions.add("ROTATE", p_input, () => { p_cre.transform.Rotate(0, 0, -2 * Mathf.Sign(angle)); });
        p_cre.m_actions.add("MOVE", p_input, () => { p_cre.move(p_cre.m_forward * -1f); } );
			} else {
				return;
			}
				
    };
  }

}
