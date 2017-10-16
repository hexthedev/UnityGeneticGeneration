using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class NIProxPlayer : INeuralInput {

	private CreatureController m_controller;

	private ObjectLogger m_logger;

	private float m_scale;

	public NIProxPlayer (CreatureController p_controller, float[] p_params){
		m_controller = p_controller;
		m_scale = p_params[0];
	}

    public SNeuralInputDNA dnaify()
    {
        float[] l_params = {m_scale};
		return new SNeuralInputDNA(ENeuralInput.PROXPLAYER, l_params);
    }

    public float input()
    {	
		float player_prox = (m_controller.sensePosition() - m_controller.senseNearestObjectPosition(EObjectTypes.PLAYER)).magnitude;
		return player_prox/m_scale;
    }
}
