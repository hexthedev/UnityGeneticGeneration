using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NIProxPlayer : INeuralInput {

	private GameObject m_actor;

	private ObjectLogger m_logger;

	private float m_scale;

	public NIProxPlayer (EnemyController p_controller, float[] p_params){
		//m_actor = p_actor;
		//m_logger = p_logger;
		m_scale = p_params[0];
	}

    public SNeuralInputDNA dnaify()
    {
        float[] l_params = {m_scale};
		return new SNeuralInputDNA(ENeuralInput.PROXPLAYER, l_params);
    }

    public float input()
    {		
		GameObject player = m_logger.getByType(EObjectTypes.PLAYER)[0];
		float player_prox = (player.transform.position - m_actor.transform.position).magnitude;

		//NumberTester.log(player_prox/m_scale == 0 ? 0 : Mathf.Sign(player_prox/m_scale));

		return player_prox/m_scale;
    }
}
