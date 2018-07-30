using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[ExecuteInEditMode]
public class Handle : MonoBehaviour {
	public int resolution {get; private set;}
	public bool fill {get; private set;}
	public float radius {get; private set;}

	public Color fill_color {
		get{
			return mr.sharedMaterial.color;
		} private set {
			mr.sharedMaterial.color = value;
		}}
	public Color border_color {
		get{
			return lr.sharedMaterial.color;
		} private set {
			lr.sharedMaterial.color = value;
		}
	}
	public float border_width {
		get{
			return lr.widthMultiplier;
		} private set{
			lr.widthMultiplier = value;
		}}

	public bool close {
		get {
			return lr.loop;
		} private set {
			lr.loop = value;
		}}

	public RigidbodyType2D physics {
		get{
			return rb.bodyType;
		} private set {
			rb.bodyType = value;
		}}

	private LineRenderer lr;
	private Rigidbody2D rb;
	private CircleCollider2D cc2d;
	private MeshFilter mf;
	private MeshRenderer mr;
	public bool dirty_flag;
	private Vector3[] _points;

	private void Awake() {
		dirty_flag = true;
		lr = GetComponent<LineRenderer>();
		lr.material = new Material(Shader.Find("Unlit/Color"));
		rb = GetComponent<Rigidbody2D>();
		rb.useAutoMass = true;
		cc2d = GetComponent<CircleCollider2D>();
		mf = GetComponent<MeshFilter>();
		mr = GetComponent<MeshRenderer>();
		mr.material = new Material(Shader.Find("Unlit/Color"));
	}

	public void Update() {

	}

	public void SetPhysics(RigidbodyType2D rbtype){
		physics = rbtype;
	}

	public void SetResolution(int res){
		dirty_flag = true;
		resolution = res;
	}

	public void SetBorderWidth(float width){
		border_width = width;
	}

	public void SetRadius(float radius){
		this.radius = radius;
		dirty_flag = true;
	}

	public void SetFillColor(Color color){
		fill_color = color;
	}

	public void SetBorderColor(Color color){
		border_color = color;
	}

	public void SetFill(bool fill) {
		this.fill = fill;
		dirty_flag = true;
	}

	public void SetClosed(bool closed) {
		close = closed;
	}

	public void UpdateShape() {
		// Create array of points around a circle
		_points = Enumerable.Range(0, resolution)
			.Select(i => {
				var theta = 2 * Mathf.PI * i / resolution;
				return new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
			})
			.ToArray();

		if(fill)
			mf.mesh = CircleMesh();
		else
			mf.mesh = null;

		// Update collider
		cc2d.radius = radius;

		// Update outline
		lr.positionCount = resolution;
		lr.SetPositions(_points);
	}

	private Mesh CircleMesh() {

		var vec2_points = Enumerable.Range(0, resolution)
			.Select(i => {
				return new Vector2(_points[i].x, _points[i].y);
			})
			.ToArray();

		// Triangulate shape
		var triangles = new Triangulator(vec2_points).Triangulate();

		// Assign each vertex the fill color
		Color[] colors = Enumerable.Repeat(fill_color, resolution).ToArray();

		var mesh = new Mesh {
			name = "Handle",
			vertices = _points,
			triangles = triangles,
			colors = colors
		};

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.RecalculateTangents();

		return mesh;
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		float deltaTheta = (2f * Mathf.PI) / resolution;
		float theta = 0f;

		Vector3 oldPos = Vector3.zero;
		for(int i = 0; i <= resolution; i++)
		{
			Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
			Gizmos.DrawLine(oldPos, transform.position + pos);
			oldPos = transform.position + pos;
			theta += deltaTheta;
		}
	}
#endif
}
