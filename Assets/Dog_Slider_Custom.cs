using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog_Slider_Custom : MonoBehaviour {
	// Reference to the curve instantiated further down the inheritance tree
	public BezierCurve curve;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private GameObject lines;

	void OnEnable(){
		curve.SetDirty();
		var points = curve.getPoints();
		lines = new GameObject("Lines");
		lines.transform.parent = this.transform;
		if(points.Count > 1){
			for(int i = 0; i < points.Count - 1; i++){
				DrawCurveGamespace(lines.transform, points[i], points[i+1], curve.resolution);
			}
			
			if (curve.close) DrawCurveGamespace(lines.transform, points[points.Count - 1], points[0], curve.resolution);
		}
	}

	void OnDisable(){
		GameObject.DestroyImmediate(lines);
	}

	public static void DrawCurveGamespace(Transform lines, BezierPoint p1, BezierPoint p2, int resolution)
	{
		int limit = resolution+1;
		float _res = resolution;
		Vector3 lastPoint = p1.position;
		Vector3 currentPoint = Vector3.zero;
		
		for(int i = 1; i < limit; i++){
			currentPoint = BezierCurve.GetPoint(p1, p2, i/_res);
			BezierCurve.DrawLine(lastPoint, currentPoint, Color.black, lines.transform);
			lastPoint = currentPoint;
		}
	}
}
