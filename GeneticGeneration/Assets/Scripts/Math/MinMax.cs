using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMaxFloat {

	float m_min;
	float m_max;

	public MinMaxFloat(float p_min, float p_max){
		m_min = p_min;
		m_max = p_max;
	}

	public float getMin(){
		return m_min;
	}

	public float getMax(){
		return m_max;
	}

}

public class MinMaxInt {

	int m_min;
	int m_max;

	public MinMaxInt(int p_min, int p_max){
		m_min = p_min;
		m_max = p_max;
	}

	public int getMin(){
		return m_min;
	}

	public int getMax(){
		return m_max;
	}

}
