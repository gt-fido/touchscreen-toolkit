using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BgController : MonoBehaviour {
    public Color start;
    public Color end;
    private DogSlider slider;
    private Camera cam;
	// Use this for initialization
    private void Awake() {
        cam = Camera.main;
        slider = FindObjectOfType(typeof(DogSlider)) as DogSlider;
    }

    public void adjustColor(float change) {
        cam.backgroundColor = Color.Lerp(start, end, change);
    }
}
