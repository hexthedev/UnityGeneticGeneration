using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class DirectionDetector : VDetector
{
	private EObjectTypes m_of;
	private Vector2 m_direction;
	private float m_angle_threshold;
	private float m_count;

 
  public DirectionDetector(EObjectTypes p_of, Vector2 p_direction, float p_angle_threshold, int p_count, IBehaviourNode p_true_child, IBehaviourNode p_false_child) : base(p_true_child, p_false_child)
  {
		m_of = p_of;
		m_direction = p_direction;
		m_angle_threshold = p_angle_threshold;
		m_count = p_count;
  }

  public override bool detect()
  {
    GameObject[] objects = m_tree.GetLogger().getByType(m_of);

		int count = 0;

		for(int i = 0; i < objects.Length; i++){

			//Vector representing direction of target from actor
			Vector2 direction_of_target = objects[i].transform.position - m_tree.getActor().transform.position;

			if(m_tree.getActor().GetComponent<EnemyController>().m_debug){
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
		Vector3 actual_direction = origin + p_actual_direction;
		Vector3 actual_direction_big = origin + p_actual_direction*5;

		Vector3 target_direction = origin + p_target_direction;
		Vector3 target_direction_big = origin + p_target_direction*5;
	

		Debug.DrawLine(origin, actual_direction_big, Color.cyan, p_duration);
		Debug.DrawLine(origin, target_direction, Color.red, p_duration);


		Debug.DrawLine(origin, origin+VectorCalc.rotateDirectionVector(p_actual_direction, m_angle_threshold), Color.cyan, p_duration);
		Debug.DrawLine(origin, origin+VectorCalc.rotateDirectionVector(p_actual_direction, -m_angle_threshold), Color.cyan, p_duration);

	}

	public static DirectionDetector random(BehaviourTree p_tree){

		EObjectTypes of = EnumCalc.randomValue<EObjectTypes>();
		Vector2 direction = VectorCalc.randomDirection();
		float angle_threshold = Random.Range(0.1f, 179.9f);
		int count = (of == EObjectTypes.PLAYER) ? 1 : Random.Range(0, 10);

		return new DirectionDetector(of, direction, angle_threshold, count, RandomGen.IBehaviourNode(p_tree), RandomGen.IBehaviourNode(p_tree));
	}

	public static DirectionDetector random(BehaviourTree p_tree, IBehaviourNode p_true_child, IBehaviourNode p_false_child){

		EObjectTypes of = EnumCalc.randomValue<EObjectTypes>();
		Vector2 direction = VectorCalc.randomDirection();
		float angle_threshold = Random.Range(0.1f, 179.9f);
		int count = (of == EObjectTypes.PLAYER) ? 1 : Random.Range(0, 10);

		return new DirectionDetector(of, direction, angle_threshold, count, p_true_child, p_false_child);
	}

}
