using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.Events;
using JTools.Calc.DataStructures;

namespace JTools
{

  namespace DataStructures
  {

    namespace CooldownLogger
    {


      public class CooldownLogger
      {

        private Dictionary<string, float> m_cooldowns;
        private List<string> m_keys;

        public CooldownLogger()
        {
          m_cooldowns = new Dictionary<string, float>();
          m_keys = new List<string>();
        }

        public void activate(string p_ability, float p_cooldown)
        {
          keyAddTest(p_ability);
          m_cooldowns[p_ability] = p_cooldown;
        }

        public void tickAll(float p_tick)
        {
          foreach (string key in m_keys)
          {
            tickAbility(key, p_tick);
          }

        }

        public void tickAbility(string p_ability, float p_tick)
        {
          keyAddTest(p_ability);
          m_cooldowns[p_ability] -= m_cooldowns[p_ability] <= 0 ? 0 : p_tick;
        }

        public bool isCooldownOver(string p_ability)
        {
          if (!m_cooldowns.ContainsKey(p_ability)) return true;
          return m_cooldowns[p_ability] <= 0;
        }

        public float cooldownRemaining(string p_ability)
        {
          return m_cooldowns[p_ability];
        }

        private void keyAddTest(string p_ability)
        {
          if (!m_cooldowns.ContainsKey(p_ability))
          {
            m_cooldowns.Add(p_ability, 0);
            m_keys.Add(p_ability);
          }
        }

      }

    }

  }

}

