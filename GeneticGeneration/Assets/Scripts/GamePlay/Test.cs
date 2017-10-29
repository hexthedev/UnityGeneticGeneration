using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathNet.Numerics.Random;

public class Test : MonoBehaviour {

	private MersenneTwister m_random = new MersenneTwister();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(m_random.Next());
	}
}
