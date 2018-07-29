﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DogSlider : MonoBehaviour {

	public enum Parts {
		Slider,
		Handle,
		Progress
	}

	[Header("Slider Properties")]
	// Reference to the curve instantiated further down the inheritance tree
	[SerializeField]
	private int sliderResolution = 30;
	[SerializeField]
	private Color sliderBackground;
	[SerializeField]
	private Color sliderProgress;
	[SerializeField]
	private float sliderWidth = 1.0f;
	[SerializeField]
	private float sliderLength = 1.0f;
	[SerializeField]
	private float sliderColliderWidth = 1.0f;

	[Header("Handle Properties")]
	[SerializeField]
	private int handleResolution = 10;
	[SerializeField]
	private Color handleFillColor;
	[SerializeField]
	private Color handleBorderColor;
	[SerializeField]
	private bool handleClose = true;
	[SerializeField]
	private bool handleFill = true;
	[SerializeField]
	private float handleBorderWidth = 0.2f;
	[SerializeField]
	private float handleRadius = 5f;
	[SerializeField]
	private RigidbodyType2D handlePhysics = RigidbodyType2D.Static;


	private LineRenderer lr_track;
	private BezierCurve _curve;
	private Handle _handle;
	private HandleController _handle_control;
	private GameObject _lines;


	public Vector3[] sampled_line_points {
		get{
			Vector3[] ret = new Vector3[lr_track.positionCount];
			lr_track.GetPositions(ret);
			return ret;
		}
	}

	public float curveLength {
		get{
			return _curve.length;
		}
	}

	// Use this for initialization
	void Awake () {
		_curve = gameObject.GetComponentInChildren<BezierCurve>();
		_lines = GameObject.Find("Lines");
		if(_lines)
			lr_track = _lines.GetComponent<LineRenderer>();
		_handle = gameObject.GetComponentInChildren<Handle>();
		_handle_control = gameObject.GetComponentInChildren<HandleController>();
		OnDisable();
		OnEnable();
	}

	// Update is called once per frame
	void Update () {
		if(lr_track.material.color != sliderBackground)
			lr_track.material.color = sliderBackground;
		if(_handle.fill_color != handleFillColor)
			_handle.SetFillColor(handleFillColor);
		if(_handle.border_color != handleBorderColor)
			_handle.SetBorderColor(handleBorderColor);
		if(_handle.border_width != handleBorderWidth)
			_handle.SetBorderWidth(handleBorderWidth);
		if(_handle.resolution != handleResolution)
			_handle.SetResolution(handleResolution);
		if(_handle.radius != handleRadius)
			_handle.SetRadius(handleRadius);
		if(_handle.fill != handleFill)
			_handle.SetFill(handleFill);
		if(_handle.close != handleClose)
			_handle.SetClosed(handleClose);
		if(_handle.physics != handlePhysics)
			_handle.SetPhysics(handlePhysics);

		if(_curve.resolution != sliderResolution)
			_curve.resolution = sliderResolution;

		// If changed at all
		if(_handle.dirty_flag) {
			_handle.UpdateShape();
			_handle.dirty_flag = false;
		}

	}

	public void setColor(Parts part, Color color) {
		switch (part)
		{
			case Parts.Handle:
				handleFillColor = color;
				break;
			case Parts.Progress:
				sliderProgress = color;
				break;
			case Parts.Slider:
				sliderBackground = color;
				break;
			default:
				break;
		}
	}

	public Color getColor(Parts part) {
		switch (part)
		{
			case Parts.Handle:
				return handleFillColor;
			case Parts.Progress:
				return sliderProgress;
			case Parts.Slider:
				return sliderBackground;
			default:
				return Color.black;
		}
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

   public void SetHandleRadius(float radius) {
		if(Mathf.Abs(this.handleRadius - radius) <= 0.1f)
			return;

		handleRadius = radius;
   }

   public void SetWidth(float width) {
		if(Mathf.Abs(this.sliderWidth - width) <= 0.1f)
			return;

		this.sliderWidth = width;
		lr_track.startWidth = width;
		lr_track.endWidth = width;
   }

	public void SetXScale(float scale) {
		Transform trans = _curve.transform;
		Debug.Log(trans.name);
		if(Mathf.Abs(trans.localScale.x - scale) <= 0.1f)
			return;

		trans.localScale = new Vector3(scale, _curve.transform.localScale.y, _curve.transform.localScale.z);
		OnDisable();
		OnEnable();
	}

	public void SetYScale(float scale) {
		Transform trans = _curve.transform;
		Debug.Log(trans.name);
		if(Mathf.Abs(trans.localScale.y - scale) <= 0.1f)
			return;

		trans.localScale = new Vector3(_curve.transform.localScale.x, scale, _curve.transform.localScale.z);
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
		_curve.SetDirty();
		// Draw curve
		if(!_lines)
			DrawCurve();

		_lines.transform.localPosition = Vector3.zero;
	}

	void OnDisable(){
		GameObject.DestroyImmediate(_lines);
		// TODO: Don't destroy but reset handle on disable
	}

	private void DrawCurve() {
		var points = _curve.getPoints();

		_lines = new GameObject("Lines");
		_lines.transform.parent = this.transform;

		lr_track = _lines.AddComponent<LineRenderer>();
		lr_track.material = new Material(Shader.Find("Unlit/Color"));
		lr_track.material.color = sliderBackground;
		lr_track.widthMultiplier = sliderWidth;
		lr_track.numCapVertices = 10;

		var lines_collider = _lines.AddComponent<EdgeCollider2D>();
		lines_collider.edgeRadius = 1f;


		if(points.Count > 1){
			Vector3[] curv_pnts = new Vector3[(sliderResolution * (points.Count - 1)) + 1];
			float [] line_dist = new float[(sliderResolution * (points.Count - 1)) + 1];
			for(int i = 0; i < points.Count - 1; i++){
				for(int j = 0; j <= _curve.resolution; j++) {
					int idx = i * sliderResolution + j;
					curv_pnts[idx] = BezierCurve.GetPoint(points[i], points[i+1], (float)j/_curve.resolution);
					if(idx > 0) {
						line_dist[idx] = Mathf.Abs(Vector3.Distance(curv_pnts[idx], curv_pnts[idx - 1])) + line_dist[idx-1];
					} else {
						line_dist[idx] = 0f;
					}
				}
			}

			lr_track.positionCount = curv_pnts.Length;
			lr_track.SetPositions(curv_pnts);

			lines_collider.points = GetPointsSurrounding(curv_pnts);

			_handle_control.SetPositionLength(line_dist);
			// Debug.Log(_curve.length);
			// if (curve.close) DrawCurveGamespace(lines.transform, points[points.Count - 1], points[0], curve.resolution, lr);
		}
	}

	private Vector2[] GetPointsSurrounding(Vector3[] points) {
		int length = points.Length * 2 + 3;
		Vector2[] sur_pnts = new Vector2[length];
		float fullWidth = sliderColliderWidth + sliderWidth;

		Vector2 dir, pos_nine, neg_nine;

		for(int i = 1; i < points.Length; i++) {
			dir = points[i] - points[i-1];
			pos_nine = dir.normalized.Rotate(90);
			neg_nine = dir.normalized.Rotate(-90);
			sur_pnts[i] = pos_nine * fullWidth + (Vector2)points[i];
			sur_pnts[length - 2 - i] = neg_nine * fullWidth + (Vector2)points[i];
		}
		// Make endpoints to connect last and first segments of line
		dir = sur_pnts[2] - sur_pnts[1];
		dir = dir.normalized.Rotate(180);
		sur_pnts[0] = dir * fullWidth + sur_pnts[1];
		sur_pnts[length-1] = sur_pnts[0];

		dir = sur_pnts[length-4] - sur_pnts[length-3];
		dir = dir.normalized.Rotate(180);
		sur_pnts[length-2] = dir * fullWidth + sur_pnts[length-3];

		// Make midpoints connect
		dir = sur_pnts[points.Length - 2] - sur_pnts[points.Length - 1];
		dir = dir.normalized.Rotate(180);
		sur_pnts[points.Length] = dir * fullWidth + sur_pnts[points.Length - 1];

		dir = sur_pnts[points.Length + 3] - sur_pnts[points.Length + 2];
		dir = dir.normalized.Rotate(180);
		dir = sur_pnts[points.Length + 1] = dir * fullWidth + sur_pnts[points.Length + 2];

		return sur_pnts;
	}

	private void PercentageColorChange(float percent, Parts slider=Parts.Slider) {
	    // Necessary for the rgb multiplier
		float red, green, blue;
	    if(percent < 50.0) {
	        red   = 0;
	        green = 0 + (percent * 2);
	        blue  = 100 - (percent * 2);
	    }
        else {
	        red   = (percent - 50) * 2;
	        green = 100;
	        blue  = 0;
        }
        Color color = new Color(red, green, blue, 1.0F);
        setColor(slider, color);
	}
}


 public static class Vector2Extension {
     
     public static Vector2 Rotate(this Vector2 v, float degrees) {
         float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
         float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
         
         float tx = v.x;
         float ty = v.y;
         v.x = (cos * tx) - (sin * ty);
         v.y = (sin * tx) + (cos * ty);
         return v;
     }
 }