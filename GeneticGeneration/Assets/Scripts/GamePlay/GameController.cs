using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject m_enemy;

	private ObjectLogger m_logger;

	void Awake(){
		m_logger = gameObject.GetComponent<ObjectLogger>();
	}

	public void spawn(EvoObject p_evo){		
		GameObject dude =  Instantiate(m_enemy, new Vector3( Random.Range(-8, 8), Random.Range(-3, 3), 0), Quaternion.identity );
		dude.GetComponent<EnemyController>().Initalize(p_evo, m_logger);
	}
}
