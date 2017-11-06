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


public class DecisionNetCreature : AController, IBrainInit {



	GameController m_controller;
	MindBodyDNA<DecisionNetCreature> m_dna;
	Dictionary<ETrait, float> m_traits;
	IBrain m_brain;

	float m_fitness = 0;


	Vector2 m_forward;
	Rigidbody2D m_rb;
	TimeoutEventManager m_tm;
	PriorityList m_actions;
	Line2D m_goalLine = new Line2D(new Vector2(0,15), new Vector2(1,-0.5f));


  // Use this for initialization
  void Start () {
		m_rb = gameObject.GetComponent<Rigidbody2D>();
		m_tm = new TimeoutEventManager();
		m_actions = new PriorityList();

		m_tm.addTimeout(3f, () => {
			die();
		});
	}
	
	// Update is called once per frame
	protected override void FixedUpdate () {
		Debug.DrawLine(m_goalLine.Point, m_goalLine.Point+m_goalLine.Direction*100f, Color.green, Time.fixedDeltaTime);
		Debug.DrawLine(m_goalLine.Point, m_goalLine.Point+m_goalLine.Direction*-100f, Color.green, Time.fixedDeltaTime);
		
		m_tm.tick(Time.fixedDeltaTime);
		m_forward = Vector2Calc.fromAngle(gameObject.transform.rotation.eulerAngles.z+90);
		base.FixedUpdate();
		m_actions.activate();
		m_actions.flush();
		
		m_fitness += fitnessUpdate();
	}

	private void die(){
		m_controller.logDNA(m_dna, m_fitness);
		Destroy(gameObject);
	}

	private float fitnessUpdate(){
		Vector3 creature_position = gameObject.transform.position;
		Vector2 proj = Line2D.projection(creature_position, m_goalLine);
		
		//Debug.Log(1/((proj-Vector2Calc.fromVector3(creature_position)).magnitude));

		return 1/((proj-Vector2Calc.fromVector3(creature_position)).magnitude);
	}


	protected override void act()
  {
    m_brain.brainAction();
  }

  public void InitializeBrain(IBrain p_brain)
  {
    m_brain = p_brain;
  }

	public void Initialize(MindBodyDNA<DecisionNetCreature> p_dna, GameController p_controller){
		m_dna = p_dna.Clone();

		MindBody mindbody = p_dna.express(this);

		m_traits = mindbody.m_body;
		InitializeBrain(mindbody.m_mind);

		m_controller = p_controller;
	}





	//INPUTS
	public static DInputFactory<DecisionNetCreature>[] getInputFactorys(){
		return new DInputFactory<DecisionNetCreature>[] {zeroInput, closenessToLine, closenessToLineFromForwardCast, lineIsRight, lineIsLeft};
	}

	public static DInputFactory<DecisionNetCreature> zeroInput = (DecisionNetCreature p_creature) => {
		return () => { 			
			return 0;
		};
	};

	public static DInputFactory<DecisionNetCreature> closenessToLine = (DecisionNetCreature p_creature) => {
		DActivationFunction activator = ActivationFactory.generateSigmoid(2, 1, false, true, true);
		
		return () => { 			
			Vector3 creature_position = p_creature.gameObject.transform.position;
			Vector2 proj = Line2D.projection(creature_position, p_creature.m_goalLine);

			//Debug.DrawLine(creature_position, proj, Color.red, 0.25f);

			return activator( (Vector2Calc.fromVector3(creature_position)-proj).magnitude );
		};
	};

	public static DInputFactory<DecisionNetCreature> closenessToLineFromForwardCast = (DecisionNetCreature p_creature) => {
		DActivationFunction activator = ActivationFactory.generateSigmoid(4, 1, false, true, true);

		return () => { 			
			Vector2 intersection = new Vector2();
			Vector3 creature_position = p_creature.gameObject.transform.position;
			Line2D.intersectsCastToLine(new Line2D(creature_position, Vector2Calc.fromVector3(creature_position)+p_creature.m_forward), p_creature.m_goalLine, ref intersection);
			//Debug.DrawLine(creature_position, intersection, Color.blue, 0.25f);			
			return activator( (Vector2Calc.fromVector3(creature_position)-intersection).magnitude );
		};
	};

	public static DInputFactory<DecisionNetCreature> lineIsRight = (DecisionNetCreature p_creature) => {
		return () => { 			
			Vector3 creature_position = p_creature.gameObject.transform.position;
			Vector2 projVector = Line2D.projection(creature_position, p_creature.m_goalLine)-Vector2Calc.fromVector3(creature_position);
			float direction = Mathf.Sign( Vector2Calc.getAngle(projVector, p_creature.m_forward ) );
			//if(direction == -1f) Debug.DrawLine(creature_position, Vector2Calc.fromVector3(creature_position)+(Vector2Calc.rotateDirectionVector(p_creature.m_forward, -90f)), Color.cyan, 0.1f) ;
			return direction == -1f? 1 : 0;
		};
	};

		public static DInputFactory<DecisionNetCreature> lineIsLeft = (DecisionNetCreature p_creature) => {
		return () => { 			
			Vector3 creature_position = p_creature.gameObject.transform.position;
			Vector2 projVector = Line2D.projection(creature_position, p_creature.m_goalLine)-Vector2Calc.fromVector3(creature_position);
			float direction = Mathf.Sign( Vector2Calc.getAngle(projVector, p_creature.m_forward ) );
			//if(direction == 1f) Debug.DrawLine(creature_position, Vector2Calc.fromVector3(creature_position)+(Vector2Calc.rotateDirectionVector(p_creature.m_forward, 90f)), Color.cyan, 0.1f) ;
			return direction == 1f? 1 : 0;
		};
	};










	//OUTPUTS
	public static DOutputFactory<DecisionNetCreature>[] getOutputFactorys(){
		return new DOutputFactory<DecisionNetCreature>[] {rotateLeft, rotateRight, rotateDont, moveForward, moveDont, moveStop, moveForward};
	}

	public static DOutputFactory<DecisionNetCreature> moveForward = (DecisionNetCreature p_creature) => {
		return (float p_value) => { 			
			p_creature.m_actions.add("Move", p_value, () => { p_creature.gameObject.GetComponent<Rigidbody2D>().velocity = p_creature.m_forward*p_creature.m_traits[ETrait.SPEED]; });
		};
	};

	public static DOutputFactory<DecisionNetCreature> moveDont = (DecisionNetCreature p_creature) => {
		return (float p_value) => { 			
			p_creature.m_actions.add("Move", p_value, () => {});
		};
	};

	public static DOutputFactory<DecisionNetCreature> moveStop = (DecisionNetCreature p_creature) => {
		return (float p_value) => { 			
			p_creature.m_actions.add("Move", p_value, () => {p_creature.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;});
		};
	};

	public static DOutputFactory<DecisionNetCreature> moveBackwards = (DecisionNetCreature p_creature) => {
		return (float p_value) => { 			
			p_creature.m_actions.add("Move", p_value, () => { p_creature.gameObject.GetComponent<Rigidbody2D>().velocity = p_creature.m_forward*p_creature.m_traits[ETrait.SPEED]*-0.5f; });
		};
	};

	public static DOutputFactory<DecisionNetCreature> rotateDont = (DecisionNetCreature p_creature) => {
		return (float p_value) => { 			
			p_creature.m_actions.add("Rotate", p_value, () => { });
		};
	};


	public static DOutputFactory<DecisionNetCreature> rotateLeft = (DecisionNetCreature p_creature) => {
		return (float p_value) => { 			
			p_creature.m_actions.add("Rotate", p_value, () => { p_creature.transform.Rotate(0,0,-10);});
		};
	};

		public static DOutputFactory<DecisionNetCreature> rotateRight = (DecisionNetCreature p_creature) => {
		return (float p_value) => { 			
			p_creature.m_actions.add("Rotate", p_value, () => { p_creature.transform.Rotate(0,0,10);});
		};
	};



}
