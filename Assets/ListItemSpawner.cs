using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListItemSpawner : MonoBehaviour {

	public RectTransform listItem;
	public RectTransform contentParent;

	public int count;

	// Use this for initialization
	void Awake() {
		for(int i = 0; i < count; i++) {
			Instantiate(listItem.gameObject, contentParent, false);
		}
	}
}
