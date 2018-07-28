using System.Collections;
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
	private GameObject _lines;

	// Use this for initialization
	void Awake () {
		_curve = gameObject.GetComponentInChildren<BezierCurve>();
		_lines = GameObject.Find("Lines");
		if(_lines)
			lr_track = _lines.GetComponent<LineRenderer>();
		_handle = gameObject.GetComponentInChildren<Handle>();			
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
	}

	void OnDisable(){
		GameObject.DestroyImmediate(_lines);
		// TODO: Don't destroy but reset handle on disable
	}

	private void DrawCurve() {
		var points = _curve.getPoints();

		_lines = new GameObject("Lines");
		_lines.transform.parent = this.transform;

		_lines.AddComponent<LineRenderer>();
		lr_track = _lines.GetComponent<LineRenderer>();
		lr_track.material = new Material(Shader.Find("Unlit/Color"));
		lr_track.material.color = sliderBackground;
		lr_track.startWidth = sliderWidth;
		lr_track.endWidth = sliderWidth;
		lr_track.numCapVertices = 10;
		lr_track.positionCount = 0;

		if(points.Count > 1){
			List<Vector3> curv_pnts = new List<Vector3>();
			for(int i = 0; i < points.Count - 1; i++){
				for(int j = 0; j <= _curve.resolution; j++) {
					float percent =  (float)j/_curve.resolution;
					curv_pnts.Add(BezierCurve.GetPoint(points[i], points[i+1], percent));
				}
			}
			lr_track.positionCount = curv_pnts.Count;
			lr_track.SetPositions(curv_pnts.ToArray());
			
			// if (curve.close) DrawCurveGamespace(lines.transform, points[points.Count - 1], points[0], curve.resolution, lr);
		}
	}
}
