using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NIDirection : INeuralInput {

	private CreatureController m_controller;

	private Vector2 m_direction;

	public NIDirection (CreatureController p_controller, float[] p_params){
		m_controller = p_controller;
		m_direction = new Vector2(p_params[0], p_params[1]);
	}

    public SNeuralInputDNA dnaify()
    {
        float[] l_params = {m_direction[0], m_direction[1]};
		return new SNeuralInputDNA(ENeuralInput.DIRECTION, l_params);
    }

    public float input()
    {
		Vector3 player_direction = m_controller.sensePosition() - m_controller.senseNearestObjectPosition(EObjectTypes.PLAYER);
		return VectorCalc.getAngle( m_direction, player_direction )/180f;
    }
}
