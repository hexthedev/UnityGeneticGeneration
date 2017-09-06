using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float m_maxSpeed = 10f;

	//Character Facing

	private Rigidbody2D m_rb;

	private Animator m_anim;

	private Camera m_camera;

	public GameObject m_bullet;

	// Use this for initialization
	void Start () {
		m_rb = gameObject.GetComponent<Rigidbody2D>();
		m_anim = gameObject.GetComponent<Animator>();
		m_camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetMouseButtonDown(0)){
			Debug.DrawLine(gameObject.transform.position, m_camera.ScreenToWorldPoint(Input.mousePosition), Color.red, 1);
			Instantiate(m_bullet, gameObject.transform.position, Quaternion.Euler(m_camera.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position));
		}

	}

	void FixedUpdate(){

		float l_hor = Input.GetAxis("Horizontal");
		float l_ver = Input.GetAxis("Vertical");

		m_rb.velocity = new Vector2(l_hor*m_maxSpeed, l_ver*m_maxSpeed);

		m_anim.SetFloat("Speed", Mathf.Abs(l_hor)+Mathf.Abs(l_ver));

		setDirection(l_hor, l_ver);
	}

	private void setDirection(float p_hor, float p_ver){
		resetDirection();
		bool horIsBigger = (Mathf.Abs(p_hor) > Mathf.Abs(p_ver));

		if(horIsBigger){
			if(p_hor > 0){
				m_anim.SetBool("Left", true);
			} else {
				m_anim.SetBool("Right", true);
			}
		} else {
			if(p_ver >0){
				m_anim.SetBool("Up", true);
			} else if(p_ver <0){
				m_anim.SetBool("Down", true);
			}
		}
	}	

	private void resetDirection(){
		m_anim.SetBool("Left", false);
		m_anim.SetBool("Right", false);
		m_anim.SetBool("Up", false);
		m_anim.SetBool("Down", false);
	}

	
}
