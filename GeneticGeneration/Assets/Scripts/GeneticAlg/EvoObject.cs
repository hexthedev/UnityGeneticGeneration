using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvoObject {

	private DNA m_DNA;
	private BehaviourTree m_behaviour;

	public EvoObject(DNA p_DNA, BehaviourTree p_behaviour){
		m_DNA = p_DNA;
		m_behaviour = p_behaviour;
	}

	public DNA getDNA(){
		return m_DNA;
	}

	public BehaviourTree GetBehaviour(){
		return m_behaviour;
	}

}
