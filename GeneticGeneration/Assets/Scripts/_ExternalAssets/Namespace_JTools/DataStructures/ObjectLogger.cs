using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.Calc.Enum;

namespace JTools
{

  namespace DataStructures
  {
    namespace ObjectLogger
    {

      ///<summary>Used to log gameobjects of specific types for easy access to GameObjects</summary>
      public static class ObjectLogger
      {

        private static Dictionary<string, List<GameObject>> m_objects;

        static ObjectLogger()
        {
          m_objects = new Dictionary<string, List<GameObject>>();
          m_objects.Add("ALL", new List<GameObject>());
        }

        //Log
        public static void log(GameObject p_object, string p_type)
        {
          if (!m_objects.ContainsKey(p_type)) m_objects.Add(p_type, new List<GameObject>());
          m_objects[p_type].Add(p_object);
          m_objects["ALL"].Add(p_object);
        }

        public static void log(GameObject p_object, string[] p_types)
        {
          foreach (string type in p_types)
          {
            if (!m_objects.ContainsKey(type)) m_objects.Add(type, new List<GameObject>());
            m_objects[type].Add(p_object);
          }
          m_objects["ALL"].Add(p_object);
        }

        //Unlog
        public static void unlog(GameObject p_object, string p_type)
        {
          m_objects[p_type].Remove(p_object);
          m_objects["ALL"].Remove(p_object);
        }

        public static void unlog(GameObject p_object, string[] p_types)
        {
          foreach (string type in p_types)
          {
            m_objects[type].Remove(p_object);
          }
          m_objects["ALL"].Remove(p_object);
        }

        //Get
        public static GameObject[] getByType(string p_type)
        {
          if(!m_objects.ContainsKey(p_type)) return new GameObject[] {};
          return m_objects[p_type].ToArray();
        }

        //DEBUG
        private static void debug(Vector3 p_position, Color p_color, float p_duration)
        {
          Debug.DrawLine(Vector3.zero, p_position, p_color, p_duration);
        }

      }

    }

  }
}