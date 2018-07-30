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

    public void adjustBackgroundColor(float change) {
        cam.backgroundColor = Color.Lerp(start, end, change);
    }

    public void adjustHandleColor(float change) {
        slider.setColor(DogSlider.Parts.Handle, Color.Lerp(start, end, change));
    }

    public void adjustSliderColor(float change) {
        slider.setColor(DogSlider.Parts.Slider, Color.Lerp(start, end, change));
    }

    public void adjustOutlineColor(float change) {
        slider.setColor(DogSlider.Parts.Handle_Outline, Color.Lerp(start, end, change));
    }

}
