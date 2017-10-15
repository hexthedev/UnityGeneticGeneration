using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeoutEventManager {

	List<TimeoutTick> m_listeners;

	public TimeoutEventManager(){
		m_listeners = new List<TimeoutTick>();
	}

	private void purgeList(){
		List<TimeoutTick> purge = new List<TimeoutTick>();

		foreach(TimeoutTick listner in m_listeners){
			if(listner.isFired()){
				purge.Add(listner);
			}
		}

		foreach(TimeoutTick to_purge in purge){
			m_listeners.Remove(to_purge);
		}
	}

	public void addTimeout(float p_time, DTimeoutListener p_listener){
		m_listeners.Add(new TimeoutTick(p_time, p_listener));
	}

	public void tick(float p_delta_time){
		foreach(TimeoutTick listeners in m_listeners){
			listeners.tick(p_delta_time);
		}

		purgeList();

	}

}

class TimeoutTick{

	private float m_tick_limit;
	private float m_tick_current;

	private DTimeoutListener m_listener;

	public TimeoutTick(float m_limit, DTimeoutListener p_listener){
		m_tick_limit = m_limit;
		m_tick_current = 0;

		m_listener = p_listener;
	}

	public void tick(float p_delta_time){
		m_tick_current += p_delta_time;

		if(isFired()){
			m_listener();
		}
	}

	public bool isFired(){
		return m_tick_current >= m_tick_limit;
	}

}

public delegate void DTimeoutListener();
