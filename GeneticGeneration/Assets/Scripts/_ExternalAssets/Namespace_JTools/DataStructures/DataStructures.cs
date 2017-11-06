using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.Calc.Bool;

namespace JTools
{
		
namespace DataStructures
{
		
namespace PriorityList{

	public class PriorityList{

		Dictionary<string, PriorityElement> m_list;

		public PriorityList()
		{
			m_list = new Dictionary<string, PriorityElement>();
		}

		public void flush(){
			m_list = new Dictionary<string, PriorityElement>();
		}

		public void add(string p_type, float p_priority, DPriorityDelegate p_callback){
			if(!m_list.ContainsKey(p_type)){
				m_list.Add(p_type, new PriorityElement(p_priority, p_callback));
				return;
			} 

			if(m_list[p_type].Priority > p_priority) return;

			if(m_list[p_type].Priority == p_priority){
				if(!BoolCalc.random()) return;
			}

			m_list.Remove(p_type);
			m_list.Add(p_type, new PriorityElement(p_priority, p_callback));
		}

		public void activate(){
			foreach(string key in m_list.Keys){
				m_list[key].activate();
			}
		}
	}

	public struct PriorityElement{
		float m_priority;

		DPriorityDelegate m_callback;

		public PriorityElement(float p_priority, DPriorityDelegate p_callback){
			m_priority = p_priority;
			m_callback = p_callback;
		}

		public float Priority{ get { return m_priority; } }

		public void activate(){
			m_callback();
		}
	}

	public delegate void DPriorityDelegate();

}

	
}

}



