﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Calc;

public class MiniMap : MonoBehaviour {

	public ObjectLogger m_logger;

	private Texture2D m_tex;

	private RawImage m_image;

	private int test = 0;

	// Use this for initialization
	void Start () {
		m_image = gameObject.GetComponent<RawImage>();

		m_tex = new Texture2D(132,95);

		m_image.texture = m_tex;

		m_logger = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectLogger>();

	}
	
	// Update is called once per frame
	void Update () {
		
		Color[] colors = m_tex.GetPixels();
		
		for(int i = 0; i<colors.Length; i++){
			colors[i] = Color.black;
		}

		m_tex.SetPixels(colors);

		GameObject[] player = m_logger.getByType(EObjectTypes.PLAYER);

		foreach(GameObject x in player){
			Vector2 position = VectorCalc.CalcVec3to2(x.transform.position);
			position *= 5;
			position.x += 66;
			position.y += 47;

			Debug.Log(position);

			m_tex.SetPixel( Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Color.red);
		}


		GameObject[] enemies = m_logger.getByType(EObjectTypes.ENEMY);

		foreach(GameObject x in enemies){
			Vector2 position = VectorCalc.CalcVec3to2(x.transform.position);
			position *= 5;
			position.x += 66;
			position.y += 47;

			Debug.Log(position);

			m_tex.SetPixel( Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Color.green);
		}


		m_tex.Apply();

		test = test > 132 ? test = 0 : test+1;
	}
}