using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

	RectTransform panelTrans;

	void Start() {
		panelTrans = GetComponent<RectTransform>();
	}

	public void movePanelIn() {
		panelTrans.offsetMin = new Vector2(0, panelTrans.offsetMin.y);
		panelTrans.offsetMax = new Vector2(0, panelTrans.offsetMax.y);
	}

	public void movePanelOut() {
		panelTrans.offsetMin = new Vector2(-1000, panelTrans.offsetMin.y);
		panelTrans.offsetMax = new Vector2(-1000, panelTrans.offsetMax.y);
	}

}
