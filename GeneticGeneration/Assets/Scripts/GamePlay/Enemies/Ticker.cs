using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticker {

	private Dictionary<float, Tick> m_ticks;

	public Ticker(){
		m_ticks = new Dictionary<float, Tick>();
	}

	public void tick(float p_delta_time){
		foreach(Tick l_tick in m_ticks.Values){
			l_tick.addTick(p_delta_time);
		}
	}

	public void addListener(float p_period, DTickListener p_listener){
		if(!m_ticks.ContainsKey(p_period)){
			m_ticks.Add(p_period, new Tick(p_period));
		}

		m_ticks[p_period].addListener(p_listener);
	}

	public void removeListener(float p_period, DTickListener p_listener){
		if(!m_ticks.ContainsKey(p_period)){
			return;
		}

		m_ticks[p_period].removeListener(p_listener);

		if(m_ticks[p_period].numListeners() == 0){
			m_ticks.Remove(p_period);
		}
	}
}

class Tick{

	private float m_tick_limit;
	private float m_tick_current;
	

	private List<DTickListener> m_listeners;

	public Tick(float m_limit){
		m_tick_limit = m_limit;
		m_tick_current = 0;

		m_listeners = new List<DTickListener>();
	}

	public void addTick(float p_delta_time){
		m_tick_current += p_delta_time;

		if(m_tick_current >= m_tick_limit){

			foreach(DTickListener l_listener in m_listeners){
				l_listener();
			}

			m_tick_current -= m_tick_limit;

		}
	}

	public void addListener(DTickListener p_listener){
		m_listeners.Add(p_listener);
	}

	public void removeListener(DTickListener p_listener){
		if(!m_listeners.Contains(p_listener)){
			return;
		}
		
		m_listeners.Remove(p_listener);
	}

	public int numListeners(){
		return m_listeners.Count;
	}

}

public delegate void DTickListener();
