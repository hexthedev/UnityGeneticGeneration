using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JTools.Calc.Base;
using JTools.Calc.Vector;

using JTools.Gameplay;
public class MiniMap : MonoBehaviour {

	private Texture2D m_tex;

	private RawImage m_image;

	private int test = 0;

	// Use this for initialization
	void Start () {
		m_image = gameObject.GetComponent<RawImage>();

		m_tex = new Texture2D(132,95);

		m_image.texture = m_tex;
	}
	
	// Update is called once per frame
	void Update () {
		
		Color[] colors = m_tex.GetPixels();
		
		for(int i = 0; i<colors.Length; i++){
			colors[i] = Color.black;
		}

		m_tex.SetPixels(colors);

		GameObject[] player = ObjectLogger.getByType(EObjectTypes.PLAYER);

		foreach(GameObject x in player){
			Vector2 position = Vector2Calc.fromVector3(x.transform.position);
			position *= 5;
			position.x += 66;
			position.y += 47;

			Debug.Log(position);

			m_tex.SetPixel( Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Color.red);
		}


		GameObject[] enemies = ObjectLogger.getByType(EObjectTypes.ENEMY);

		foreach(GameObject x in enemies){
			Vector2 position = Vector2Calc.fromVector3(x.transform.position);
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
