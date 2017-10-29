using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectLogger {

	private static Dictionary<EObjectTypes, List<GameObject>> m_objects;

	static ObjectLogger(){

		m_objects = new Dictionary<EObjectTypes, List<GameObject>>();

		foreach(EObjectTypes type in System.Enum.GetValues(typeof(EObjectTypes))){
			m_objects[type] = new List<GameObject>();
		}
	}

	public static void log(GameObject p_object, EObjectTypes p_type){
		m_objects[p_type].Add(p_object);
		//debug(p_object.transform.position, Color.red, 1f);
		//Debug.Log("ADD: " +  m_objects[p_type].Count);
	}

	public static void unlog(GameObject p_object, EObjectTypes p_type){
		m_objects[p_type].Remove(p_object);
		m_objects[EObjectTypes.ALL].Remove(p_object);
		//debug(p_object.transform.position, Color.blue, 1f);
		// Debug.Log("REMOVE: " +  m_objects[p_type].Count);
		// Debug.Log(p_type);
	}

	public static GameObject[] getByType(EObjectTypes p_type){
		foreach(GameObject x in m_objects[p_type].ToArray()){
			//debug(x.transform.position, Color.cyan, 10f);
		}

		if(p_type == EObjectTypes.ALL){
			return getAll();
		}

		return m_objects[p_type].ToArray();
	}

	public static GameObject[] getByTypeByDistance(EObjectTypes p_type, Vector3 p_point){
		
		//if(p_type == EObjectTypes.BULLET) Debug.Log(m_objects[p_type].Count);

		
		List<GameObject> objects = m_objects[p_type];

		SortedDictionary<float, GameObject> ob_by_dist = new SortedDictionary<float, GameObject>();

		for(int i = 0; i<objects.Count; i++){	

			
			float mag = objects[i] == null ? 0 : (objects[i].transform.position - p_point).magnitude;
			
			//THIS SHOULD NEVER HAPPEN, CANT FIND BUG RIGHT NOW
			if(objects[i] == null) {
				// Debug.Log("Happens: " + m_objects[p_type].Count);
				m_objects[p_type].Remove(objects[i]);
				m_objects[EObjectTypes.ALL].Remove(objects[i]);
				// Debug.Log("Happens2: " + m_objects[p_type].Count);
			}

			while(ob_by_dist.ContainsKey(mag)){
				mag += 0.001f;
			}

			ob_by_dist.Add(mag, objects[i]);
		}

		GameObject[] to_return_temp = new GameObject[ob_by_dist.Count];
		ob_by_dist.Values.CopyTo(to_return_temp, 0);

		return to_return_temp;
	}

	public static GameObject getNthClosest(int p_n, EObjectTypes p_type, Vector3 p_point){
		GameObject[] objects_of_type = getByTypeByDistance(p_type, p_point);

		if(objects_of_type.Length < p_n-1){
			return null; //No gameobject avaialble
		}

		return objects_of_type[p_n];
	}
	

	public static GameObject[] getAll(){
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
	private static void debug(Vector3 p_position, Color p_color, float p_duration){
		Debug.DrawLine(Vector3.zero, p_position, p_color, p_duration);
	}	



}
