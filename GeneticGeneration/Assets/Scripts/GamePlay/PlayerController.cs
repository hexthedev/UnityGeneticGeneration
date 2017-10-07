using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class PlayerController : MonoBehaviour, IDamagable {
	
	public GameController m_game_controller;
	private EvolutionController m_evo;	
	private ObjectLogger m_logger;
	
	public float m_hp = 100f;
	public float m_speed = 20;
	private Rigidbody2D m_rb;
	public GameObject m_bullet;

	private float shot_timer;
	public float shot_rate;
	public float m_damage;

	public float m_damage_increase_time;
	private float m_damage_increase_timer = 0f;

	// Use this for initialization
	void Start () {
		m_rb = gameObject.GetComponent<Rigidbody2D>();
		m_evo = m_game_controller.GetComponent<EvolutionController>();
		shot_timer = shot_rate;

		m_logger = m_game_controller.GetComponent<ObjectLogger>();
		m_logger.log(gameObject, EObjectTypes.PLAYER);
	}
	
	// Update is called once per frame
	void Update () {
		shot_timer += Time.deltaTime * m_game_controller.m_game_speed;
		m_damage_increase_timer += Time.deltaTime * m_game_controller.m_game_speed;

		//m_rb.velocity = playerVelocityUpdate();

		if((shot_timer > shot_rate) && Input.GetKey(KeyCode.Mouse0)){
			//shoot();
			shot_timer = 0;
		}

		if(Input.GetKeyDown(KeyCode.P)){
			m_logger.getByType(EObjectTypes.ENEMY);
		}

		if(Input.GetKeyDown(KeyCode.O)){
			m_logger.getAll();
		}

		if(m_damage_increase_timer >= m_damage_increase_time){
			m_damage_increase_timer = 0;
			m_damage += 1f;
			shot_rate *= 0.995f;
			m_evo.playerChangeFitMod();
		}


		/* 
		if(Input.GetKeyDown(KeyCode.Comma)){
			shot_rate -= 0.025f;
			m_evo.playerChangeFitMod();
		}

		if(Input.GetKeyDown(KeyCode.Period)){
			m_damage += 1f;
			m_evo.playerChangeFitMod();
		}*/

		m_rb.velocity += VectorCalc.CalcVec3to2((ArrayCalc.randomElement(m_logger.getByType(EObjectTypes.ENEMY)).transform.position - gameObject.transform.position).normalized * m_speed * m_game_controller.m_game_speed /10f);

		if(m_rb.velocity.magnitude > m_speed){
			m_rb.velocity = m_rb.velocity.normalized*m_speed;
		}

		if(shot_timer > shot_rate){
			
			float closest_distance = -1f;
			Vector3 closest_position = Vector3.zero;

			Vector3 player_position = gameObject.transform.position;

			foreach(GameObject obj in m_logger.getByType(EObjectTypes.ENEMY)){
				float test = (obj.transform.position - player_position).magnitude;
				
				if(closest_distance == -1f){
					closest_position = obj.transform.position;
					closest_distance = test;
				} else {
					
					if(test < closest_distance){
						closest_distance = test;
						closest_position = obj.transform.position;
					} 
				}				
			}

			if(!(closest_position == Vector3.zero)){
				Debug.DrawLine(gameObject.transform.position, closest_position, Color.red, 2f);
				shoot(closest_position);
			}

			shot_timer = 0;
			
		}



	}

	Vector2 playerVelocityUpdate(){

		Vector2 toReturn = Vector2.zero;

		if(Input.GetKey(KeyCode.A)){
			toReturn += new Vector2(-1, 0);
		}

		if(Input.GetKey(KeyCode.W)){
			toReturn += new Vector2(0, 1);
		}


		if(Input.GetKey(KeyCode.D)){
			toReturn += new Vector2(1, 0);
		}

		if(Input.GetKey(KeyCode.S)){
			toReturn += new Vector2(0, -1);
		}
				
		return toReturn * m_speed;
	}

	void shoot(Vector3 p_direction){
		Vector2 direction = VectorCalc.CalcVec3to2( p_direction - gameObject.transform.position ).normalized;

		GameObject bullet = Instantiate(m_bullet, VectorCalc.CalcVec3to2(gameObject.transform.position) + direction*0.5f, Quaternion.identity);	
		//Vector3 position  = Camera.allCameras[0].ScreenToWorldPoint(Input.mousePosition);

		bullet.GetComponent<Bullet>().Initalize(direction, m_damage, "Player", m_logger, m_game_controller);
	}

  public void damage(float damage)
  {
    m_hp -= damage;
		if(m_hp <= 0 ){
			m_logger.unlog(gameObject, EObjectTypes.PLAYER);
			Destroy(gameObject);
		}
  }
}
