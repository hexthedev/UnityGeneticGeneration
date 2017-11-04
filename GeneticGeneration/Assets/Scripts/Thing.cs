using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Genetic.Behaviour.Controllers;
using Genetic.Numerical.Base;
using Genetic.Composite;

using JTools.Events;

public class Thing : BrainController {

  MindBodyDNA<BrainController> m_dna;

  GameController controller;

  Dictionary<ETrait, float> m_stats;

  Rigidbody2D m_rb;

  TimeoutEventManager m_te;

  protected override void Start(){
    base.Start();

    m_rb =  gameObject.GetComponent<Rigidbody2D>();

    m_te = new TimeoutEventManager();

    m_te.addTimeout(3f, () => {
      die();
    });
  }

  protected override void FixedUpdate(){
    base.FixedUpdate();

    m_te.tick(Time.fixedDeltaTime);

    Vector2 velo = m_rb.velocity;

    if(velo.magnitude > m_stats[ETrait.SPEED]){
      m_rb.velocity = velo.normalized*m_stats[ETrait.SPEED];
    }
  }

  public void Initialze(MindBody p_mindbody, MindBodyDNA<BrainController> p_dna, GameController p_controller){
    m_stats = p_mindbody.m_body;
    base.InitializeBrain(p_mindbody.m_mind);
    controller = p_controller;
    m_dna = p_dna.Clone();
  }

  private void die(){
    controller.m_evolution.addDNA(m_dna, gameObject.transform.position.magnitude);
    Destroy(gameObject);
  }
}
