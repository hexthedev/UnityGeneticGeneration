using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLogger : MonoBehaviour {

	private Dictionary<EObjectTypes, List<GameObject>> m_objects;

	void Awake(){

		m_objects = new Dictionary<EObjectTypes, List<GameObject>>();

		foreach(EObjectTypes type in System.Enum.GetValues(typeof(EObjectTypes))){
			m_objects[type] = new List<GameObject>();
		}
	}

	public void log(GameObject p_object, EObjectTypes p_type){
		m_objects[p_type].Add(p_object);
		//debug(p_object.transform.position, Color.red, 1f);
		//Debug.Log("ADD: " +  m_objects[p_type].Count);
	}

	public void unlog(GameObject p_object, EObjectTypes p_type){
		m_objects[p_type].Remove(p_object);
		//debug(p_object.transform.position, Color.blue, 1f);
		//Debug.Log("REMOVE: " +  m_objects[p_type].Count);
	}

	public GameObject[] getByType(EObjectTypes p_type){
		/*foreach(GameObject x in m_objects[p_type].ToArray()){
			debug(x.transform.position, Color.cyan, 10f);
		}*/

		return m_objects[p_type].ToArray();
	}

	public GameObject[] getAll(){
		List<GameObject> objects = new List<GameObject>();

		foreach(List<GameObject> list in m_objects.Values){
			objects.AddRange(list);
		}
		
		/*foreach(GameObject x in objects.ToArray()){
			debug(x.transform.position, Color.magenta, 10f);
		}*/

		return objects.ToArray();
	}


	//DEBUG
	private void debug(Vector3 p_position, Color p_color, float p_duration){
		Debug.DrawLine(Vector3.zero, p_position, p_color, p_duration);
	}	



}
