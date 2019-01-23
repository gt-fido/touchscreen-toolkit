﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class HandleController : MonoBehaviour
     , IPointerDownHandler // 2
     , IPointerUpHandler
     , IDragHandler{

    private bool dragging;
    public float startingPercent {get; private set;}
    private Vector3 velocity;
    private Rigidbody2D _handle_body;
    private int current_closest_index;
    private DogSlider _dog_slider;

    public bool physics {get; private set;}
    public float[] position_length_info {get; private set;}
    private float _percent;
    public float percent {
        get{
            return _percent;
        } private set{
            if(_percent == value) return;
            _percent = value;
            if(_dog_slider.percentChanged != null)
                _dog_slider.percentChanged.Invoke(_percent);
        }
    }
    
	// Use this for initialization
	void Awake () {
        _handle_body = gameObject.GetComponent<Rigidbody2D>();
        _dog_slider = gameObject.GetComponentInParent<DogSlider>();
	}

    void Start () {
        // Find screen size and calculate necessary offset
        Canvas cnvs = GameObject.FindObjectOfType<Canvas>();
        var rect = (cnvs.transform as RectTransform).rect;
        // _handle_body.transform.position = transform.parent.gameObject.GetComponentInChildren<BezierCurve>().GetPointAt(startingPercent);
        _handle_body.transform.position = transform.parent.gameObject.GetComponentInChildren<BezierCurve>().GetPointAt(0) + new Vector3(0, 0, -1);
    }

    void OnDisable() {
        _handle_body.transform.position = transform.parent.gameObject.GetComponentInChildren<BezierCurve>().GetPointAt(0) + new Vector3(0, 0, -1);
        percent = 0f;
        current_closest_index = 0;
    }

    void OnEnable() {
    }

    void Update() {
        if(_handle_body.velocity != Vector2.zero || dragging){
            UpdatePercent();
        }
    }

    private void UpdatePercent(){
        var samples = _dog_slider.sampled_line_points;
        Vector3 cur = _handle_body.transform.position;

        int start = current_closest_index - 10 < 0 ? 0 : current_closest_index - 10; 
        int end = current_closest_index + 10 < samples.Length ? current_closest_index + 10 : samples.Length - 1;

        int min_idx = start;
        float min_dist = Mathf.Abs(Vector3.Distance(samples[min_idx], cur));
        float temp = 0f;
        for(int i = start + 1; i <= end; i++) {
            temp = Mathf.Abs(Vector3.Distance(cur, samples[i]));
            if(temp < min_dist){
                min_idx = i;
                min_dist = temp;
            }
        }
        percent = Mathf.Round(position_length_info[min_idx] * 1000f / _dog_slider.curveLength) / 1000f;
        current_closest_index = min_idx;
        // Debug.Log(percent);
    }

    public void SetPositionLength(float[] arr){
        position_length_info = arr;
    }

	public void SetPhysics(bool physics){
		this.physics = physics;
        _handle_body.velocity = Vector2.zero;
        _handle_body.gravityScale = physics ? 1.0f : 0.0f;
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        _handle_body.velocity = Vector2.zero;
        dragging = true;
        Debug.Log(eventData.pointerCurrentRaycast.worldPosition);
    }



    public void OnDrag(PointerEventData eventData)
    {
        
        // Debug.Log(yOffset);
        // Debug.Log(xOffset);
        // Vector2 scaled = new Vector2(eventData.position.x + xOffset, eventData.position.y + yOffset) / transform.localScale;
        if(eventData.pointerCurrentRaycast.gameObject && dragging)
        {
            Vector2 scaled = (Vector2)eventData.pointerCurrentRaycast.worldPosition;
            _handle_body.MovePosition(scaled);
            // this.transform.localPosition = new Vector3(eventData.position.x + xOffset, eventData.position.y + yOffset, 0f);
            velocity = eventData.delta * transform.localScale;
        } else if (dragging) {
            OnPointerUp(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(physics)
            _handle_body.velocity = velocity;
        dragging = false;
    }
}