using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public enum Elements {
		Handle,
		Background,
		Fill,
		Empty,
		None
	}

	public Dog_Slider_Custom slider; 

	RectTransform panelTrans;

	public Elements focusedElement = Elements.None;

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

	private Color getElementColor(Elements el) {
		if(el == Elements.Background)
			return cam.backgroundColor;
		else if(el == Elements.Handle)
			return slider.handle;
		else if(el == Elements.Empty)
			return slider.background;
		else if(el == Elements.Fill)
			return slider.fill;
		else
			return Color.black;
	}

	private void setElementColor(Elements el, Color color) {
		if(el == Elements.Background)
			cam.backgroundColor = color;
		else if(el == Elements.Handle)
			slider.handle = color;
		else if(el == Elements.Empty)
			slider.background = color;
		else if(el == Elements.Fill)
			slider.fill = color;
	}

	public void focusOnElement(string element){
		Elements el = (Elements) System.Enum.Parse(typeof(Elements), element, true);
		this.focusedElement = el;
		color_pkr.text = ColorUtility.ToHtmlStringRGB(getElementColor(el));
		color_pkr.onEndEdit.Invoke(color_pkr.text);
		Debug.Log(color_pkr.text);
	}

	public void changeColor(Color color){
		// Debug.Log("Setting color to: " + color);
		if(this.focusedElement != Elements.None)
			setElementColor(this.focusedElement, color);
	}
}
