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
		// if(lr.material.color != background)
		// 	lr.material.color = background;
		// if(lr.startWidth - width > 0.1f){
		// 	lr.startWidth = width;
		// 	lr.endWidth = width;
		// }
	}

    public void RotateSlider(float degree) {
        // If the absolute difference is less than 0.1 do nothing.
        if(Mathf.Abs(this.transform.eulerAngles.z - degree) <= 0.1f)
            return;
        
        // Otherwise rotate to the new degree
        this.transform.localEulerAngles = new Vector3(0.0f,0.0f,degree);
		OnDisable();
		OnEnable();

        // TODO: Intelligently change scale to keep in frame
   }

//    public void ScaleHandle(float scale) {
//         // If the absolute difference is less than 0.01 do nothing.
//         if(Mathf.Abs(this.handleRect.transform.localScale.x - scale) <= 0.01)
//             return;
        
//         // Otherwise scale to new scale
//         this.handleRect.transform.localScale = new Vector3(scale, scale, 1.0f);
//    }

   public void SetWidth(float width) {
		if(Mathf.Abs(this.width - width) <= 0.1f)
			return;

		this.width = width;
		lr.startWidth = width;
		lr.endWidth = width;
   }

	public void SetXScale(float scale) {
		Transform trans = curve.transform;
		Debug.Log(trans.name);
		if(Mathf.Abs(trans.localScale.x - scale) <= 0.1f)
			return;
		
		trans.localScale = new Vector3(scale, curve.transform.localScale.y, curve.transform.localScale.z);
		OnDisable();
		OnEnable();
	}

	public void SetYScale(float scale) {
		Transform trans = curve.transform;
		Debug.Log(trans.name);
		if(Mathf.Abs(trans.localScale.y - scale) <= 0.1f)
			return;
		
		trans.localScale = new Vector3(curve.transform.localScale.x, scale, curve.transform.localScale.z);
		OnDisable();
		OnEnable();
	}

    public void SetHeight(float height) {
        // If the absolute difference is less than 0.01 do nothing.
        RectTransform rt = this.transform as RectTransform;

        // Otherwise scale to new scale
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
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
		lr.alignment = LineAlignment.TransformZ;

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
