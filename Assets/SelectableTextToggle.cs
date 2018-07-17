using UnityEngine;
using UnityEngine.UI;

public class SelectableTextToggle : MonoBehaviour {

	[Header("Text")]
	public Text text;
	public Color selectedTextColor;

	[Header("Background")]
	public Image background;
	public Color selectedBackgroundColor;

	private Toggle toggle;
	private Color initialTextColor;
	private Color initialBackgroundColor;

	private void Awake() {
		toggle = GetComponent<Toggle>();
		toggle.group = GetComponentInParent<ToggleGroup>();

		initialTextColor = text.color;
		initialBackgroundColor = background.color;
	}

	private void OnEnable() {
		if(toggle != null) {
			toggle.onValueChanged.AddListener(OnToggled);
		}
	}

	private void OnDisable() {
		if(toggle != null) {
			toggle.onValueChanged.RemoveListener(OnToggled);
		}
	}

	private void OnToggled(bool isOn) {
		if(isOn) {
			text.color = selectedTextColor;
			background.color = selectedBackgroundColor;
		} else {
			text.color = initialTextColor;
			background.color = initialBackgroundColor;
		}
	}
}