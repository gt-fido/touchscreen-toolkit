using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public enum Elements {
		Handle,
		Handle_Outline,
		Background,
		Empty,
		Fill,
		None
	}

	public DogSlider slider; 

	RectTransform panelTrans;

	public Elements focusedElement = Elements.None;

    private Camera cam;

	InputField color_pkr;

	void Start() {
		this.cam = Camera.FindObjectOfType<Camera>();
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
			return slider.getColor(DogSlider.Parts.Handle);
		else if(el == Elements.Empty)
			return slider.getColor(DogSlider.Parts.Slider);
		else if(el == Elements.Fill)
			return slider.getColor(DogSlider.Parts.Progress);
		else if(el == Elements.Handle_Outline)
			return slider.getColor(DogSlider.Parts.Handle_Outline);
		else
			return Color.black;
	}

	private void setElementColor(Elements el, Color color) {
		if(el == Elements.Background)
			cam.backgroundColor = color;
		else if(el == Elements.Handle)
			slider.setColor(DogSlider.Parts.Handle, color);
		else if(el == Elements.Empty)
			slider.setColor(DogSlider.Parts.Slider, color);
		else if(el == Elements.Fill)
			slider.setColor(DogSlider.Parts.Progress, color);
		else if(el == Elements.Handle_Outline)
			slider.setColor(DogSlider.Parts.Handle_Outline, color);
	}

	public void focusOnElement(int element){
		Elements el = (Elements) element;
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

	public void setHandleFill(bool value){
		slider.SetHandleFill(value);
	}

	public void setPhysics(bool physics) {
		slider.SetPhysics(physics);
	}

	public void rotateSlider(float degree) {
		slider.RotateSlider(degree);
	}

	public void setHandleRadius(float radius) {
		slider.SetHandleRadius(radius);
	}

	public void setTrackWidth(float width) {
		slider.SetWidth(width);
	}

	public void setSliderYScale(float scale) {
		slider.SetYScale(scale);
	}

	public void setSliderXScale(float scale) {
		slider.SetXScale(scale);
	}

	public void setHandleOutlineWidth(float width) {
		slider.SetHandleBorderWidth(width);
	}
}
