using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NIDirection : INeuralInput {

	private GameObject m_actor;

	private ObjectLogger m_logger;

	private Vector2 m_direction;

	public NIDirection (GameObject p_actor, ObjectLogger p_logger, float[] p_params){
		m_actor = p_actor;
		m_logger = p_logger;
		m_direction = new Vector2(p_params[0], p_params[1]);
	}

    public SNeuralInputDNA dnaify()
    {
        float[] l_params = {m_direction[0], m_direction[1]};
		return new SNeuralInputDNA(ENeuralInput.DIRECTION, l_params);
    }

    public float input()
    {
        GameObject player = m_logger.getByType(EObjectTypes.PLAYER)[0];
		
		Vector3 player_direction = player.transform.position - m_actor.transform.position;

		//NumberTester.log( VectorCalc.getAngle( m_direction, player_direction )/180f == 0 ? 0 : Mathf.Sign(VectorCalc.getAngle( m_direction, player_direction )/180f));

		return VectorCalc.getAngle( m_direction, player_direction )/180f;
    }
}
