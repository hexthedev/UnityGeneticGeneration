using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using Genetic.Base;
using Genetic.Composite;
using Genetic.Traits.Base;

using JTools.Calc.Vectors;
using JTools.Events;
using JTools.Interfaces;
using JTools.DataStructures.PriorityList;
using JTools.Calc.Lines;
using JTools.Calc.ActiavationFunctions;

//T1 is concrete creature, T2 is the gamecontroller, T3 is the MindBodyDNA, T4 is the Concrete Mindbody, T5 is the AI datastructure
public abstract class ALineFollowingCreature<T1, T2, T3, T4, T5> : AController, IBrainInit
  where T1 : ALineFollowingCreature<T1, T2, T3, T4, T5>
  where T2 : ALineFollowingGameController<T1, T2, T3, T4, T5>
  where T3 : ADNA<T3>, IControllerExpressable<T1, T4>, ICloneable<T3>
  where T4 : AMindBody<T5>, IBrain
  where T5 : IBrain
{

  T2 m_controller;    

  //DNA, Brain and Traits
  T3 m_dna;
  Dictionary<string, float> m_traits;
  IBrain m_brain;

  //Fitness
  float m_fitness = 0;

  //Instance Objects
  Vector2 m_forward;
  Rigidbody2D m_rb;
  TimeoutEventManager m_tm;
  IntervalEventManager m_im;
  PriorityList m_actions;

  bool m_is_initialized = false;

  SpriteRenderer m_renderer;

  int fit_samples;
  int set_behaviour_samples;



  //----------------
  //Task Vars
  float m_fit_color;
  





  // Use this for initialization
  void Start()
  {
    m_rb = gameObject.GetComponent<Rigidbody2D>();
    m_tm = new TimeoutEventManager();
    m_actions = new PriorityList();

    m_renderer = gameObject.GetComponent<SpriteRenderer>();

    m_tm.addTimeout(lineFollowCONFIG.CreatureTimeAlive, () =>
    {
      die();
    });

    m_im = new IntervalEventManager();

    m_im.addListener(lineFollowCONFIG.CreatureDecisionLatency, () =>
    {
      m_actions.flush();
      //Flush actions, set neew behaviours
      set_behaviour_samples++;
      setBehaviours();
    });

    m_im.addListener(2f, () =>
    {
      // Debug.Log("Fit: " + fit_samples + ", Beh: " + set_behaviour_samples);
      fit_samples = 0;
      set_behaviour_samples = 0;
    });

    m_im.addListener(lineFollowCONFIG.CreatureFitnessLatency, fitnessUpdate);

    
  }

  // Update is called once per frame
  // Using Fixed update because it's easy to speed up. When in real time, should probably split decision making into Update() from action activation in FixedUpdate()
  void FixedUpdate()
  {
    if (!m_is_initialized) Debug.LogError("Creatures require initilization after Instantiation");

    //Tick Event Managers
    m_tm.tick(Time.fixedDeltaTime);
    m_im.tick(Time.fixedDeltaTime);

    //Set forward to correct forward vector
    m_forward = Vector2Calc.fromAngle(gameObject.transform.rotation.eulerAngles.z + 90);

    //Actiavte and flush the actions priority list
    m_actions.activate();
  }

  void Update(){
    //Tick the timeout event manager
    // m_tm.tick(Time.deltaTime);
    // m_im.tick(Time.deltaTime);
  }


  //----------------------------------------------------------
  //Construction
  protected abstract T1 getSelf();

  public void Initialize(T3 p_dna, T2 p_controller)
  {
    m_is_initialized = true;
    m_dna = p_dna.Clone();

    T4 mindbody = p_dna.express(getSelf());

    m_traits = mindbody.m_body;
    
    // Debug.Log(mindbody.m_mind);
    InitializeBrain(mindbody.m_mind);

    m_controller = p_controller;
  }

  public void InitializeBrain(IBrain p_brain)
  {
    m_brain = p_brain;
  }

  protected override void setBehaviours()
  {
    m_brain.brainAction();
  }

  //Log fitness and Destroy the game object
  private void die()
  {
    m_controller.logDNA(m_dna, m_fitness);
    Destroy(gameObject);
  }

  private void fitnessUpdate()
  {

    Vector3 creature_position = gameObject.transform.position;     



    Task t =Task.Factory.StartNew( ()=> {
      Vector2 proj = Line2D.projection(creature_position, m_controller.GoalLine);
      
      
      //Prox to Line
      // float lineProx = (9.8f-((proj - Vector2Calc.fromVector3(creature_position)).magnitude));
      // float fitness = Mathf.Sign(lineProx) > 0 ? Mathf.Pow( lineProx, 2 ) : -Mathf.Pow( lineProx, 2 );
      

      //Prox to Line + Distance From Origin
      // float lineProx = (9.8f-((proj - Vector2Calc.fromVector3(creature_position)).magnitude));
      // float originDist = (Vector2.zero - Vector2Calc.fromVector3(creature_position)).magnitude;
      // float fitness = Mathf.Sign(lineProx*originDist) > 0 ? Mathf.Pow( lineProx*originDist, 2 ) : -Mathf.Pow( lineProx*originDist, 2 );
      
      //Distance from origin only if within prox of line
      // float counting = (proj - Vector2Calc.fromVector3(creature_position)).magnitude > 1f ? 0 : 1;         
      // float originDist = (Vector2.zero - Vector2Calc.fromVector3(creature_position)).magnitude;
      // float fitness = counting * originDist;

      //Distance from origin only if within prox of line and only above x axis
      float counting = (proj - Vector2Calc.fromVector3(creature_position)).magnitude > 1f ? 0 : 1;       
      float negative = creature_position.x >= 0 ? 1 : -1;  
      float originDist = (Vector2.zero - Vector2Calc.fromVector3(creature_position)).magnitude;
      float fitness = counting * originDist * negative;

      fit_samples++;
      m_fit_color = fitness;
      m_fitness += fitness;
    } );

    // if(m_fit_color > 0){
    //   m_renderer.color = new Color(0,Mathf.Clamp(0.2f+0.8f*(m_fit_color/98),0,1),0,1);
    // } else {
    //   m_renderer.color = new Color(Mathf.Clamp(0.2f+(-0.8f)*(m_fit_color/98), 0, 1),0,0,1);
    // }

    //COLOR: Distance from origin only if within prox of line
    if(m_fit_color > 0f){
      m_renderer.color = new Color(0,Mathf.Clamp(0.2f+0.8f*(m_fit_color/16),0,1),0,1);
    } else if(m_fit_color < 0f) {
      m_renderer.color = new Color(Mathf.Clamp(0.2f+(-0.8f)*(m_fit_color/16), 0, 1),0,0,1);
    } else {
      m_renderer.color = new Color(0,0,0.5f,1);
    }



  }



  //----------------------------------------------------------
  //INPUTS
  public static DInputFactory<T1>[] getInputFactorys()
  {
    return new DInputFactory<T1>[] { zeroInput, closenessToLine, closenessToLineFromForwardCast, lineIsRight, lineIsLeft,xPosititive };
  }

  public static DInputFactory<T1> zeroInput = (T1 p_creature) =>
  {
    return () =>
    {
      return 0;
    };
  };

  public static DInputFactory<T1> xPosititive = (T1 p_creature) =>
  {
    return () =>
    {
      return p_creature.transform.position.x > 0? 1 : 0;
    };
  };


  public static DInputFactory<T1> closenessToLine = (T1 p_creature) =>
  {
    DActivationFunction activator = ActivationFactory.generateSigmoid(2, 1, false, true, true);

    return () =>
    {
      Vector3 creature_position = p_creature.gameObject.transform.position;
      Vector2 proj = Line2D.projection(creature_position, p_creature.m_controller.GoalLine);

      //Debug.DrawLine(creature_position, proj, Color.red, 0.25f);

      return activator((Vector2Calc.fromVector3(creature_position) - proj).magnitude);
    };
  };

  public static DInputFactory<T1> closenessToLineFromForwardCast = (T1 p_creature) =>
  {
    DActivationFunction activator = ActivationFactory.generateSigmoid(4, 1, false, true, true);

    return () =>
    {
      Vector2 intersection = new Vector2();
      Vector3 creature_position = p_creature.gameObject.transform.position;
      bool can_mesure = Line2D.intersectsCastToLine(new Line2D(creature_position, Vector2Calc.fromVector3(creature_position) + p_creature.m_forward), p_creature.m_controller.GoalLine, ref intersection);
      //Debug.DrawLine(creature_position, intersection, Color.blue, 0.25f);			
      return can_mesure ? activator((Vector2Calc.fromVector3(creature_position) - intersection).magnitude) : 0;
    };
  };

  public static DInputFactory<T1> lineIsRight = (T1 p_creature) =>
  {
    return () =>
    {
      Vector3 creature_position = p_creature.gameObject.transform.position;
      Vector2 projVector = Line2D.projection(creature_position, p_creature.m_controller.GoalLine) - Vector2Calc.fromVector3(creature_position);
      float direction = Mathf.Sign(Vector2Calc.getAngle(projVector, p_creature.m_forward));
      //if(direction == -1f) Debug.DrawLine(creature_position, Vector2Calc.fromVector3(creature_position)+(Vector2Calc.rotateDirectionVector(p_creature.m_forward, -90f)), Color.cyan, 0.1f) ;
      return direction == -1f ? 1 : 0;
    };
  };

  public static DInputFactory<T1> lineIsLeft = (T1 p_creature) =>
  {
    return () =>
    {
      Vector3 creature_position = p_creature.gameObject.transform.position;
      Vector2 projVector = Line2D.projection(creature_position, p_creature.m_controller.GoalLine) - Vector2Calc.fromVector3(creature_position);
      float direction = Mathf.Sign(Vector2Calc.getAngle(projVector, p_creature.m_forward));
      //if(direction == 1f) Debug.DrawLine(creature_position, Vector2Calc.fromVector3(creature_position)+(Vector2Calc.rotateDirectionVector(p_creature.m_forward, 90f)), Color.cyan, 0.1f) ;
      return direction == 1f ? 1 : 0;
    };
  };



  //----------------------------------------------------------
  //OUTPUTS
  public static DOutputFactory<T1>[] getOutputFactorys()
  {
    return new DOutputFactory<T1>[] { rotateLeft, rotateRight, rotateDont, moveForward, moveStop, moveDont };
  }

  public static DOutputFactory<T1> moveForward = (T1 p_creature) =>
  {
    return (float p_value) =>
    {
      p_creature.m_actions.add("Move", p_value, () => { p_creature.gameObject.GetComponent<Rigidbody2D>().velocity = p_creature.m_forward * p_creature.m_traits["SPEED"]; });
    };
  };

  public static DOutputFactory<T1> moveDont = (T1 p_creature) =>
  {
    return (float p_value) =>
    {
      p_creature.m_actions.add("Move", p_value, () => { });
    };
  };

  public static DOutputFactory<T1> moveStop = (T1 p_creature) =>
  {
    return (float p_value) =>
    {
      p_creature.m_actions.add("Move", p_value, () => { p_creature.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero; });
    };
  };

  public static DOutputFactory<T1> moveBackwards = (T1 p_creature) =>
  {
    return (float p_value) =>
    {
      p_creature.m_actions.add("Move", p_value, () => { p_creature.gameObject.GetComponent<Rigidbody2D>().velocity = p_creature.m_forward * p_creature.m_traits["SPEED"] * -1f; });
    };
  };

  public static DOutputFactory<T1> rotateDont = (T1 p_creature) =>
  {
    return (float p_value) =>
    {
      p_creature.m_actions.add("Rotate", p_value, () => { });
    };
  };


  public static DOutputFactory<T1> rotateLeft = (T1 p_creature) =>
  {
    return (float p_value) =>
    {
      p_creature.m_actions.add("Rotate", p_value, () => { p_creature.transform.Rotate(0, 0, -4); });
    };
  };

  public static DOutputFactory<T1> rotateRight = (T1 p_creature) =>
  {
    return (float p_value) =>
    {
      p_creature.m_actions.add("Rotate", p_value, () => { p_creature.transform.Rotate(0, 0, 4); });
    };
  };










}
