using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Dog_Slider_Custom : MonoBehaviour {

	// Reference to the curve instantiated further down the inheritance tree
	public BezierCurve curve;
	public Color background = Color.red;
	public float width = 1.0f;

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

		lines.AddComponent<LineRenderer>();
		LineRenderer lr = lines.GetComponent<LineRenderer>();
		lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		lr.startColor = background;
		lr.endColor = background;
		lr.startWidth = width;
		lr.endWidth = width;
		lr.positionCount = 0;

		if(points.Count > 1){
			List<Vector3> curv_pnts = new List<Vector3>();
			for(int i = 0; i < points.Count - 1; i++){
				for(int j = 0; j <= curve.resolution; j++) {
					float percent =  (float)j/curve.resolution;
					curv_pnts.Add(BezierCurve.GetPoint(points[i], points[i+1], percent));
				}
			}
			lr.positionCount = curv_pnts.Count;
			lr.SetPositions(curv_pnts.ToArray());
			
			// if (curve.close) DrawCurveGamespace(lines.transform, points[points.Count - 1], points[0], curve.resolution, lr);
		}
	}

	void OnDisable(){
		GameObject.DestroyImmediate(lines);
	}

	// public static void DrawCurveGamespace(BezierPoint p1, BezierPoint p2, int resolution, LineRenderer lr)
	// {
	// 	int limit = lr.;
	// 	float _res = resolution;
	// 	Vector3 currentPoint = Vector3.zero;

	// 	for(int i = lr.positionCount - resolution; i < limit; i++){
	// 		currentPoint = BezierCurve.GetPoint(p1, p2, i/_res);
	// 		lr.SetPosition(i, currentPoint);
	// 	}
	// }

	// public static void DrawCurveGamespace(Transform lines, BezierPoint p1, BezierPoint p2, int resolution, Color color, float width)
	// {
	// 	int limit = resolution+1;
	// 	float _res = resolution;
	// 	Vector3 lastPoint = p1.position;
	// 	Vector3 currentPoint = Vector3.zero;
		
	// 	for(int i = 1; i < limit; i++){
	// 		currentPoint = BezierCurve.GetPoint(p1, p2, i/_res);
	// 		BezierCurve.DrawLine(lastPoint, currentPoint, color, lines.transform, width);
	// 		lastPoint = currentPoint;
	// 	}
	// }
}
