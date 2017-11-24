using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Genetic.Base;
using Genetic.Composite;
using Genetic.Traits.Base;

using JTools.Calc.Vectors;
using JTools.Events;
using JTools.DataStructures.PriorityList;
using JTools.Calc.Lines;
using JTools.Calc.ActiavationFunctions;


public class LineFollowingDTCreature : AController, IBrainInit {

	//Global References
	LineFollowingDTGameController m_controller;

	//DNA, Brain and Traits
	MindBodyDTDNA<LineFollowingDTCreature> m_dna;
	Dictionary<string, float> m_traits;
	IBrain m_brain;

	//Fitness
	float m_fitness = 0;

	//Instance Objects
	Vector2 m_forward;
	Rigidbody2D m_rb;
	TimeoutEventManager m_tm;
	PriorityList m_actions;

	bool m_is_initialized = false;

	//Goal
	Line2D m_goalLine;

	//----------------------------------------------------------
	//Unity Callbacks

  // Use this for initialization
  void Start () {
		m_rb = gameObject.GetComponent<Rigidbody2D>();
		m_tm = new TimeoutEventManager();
		m_actions = new PriorityList();

		m_tm.addTimeout(3f, () => {
			die();
		});
	}
	
	// Using Fixed update because it's easy to speed up. When in real time, should probably split decision making into Update() from action activation in FixedUpdate()
	protected override void FixedUpdate () {
		if(!m_is_initialized) Debug.LogError("Creatures require initilization after Instantiation");

		//Draw the line we're following
		Debug.DrawLine(m_goalLine.Point, m_goalLine.Point+m_goalLine.Direction*100f, Color.green, Time.fixedDeltaTime);
		Debug.DrawLine(m_goalLine.Point, m_goalLine.Point+m_goalLine.Direction*-100f, Color.green, Time.fixedDeltaTime);
		
		//Tick the timeout event manager
		m_tm.tick(Time.fixedDeltaTime);

		//Set forward to correct forward vector
		m_forward = Vector2Calc.fromAngle(gameObject.transform.rotation.eulerAngles.z+90);
		
		//Call fixed update of Controller to do one brain iteration
		base.FixedUpdate();

		//Actiavte and flush the actions priority list
		m_actions.activate();
		m_actions.flush();
		
		//Update fitness for this frame
		m_fitness += fitnessUpdate();
	}

	//----------------------------------------------------------
	//Construction

	public void Initialize(MindBodyDTDNA<LineFollowingDTCreature> p_dna, LineFollowingDTGameController p_controller, Line2D p_goalLine){
		m_is_initialized = true;
		m_dna = p_dna.Clone();

		MindBodyDT mindbody = p_dna.express(this);

		m_traits = mindbody.m_body;
		InitializeBrain(mindbody.m_mind);

		m_controller = p_controller;

		m_goalLine = p_goalLine;
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

	//Log fitness and Destroy the game object
	private void die(){
		m_controller.logDNA(m_dna, m_fitness);
		Destroy(gameObject);
	}


	//----------------------------------------------------------
	// Fitness

	private float fitnessUpdate(){
		Vector3 creature_position = gameObject.transform.position;
		Vector2 proj = Line2D.projection(creature_position, m_goalLine);
		
		//Debug.Log(1/((proj-Vector2Calc.fromVector3(creature_position)).magnitude));

		return 1/((proj-Vector2Calc.fromVector3(creature_position)).magnitude);
	}











	//----------------------------------------------------------
	//INPUTS
	public static DInputFactory<LineFollowingDTCreature>[] getInputFactorys(){
		return new DInputFactory<LineFollowingDTCreature>[] {zeroInput, closenessToLine, closenessToLineFromForwardCast, lineIsRight, lineIsLeft};
	}

	public static DInputFactory<LineFollowingDTCreature> zeroInput = (LineFollowingDTCreature p_creature) => {
		return () => { 			
			return 0;
		};
	};

	public static DInputFactory<LineFollowingDTCreature> closenessToLine = (LineFollowingDTCreature p_creature) => {
		DActivationFunction activator = ActivationFactory.generateSigmoid(2, 1, false, true, true);
		
		return () => { 			
			Vector3 creature_position = p_creature.gameObject.transform.position;
			Vector2 proj = Line2D.projection(creature_position, p_creature.m_goalLine);

			//Debug.DrawLine(creature_position, proj, Color.red, 0.25f);

			return activator( (Vector2Calc.fromVector3(creature_position)-proj).magnitude );
		};
	};

	public static DInputFactory<LineFollowingDTCreature> closenessToLineFromForwardCast = (LineFollowingDTCreature p_creature) => {
		DActivationFunction activator = ActivationFactory.generateSigmoid(4, 1, false, true, true);

		return () => { 			
			Vector2 intersection = new Vector2();
			Vector3 creature_position = p_creature.gameObject.transform.position;
			bool can_mesure =  Line2D.intersectsCastToLine(new Line2D(creature_position, Vector2Calc.fromVector3(creature_position)+p_creature.m_forward), p_creature.m_goalLine, ref intersection);
			//Debug.DrawLine(creature_position, intersection, Color.blue, 0.25f);			
			return can_mesure ? activator( (Vector2Calc.fromVector3(creature_position)-intersection).magnitude ) : 0;
		};
	};

	public static DInputFactory<LineFollowingDTCreature> lineIsRight = (LineFollowingDTCreature p_creature) => {
		return () => { 			
			Vector3 creature_position = p_creature.gameObject.transform.position;
			Vector2 projVector = Line2D.projection(creature_position, p_creature.m_goalLine)-Vector2Calc.fromVector3(creature_position);
			float direction = Mathf.Sign( Vector2Calc.getAngle(projVector, p_creature.m_forward ) );
			//if(direction == -1f) Debug.DrawLine(creature_position, Vector2Calc.fromVector3(creature_position)+(Vector2Calc.rotateDirectionVector(p_creature.m_forward, -90f)), Color.cyan, 0.1f) ;
			return direction == -1f? 1 : 0;
		};
	};

		public static DInputFactory<LineFollowingDTCreature> lineIsLeft = (LineFollowingDTCreature p_creature) => {
		return () => { 			
			Vector3 creature_position = p_creature.gameObject.transform.position;
			Vector2 projVector = Line2D.projection(creature_position, p_creature.m_goalLine)-Vector2Calc.fromVector3(creature_position);
			float direction = Mathf.Sign( Vector2Calc.getAngle(projVector, p_creature.m_forward ) );
			//if(direction == 1f) Debug.DrawLine(creature_position, Vector2Calc.fromVector3(creature_position)+(Vector2Calc.rotateDirectionVector(p_creature.m_forward, 90f)), Color.cyan, 0.1f) ;
			return direction == 1f? 1 : 0;
		};
	};



	//----------------------------------------------------------
	//OUTPUTS
	public static DOutputFactory<LineFollowingDTCreature>[] getOutputFactorys(){
		return new DOutputFactory<LineFollowingDTCreature>[] {rotateLeft, rotateRight, rotateDont, moveForward, moveDont, moveStop, moveBackwards};
	}

	public static DOutputFactory<LineFollowingDTCreature> moveForward = (LineFollowingDTCreature p_creature) => {
		return (float p_value) => { 			
			p_creature.m_actions.add("Move", p_value, () => { p_creature.gameObject.GetComponent<Rigidbody2D>().velocity = p_creature.m_forward*p_creature.m_traits["SPEED"]; });
		};
	};

	public static DOutputFactory<LineFollowingDTCreature> moveDont = (LineFollowingDTCreature p_creature) => {
		return (float p_value) => { 			
			p_creature.m_actions.add("Move", p_value, () => {});
		};
	};

	public static DOutputFactory<LineFollowingDTCreature> moveStop = (LineFollowingDTCreature p_creature) => {
		return (float p_value) => { 			
			p_creature.m_actions.add("Move", p_value, () => {p_creature.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;});
		};
	};

	public static DOutputFactory<LineFollowingDTCreature> moveBackwards = (LineFollowingDTCreature p_creature) => {
		return (float p_value) => { 			
			p_creature.m_actions.add("Move", p_value, () => { p_creature.gameObject.GetComponent<Rigidbody2D>().velocity = p_creature.m_forward*p_creature.m_traits["SPEED"]*-0.5f; });
		};
	};

	public static DOutputFactory<LineFollowingDTCreature> rotateDont = (LineFollowingDTCreature p_creature) => {
		return (float p_value) => { 			
			p_creature.m_actions.add("Rotate", p_value, () => { });
		};
	};


	public static DOutputFactory<LineFollowingDTCreature> rotateLeft = (LineFollowingDTCreature p_creature) => {
		return (float p_value) => { 			
			p_creature.m_actions.add("Rotate", p_value, () => { p_creature.transform.Rotate(0,0,-10);});
		};
	};

		public static DOutputFactory<LineFollowingDTCreature> rotateRight = (LineFollowingDTCreature p_creature) => {
		return (float p_value) => { 			
			p_creature.m_actions.add("Rotate", p_value, () => { p_creature.transform.Rotate(0,0,10);});
		};
	};



}
