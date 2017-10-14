using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NumberTester {

	private static Dictionary<float, int> m_num_log = new Dictionary<float, int>();

	public static void log(float p){
		if(!m_num_log.ContainsKey(p)){
			m_num_log.Add(p, 0);
		}

		m_num_log[p]++;
	}

	public static void print(){
		string results = "";

		foreach(float key in m_num_log.Keys){
			results += "[ " + key + " : " + m_num_log[key] + " ]\n"; 
		}

		Debug.Log(results);
	}

}
