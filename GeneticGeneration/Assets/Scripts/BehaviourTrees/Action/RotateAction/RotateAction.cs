using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class RotateAction : IAction
{

  IDirection m_rotate_target;
	
  float m_forward;

  float m_speed;

	public RotateAction(IDirection p_rotate_target, float p_forward, float speed){
    m_rotate_target = p_rotate_target;
    m_forward = p_forward;
	}

  public bool act(GameObject p_actor)
  {
    Debug.Log("RotateAction: " + m_speed);

    Vector2 forwardVector = VectorCalc.forwardVector( p_actor.transform.rotation.eulerAngles.z, m_forward );
    float angle = VectorCalc.getAngle( forwardVector, m_rotate_target.direction() );

    p_actor.transform.eulerAngles += new Vector3( 0, 0, Mathf.Sign(angle) * m_speed );

    return true;
  }

  public void reset()
  {
    return;
  }
}
