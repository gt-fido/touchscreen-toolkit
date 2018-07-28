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

	// Use this for initialization
	void Awake () {
		_handle = gameObject.GetComponent<Handle>();
	}
	
    void Update(){
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // TODO: Check if inside handle and everything
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position += new Vector3(eventData.delta.x / transform.localScale.x, eventData.delta.y / transform.localScale.y, 0f);
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