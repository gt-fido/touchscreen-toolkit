using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	RectTransform panelTrans;

	Image img;

    public Color c_start = Color.blue;
    public Color c_end = Color.yellow;
    
    private Camera cam;

	InputField color_pkr;

	void Start() {
		this.cam = Camera.FindObjectOfType<Camera>();
		this.cam.backgroundColor = c_start;
		panelTrans = GetComponent<RectTransform>();
		color_pkr = GameObject.FindGameObjectWithTag("color_picker").GetComponent<InputField>();
	}

	public void movePanelIn() {
		panelTrans.offsetMin = new Vector2(0, panelTrans.offsetMin.y);
		panelTrans.offsetMax = new Vector2(0, panelTrans.offsetMax.y);
	}

	public void movePanelOut() {
		panelTrans.offsetMin = new Vector2(-1000, panelTrans.offsetMin.y);
		panelTrans.offsetMax = new Vector2(-1000, panelTrans.offsetMax.y);
	}

	public void focusOnElement(Image img){
		this.img = img;
		color_pkr.text = ColorUtility.ToHtmlStringRGB(img.color);
		color_pkr.onEndEdit.Invoke(color_pkr.text);
		Debug.Log(color_pkr.text);
	}

	public void focusOnCamera(){
		this.img = null;
		color_pkr.text = ColorUtility.ToHtmlStringRGB(cam.backgroundColor);
		color_pkr.onEndEdit.Invoke(color_pkr.text);
		Debug.Log(color_pkr.text);
	}

	public void changeColor(Color color){
		// Debug.Log("Setting color to: " + color);
		if(this.img != null) {
			this.img.color = color;
		} else if(this.cam != null) {
			this.cam.backgroundColor = color;
		}
	}
}
