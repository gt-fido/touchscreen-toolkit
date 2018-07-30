using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour {

    public Slider mainSlider;
    public AudioSource sineTone;

    float currValue;

    public void Start()
    { 
    	sineTone = GetComponent<AudioSource>();
        mainSlider.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
	    currValue = mainSlider.value;
    }

    public void ValueChangeCheck()
    {
        Debug.Log(mainSlider.value);
        if(currValue != mainSlider.value) {
	    	sineTone.Play(0);
        }
        currValue = mainSlider.value;
    }
}
