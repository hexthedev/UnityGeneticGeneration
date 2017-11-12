using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JTools.Interfaces;

using JTools.DataStructures.LimitedNumber;
using JTools.DataStructures.ObjectLogger;


public class Resource : MonoBehaviour, IResource
{

  TextMesh m_resource_left_text;

  LimitedNumber m_resource_left;

  public void Initalize(float m_total_energy)
  {
    m_resource_left = new LimitedNumber(m_total_energy);
    m_resource_left_text = transform.GetChild(0).GetComponent<TextMesh>();
    UpdateText();
  }

  public float collect(float p_collect_power)
  {
    if (m_resource_left.Value > p_collect_power)
    {
      m_resource_left.add(-p_collect_power);
      UpdateText();
      return p_collect_power;
    }

    die();
    return p_collect_power - m_resource_left.Value;
  }

  private void die()
  {
    ObjectLogger.unlog(gameObject, "RESOURCE");
    Destroy(gameObject);
  }

  private void UpdateText()
  {
    m_resource_left_text.text = "" + Mathf.Round(m_resource_left.Value * 10) / 10f;
  }
}
