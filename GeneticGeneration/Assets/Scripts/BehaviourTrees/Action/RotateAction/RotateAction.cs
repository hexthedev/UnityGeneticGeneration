using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class RotateAction : IAction
{

  IDirection m_rotate_target;
	
  float m_forward;

  float m_speed;

	public RotateAction(IDirection p_rotate_target, float p_forward, float p_speed){
    m_rotate_target = p_rotate_target;
    m_forward = p_forward;
    m_speed = p_speed;
  }

  public bool act(BehaviourTree p_tree)
  {
    GameObject actor = p_tree.getActor();

    Vector2 forwardVector = VectorCalc.forwardVector( actor.transform.rotation.eulerAngles.z, m_forward );
    float angle = VectorCalc.getAngle( forwardVector, m_rotate_target.direction() );

    actor.transform.eulerAngles -= new Vector3(0,0, Mathf.Sign(angle))*m_speed;

    actor.GetComponent<Rigidbody2D>().velocity *= 0.99f;

    return true;
  }

  public void reset()
  {
    return;
  }
}
