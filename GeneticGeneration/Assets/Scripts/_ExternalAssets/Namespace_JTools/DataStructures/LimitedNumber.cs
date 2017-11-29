using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.Calc.Bool;
using JTools.Calc.Base;

namespace JTools
{

  namespace DataStructures
  {

    namespace LimitedNumber
    {

      public class LimitedNumber
      {

        private float m_value;

        public float Value { get { return m_value; } }

        private Range<float> m_limits;

        public float Min { get { return m_limits.Min; } }
        public float Max { get { return m_limits.Max; } }
        

        ///<summary>Special Constructor for limited numbers that go from 0 to a max value</summary>
        public LimitedNumber(float p_start_value)
        {
          m_value = p_start_value;
          m_limits = new Range<float>(0, p_start_value);
        }


        public LimitedNumber(float p_start_value, Range<float> p_limits)
        {
          m_value = p_start_value;
          m_limits = p_limits.Clone();
        }

        public void add(float p_value)
        {
          m_value += p_value;
          balance();
        }

        public void changeMax(float p_new_value)
        {
          m_limits.Max = p_new_value;
        }

        public bool isMax(){
          return m_value == m_limits.Max;
        }

        public void changeMin(float p_new_value)
        {
          m_limits.Min = p_new_value;
        }

        public bool isMin(){
          return m_value == m_limits.Min;
        }

        private void balance()
        {
          if (m_value > m_limits.Max) m_value = m_limits.Max;
          if (m_value < m_limits.Min) m_value = m_limits.Min;
        }

        public LimitedNumber Clone()
        {
          return new LimitedNumber(m_value, m_limits.Clone());
        }

        public override string ToString(){
          return "LimitedValue [ " + m_value + " : ( " + m_limits.Min + " , " + m_limits.Max + " ) ]";
        }

      }

    }

  }

}



