using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Dog_Slider_Custom : MonoBehaviour {

	// Reference to the curve instantiated further down the inheritance tree
	public BezierCurve curve;
	public Color background;
	public Color handle;
	public Color fill;
	public float width = 1.0f;
	public float length = 1.0f;

	private LineRenderer lr;

	private GameObject lines;

	// Use this for initialization
	void Awake () {
		lines = GameObject.Find("Lines");
		if(lines)
			lr = lines.GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(lr.material.color != background)
			lr.material.color = background;
		if(lr.startWidth - width > 0.1f){
			lr.startWidth = width;
			lr.endWidth = width;
		}
	}

	void OnEnable(){
		// Set dirty for redraw
		curve.SetDirty();
		// Draw curve
		if(!lines)
			DrawCurve();
	}

	void OnDisable(){
		GameObject.DestroyImmediate(lines);
	}

	private void DrawCurve() {
		var points = curve.getPoints();

		lines = new GameObject("Lines");
		lines.transform.parent = this.transform;

		lines.AddComponent<LineRenderer>();
		lr = lines.GetComponent<LineRenderer>();
		lr.material = new Material(Shader.Find("Unlit/Color"));
		lr.material.color = background;
		lr.startWidth = width;
		lr.endWidth = width;
		lr.numCapVertices = 10;
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
}
