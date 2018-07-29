using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandleController : MonoBehaviour
     , IPointerClickHandler // 2
     , IDragHandler
     , IPointerEnterHandler
     , IPointerExitHandler {

	private Handle _handle;
    private float yOffset;
    private float xOffset;

	// Use this for initialization
	void Awake () {
		_handle = gameObject.GetComponent<Handle>();
	}

    void Start () {
        // TODO: Find screen size and calculate necessary offset
        Canvas cnvs = GameObject.FindObjectOfType<Canvas>();
        var rect = (cnvs.transform as RectTransform).rect;
        yOffset = -1 * rect.height / 2f;
        xOffset = -1 * rect.width / 2f;

    }

    void Update(){
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // TODO: Check if inside handle and everything
        Debug.Log(transform.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.localPosition = new Vector3(eventData.position.x + xOffset, eventData.position.y + yOffset, 0f);
        // this.transform.position += new Vector3(eventData.delta.x, eventData.delta.y, 0f);
        Vector3 velocity = eventData.delta * transform.localScale;
        _handle.GetComponent<Rigidbody2D>().velocity = velocity;
        // Debug.Log("Velocity: " + velocity.ToString() + ".");
        // Debug.Log("Dragged Handle");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Entered Handle");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
  		Debug.Log("Exited Handle");
    }
}