using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject m_enemy;
	public float m_game_speed;

	private Dictionary<int, EvolutionController> m_species;

	void Start(){
		m_species = new Dictionary<int, EvolutionController>();
		m_species.Add(0, new EvolutionController(2));
		spawn(0);
	}

	private void spawn(int p_species_id){		
		GameObject creature =  Instantiate(m_enemy, new Vector3( Random.Range(-8, 8), Random.Range(-3, 3), 0), Quaternion.identity );
		creature.GetComponent<CreatureController>().Initalize(m_species[p_species_id].birth(), m_species[p_species_id].getCreaturesBirthed());
	}

}
