using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class PointingAtDetector : VDetector, IBehaviourNode {

	float m_forward_offset_angle;
	float m_angle_threshold;
	EObjectTypes m_pointing_at;
	int m_count;

  public PointingAtDetector(EObjectTypes p_pointing_at, float p_forward_offset_angle, float p_angle_threshold, int p_count, IBehaviourNode p_true_child, IBehaviourNode p_false_child): base(p_true_child, p_false_child)
  {
		m_forward_offset_angle = p_forward_offset_angle;
		m_pointing_at = p_pointing_at;
		m_angle_threshold = p_angle_threshold;
		m_count = p_count;
  }

  public override bool detect()
  {
 		//Vector Representing direction actor is pointing
		Vector2 forward_vector = VectorCalc.forwardVector(m_tree.getActor().transform.rotation.eulerAngles.z, m_forward_offset_angle);

		GameObject[] objects = m_tree.GetLogger().getByType(m_pointing_at);

		int count = 0;

		for(int i = 0; i<objects.Length; i++){
			
			Vector2 pointing_vector = objects[i].transform.position - m_tree.getActor().transform.position;

			if(m_tree.getActor().GetComponent<EnemyController>().m_debug){
				debug(forward_vector, pointing_vector, Time.deltaTime);
			}

			if(Vector2.Angle( forward_vector, pointing_vector ) < m_angle_threshold){
				count++;
			}

			if(count >= m_count){
				return true;
			}

		}

		return false;
  }


	public void debug(Vector3 p_forward, Vector3 p_pointing, float duration){
		
		Vector3 origin = m_tree.getActor().transform.position;

		Vector3 forward = m_tree.getActor().transform.position+(p_forward);
		Vector3 forwardBig = m_tree.getActor().transform.position+(p_forward)*5;
		
		Vector3 pointing = m_tree.getActor().transform.position+(p_pointing);
		Vector3 pointingNormal = m_tree.getActor().transform.position+(p_pointing.normalized);
		
		
		Debug.DrawLine(origin, forwardBig, Color.cyan, duration);
		Debug.DrawLine(origin, pointing, Color.red, duration);
		Debug.DrawLine(forward, pointingNormal, Color.cyan, duration);
	}


	public static PointingAtDetector random(BehaviourTree p_tree){
		EObjectTypes pointing_at = EnumCalc.randomValue<EObjectTypes>();
		float forward_offset_angle = p_tree.getActorController().getForward();
		float angle_threshold = Random.Range(0f, 180f);
		int count = Random.Range(0,10);

		return new PointingAtDetector(pointing_at, forward_offset_angle, angle_threshold, count, RandomGen.IBehaviourNode(p_tree), RandomGen.IBehaviourNode(p_tree));
	}

	public static PointingAtDetector random(BehaviourTree p_tree, IBehaviourNode p_true_child, IBehaviourNode p_false_child){
		EObjectTypes pointing_at = EnumCalc.randomValue<EObjectTypes>();
		float forward_offset_angle = p_tree.getActorController().getForward();
		float angle_threshold = Random.Range(0f, 180f);
		int count = Random.Range(0,10);

		return new PointingAtDetector(pointing_at, forward_offset_angle, angle_threshold, count, p_true_child, p_false_child);
	}

}
