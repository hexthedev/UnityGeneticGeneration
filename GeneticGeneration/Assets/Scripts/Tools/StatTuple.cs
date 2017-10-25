using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTuple {

	private float m_current;
	private float m_total;

	public StatTuple(float p_current, float p_total){
		m_current = p_current;
		m_total = p_total;
	}
	
	public float getCurrent(){
		return m_current;
	}

	public void setCurrent(float new_value){
		m_current = new_value;
	}

	public void addToCurrent(float p_update){
		m_current += p_update;
		
		if(m_current > m_total){
			m_current = m_total;
		}
	}

	public float getTotal(){
		return m_total;
	}

	public void adjust(float p_mod){
		m_current *= p_mod;
		m_total *= p_mod;
	}
}
