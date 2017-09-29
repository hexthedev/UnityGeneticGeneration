using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class RotateAction : IAction
{
  BehaviourTree m_tree;

  Rigidbody2D m_rb;

  IDirection m_rotate_target;
	
  float m_forward;

  float m_speed;

	public RotateAction(IDirection p_rotate_target, float p_forward, float p_speed){
    m_rotate_target = p_rotate_target;
    m_forward = p_forward;
    m_speed = p_speed;
  }

  //LOADING
  public void load(BehaviourTree p_tree)
  {
    m_tree = p_tree;
    m_rb = m_tree.getActor().GetComponent<Rigidbody2D>();
    m_rotate_target.load(p_tree);
  }


  public void unload()
  {
    //m_tree = null;
    m_rb = null;
    m_rotate_target.unload();
  }

  //BEHAVIOUR
  public bool act()
  {
    GameObject actor = m_tree.getActor();

    Vector2 forwardVector = VectorCalc.forwardVector( actor.transform.rotation.eulerAngles.z, m_forward );
    float angle = VectorCalc.getAngle( forwardVector, m_rotate_target.direction() );

    actor.transform.eulerAngles -= new Vector3(0,0, Mathf.Sign(angle))*m_speed;

    m_rb.velocity *= 0.99f;

    return true;
  }



  public void reset()
  {
    return;
  }

  public IAction clone()
  {
    return new RotateAction(m_rotate_target.clone(), m_forward, m_speed);
  }
}
