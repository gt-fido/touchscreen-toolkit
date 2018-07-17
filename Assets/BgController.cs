using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BgController : MonoBehaviour {

    public Button button;
    public Slider slider;
    public Material color;
	// Use this for initialization
	void Start () {
        color.color = UnityEngine.Color.blue;
        
		
	}
	
	// Update is called once per frame
	void Update () {
        
        
	}

    public void adjustColor(float change) {
        color.color = Color.Lerp(UnityEngine.Color.blue, UnityEngine.Color.yellow, change);
        
    }
}
