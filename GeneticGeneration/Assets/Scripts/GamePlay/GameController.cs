using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject m_enemy;

	[Range(0.01f,30)]
	public float m_timeToSpawn;
	private float m_timer = 0f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		m_timer += Time.deltaTime;

		if(m_timer >= m_timeToSpawn){
			GameObject dude =  Instantiate(m_enemy, new Vector3( Random.Range(-8, 8), Random.Range(-5, 5), 0), Quaternion.identity );
			dude.GetComponent<EnemyController>().Initalize(Random.Range(1,10), Random.Range(1,10), Random.Range(1,10), Random.Range(1,10));
			m_timer = 0;
		}

	}
}
