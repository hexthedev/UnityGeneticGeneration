// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Calc;
// using Calc.Vector;

// public class PlayerController : MonoBehaviour, IDamagable {
	
// 	public GameController m_game_controller;
// 	private EvolutionController m_evo;	
		
// 	public GameObject m_barrel_start;
// 	public GameObject m_barrel_end;

// 	public float m_hp = 100f;
// 	public float m_speed = 20;
// //	private Rigidbody2D m_rb;
// 	public GameObject m_bullet;

// 	public float shot_rate;
// 	public float m_damage;
// 	public float m_rotation_speed;
// 	private float m_current_angle = 0f;
// 	private Vector3 m_rotation_axis = new Vector3(0,0,1);	

// 	private bool m_can_shoot = true;

// 	public float m_damage_increase_time;
// //	private float m_damage_increase_timer = 0f;
	

// 	TimeoutEventManager m_timeout_manager = new TimeoutEventManager();
// 	IntervalEventManager m_interval_manager = new IntervalEventManager();

// 	// Use this for initialization
// 	void Start () {
// 	//	m_rb = gameObject.GetComponent<Rigidbody2D>();
// 		ObjectLogger.log(gameObject, EObjectTypes.PLAYER);

		
// 	}
	
// 	// Update is called once per frame
// 	void FixedUpdate () {
// 		gameObject.transform.position = Vector3.zero;

// 		m_timeout_manager.tick(Time.fixedDeltaTime);
// 		m_interval_manager.tick(Time.fixedDeltaTime);

// 		m_current_angle += 1*m_rotation_speed;
// 		gameObject.transform.rotation = Quaternion.AngleAxis(m_current_angle, m_rotation_axis);

// 		RaycastHit2D test = Physics2D.Raycast(m_barrel_start.transform.position, m_barrel_end.transform.position, 20f, 1 << 9);
// 		Debug.DrawLine(m_barrel_start.transform.position, (m_barrel_end.transform.position - m_barrel_start.transform.position)*20, Color.red, Time.fixedDeltaTime);


// 		if(test.collider != null && m_can_shoot) shoot(m_barrel_end.transform.position - m_barrel_start.transform.position); 
		
		
// 	}

// 	void shoot(Vector3 p_direction){
// 		Vector2 direction = p_direction.normalized;

// 		GameObject bullet = Instantiate(m_bullet, Vector2Calc.fromVector3(m_barrel_end.transform.position) + direction*0.5f, Quaternion.identity);	

// 		bullet.GetComponent<Bullet>().Initalize(direction, m_damage, "Player", m_game_controller);

// 		m_can_shoot = false;
// 		m_timeout_manager.addTimeout(shot_rate, () => {	m_can_shoot = true;	});
// 	}

//   public void damage(float damage)
//   {
//     m_hp -= damage;
// 		if(m_hp <= 0 ){
// 			ObjectLogger.unlog(gameObject, EObjectTypes.PLAYER);
// 			Destroy(gameObject);
// 		}
//   }
// }
