using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace JTools{
	namespace Prototyping{
		namespace LineCreator{

			static class LineCreator{

				public static GameObject createLine(Vector3 p_point1, Vector3 p_point2, Color p_color, float p_width){

					GameObject ob = new GameObject();
					LineRenderer lineRenderer = ob.AddComponent<LineRenderer>();
					lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
					lineRenderer.widthMultiplier = p_width;
					lineRenderer.positionCount = 2;
					lineRenderer.SetPosition(0,p_point1);
					lineRenderer.SetPosition(1,p_point2);
					
					float alpha = 1.0f;
					Gradient gradient = new Gradient();
        	gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(p_color, 0.0f), new GradientColorKey(p_color, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1f) }
            );
        	lineRenderer.colorGradient = gradient;

					return ob;
				}

			}

		}
	}
}



