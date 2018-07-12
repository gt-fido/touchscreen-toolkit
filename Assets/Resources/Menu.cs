using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	RectTransform panelTrans;

	Image img;

	InputField color_pkr;

	void Start() {
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
}
