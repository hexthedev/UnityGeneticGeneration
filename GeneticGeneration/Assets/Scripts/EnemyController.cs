using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class EnemyController : MonoBehaviour {

	private float m_attack;
	private float m_defense;
	private float m_speed;
	private float m_hp;

	private float m_forward = -90;

	private BehaviourNode m_current_behav;

	// Use this for initialization
	void Start () {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		
		IAction[] move = {
			new MoveAction(3f, 1f, new TowardsPlayerDirection(player, gameObject)), 
			new MoveAction(1f, 2f, new TowardsPlayerDirection(player, gameObject))  
		};

		IAction[] rotate = {
			new RotateAction(new TowardsPlayerDirection(player, gameObject), m_forward, 3f) 
		};
		
		m_current_behav = new BehaviourNode( null, ActionSequence.emptySequence(), new PointingAtDetector(gameObject, m_forward, player, 10f) );

		m_current_behav.addChild(true, new ActionSequence(move), null);
		m_current_behav.addChild(false, new ActionSequence(rotate), null);

	}
	
	// Update is called once per frame
	void Update () {
	
		m_current_behav = m_current_behav.act(gameObject);

	}

	public void Initalize(float p_attack, float p_defense, float p_speed, float p_hp){
		//All passed in float are between 1 and 10. They need converting for proper values
		m_attack = p_attack;
		m_defense = p_defense/3;
		m_speed = p_speed;
		m_hp = p_hp*3;
	}


	// private Vector2 forwardVector(){
	// 	return VectorCalc.fromAngle(gameObject.transform.rotation.eulerAngles.z + m_forward);
	// }

	void OnCollisionEnter2D(Collision2D coll) {
		Destroy(gameObject);
	}

	
}
