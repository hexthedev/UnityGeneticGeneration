using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject m_enemy;

	public void spawn(DNA p_dna){
		GameObject dude =  Instantiate(m_enemy, new Vector3( Random.Range(-8, 8), Random.Range(-3, 3), 0), Quaternion.identity );
		dude.GetComponent<EnemyController>().Initalize(p_dna);
	}
}
