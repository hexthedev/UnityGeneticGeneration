using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject m_enemy;

	public float m_game_speed;
	public bool m_reset_speed;

	void Start(){
		//spawn(new DNA(new PhysicalDNA(), new NeuralDNA()), 1);
	}

	public void spawn(DNA p_evo, int p_id){		
		GameObject dude =  Instantiate(m_enemy, new Vector3( Random.Range(-8, 8), Random.Range(-3, 3), 0), Quaternion.identity );
		dude.GetComponent<CreatureController>().Initalize(p_evo, p_id);
	}

}
