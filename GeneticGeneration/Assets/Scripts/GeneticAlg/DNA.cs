using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Genetic.Base;

public class MindBodyDNA : IDNA<MindBodyDNA, MindBody>{

	private PhysicalDNA m_body;

	private PhysicalDNA m_mind;

	public MindBodyDNA(PhysicalDNA p_body, PhysicalDNA p_mind){
		m_body = p_body;
		m_mind = p_mind;
	}


  public MindBody birth(MindBodyDNA p_birth_object)
  {
    new MindBody(p_birth_object);
  }

  public IDNA<MindBodyDNA, MindBody> crossover(MindBodyDNA p_object)
  {
     return new MindBodyDNA(m_body.crossover(p_object.m_body), m_mind.crossover(p_object.m_mind));
  }

  public MindBodyDNA getSelf()
  {
    return this;
  }

  public IDNA<MindBodyDNA, MindBody> mutate()
  {
    return new MindBodyDNA(m_body.mutate(), m_mind.mutate());
  }
}


public class MindBody{

	public PhysicalDNA m_mind;
	public PhysicalDNA m_body;

}
