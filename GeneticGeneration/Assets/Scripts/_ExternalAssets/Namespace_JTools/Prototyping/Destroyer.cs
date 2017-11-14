using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.Events;

namespace JTools
{
  namespace Prototyping
  {
    namespace Destroyer
    {
      public class Destroyer : MonoBehaviour
      {

        float m_time = -1;
				bool m_start = false;

        // Use this for initialization

        public void DestroyIn(float p_time)
        {
					m_start = true;
					m_time = p_time;
        }

				void Update(){
					if(m_time > 0 && m_start){
						m_time -= Time.deltaTime;

						if(m_time <= 0f){
							Destroy(gameObject);
						}
					}
				}


      }
    }
  }
}