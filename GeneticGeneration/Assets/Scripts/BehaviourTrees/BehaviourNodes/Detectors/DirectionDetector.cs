using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class DirectionDetector : VDetector
{
	private EObjectTypes m_of;
	private Vector2 m_direction;
	private float m_angle_threshold;
	private int m_count;

 
  public DirectionDetector(EObjectTypes p_of, Vector2 p_direction, float p_angle_threshold, int p_count, BehaviourTree p_tree, VTreeNode<IBehaviourNode> p_parent) : base(p_tree, p_parent)
  {
		m_of = p_of;
		m_direction = p_direction;
		m_angle_threshold = p_angle_threshold;
		m_count = p_count;
  }


  public override bool detect()
  {
    GameObject[] objects = ObjectLogger.getByType(m_of);

		int count = 0;

		for(int i = 0; i < objects.Length; i++){

			//Vector representing direction of target from actor
			Vector2 direction_of_target = objects[i].transform.position - m_tree.getActor().transform.position;

			if(m_tree.getActorController()){
				debug(m_direction, direction_of_target, Time.deltaTime);
			}

			if(Vector2.Angle( direction_of_target, m_direction ) < m_angle_threshold){
				count++;
			}

			if(count >= m_count){
				return true;
			}
		}

		return false;
  }


	public void debug(Vector3 p_actual_direction, Vector3 p_target_direction, float p_duration){
		
		Vector3 origin = m_tree.getActor().transform.position;
		//Vector3 actual_direction = origin + p_actual_direction;
		Vector3 actual_direction_big = origin + p_actual_direction*5;

		Vector3 target_direction = origin + p_target_direction;
//		Vector3 target_direction_big = origin + p_target_direction*5;
	

		Debug.DrawLine(origin, actual_direction_big, Color.cyan, p_duration);
		Debug.DrawLine(origin, target_direction, Color.red, p_duration);


		Debug.DrawLine(origin, origin+VectorCalc.rotateDirectionVector(p_actual_direction, m_angle_threshold), Color.cyan, p_duration);
		Debug.DrawLine(origin, origin+VectorCalc.rotateDirectionVector(p_actual_direction, -m_angle_threshold), Color.cyan, p_duration);

		Debug.Log("Direction: " + m_of + ", Number: " + m_count);
	}
}
