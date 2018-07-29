using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandleController : MonoBehaviour
     , IPointerDownHandler // 2
     , IPointerUpHandler
     , IDragHandler{

    private float yOffset;
    private float xOffset;
    public float startingPercent {get; private set;}
    private Vector3 velocity;
    private Rigidbody2D _handle_body;
    private int current_closest_index;

	// Use this for initialization
	void Awake () {
        _handle_body = gameObject.GetComponent<Rigidbody2D>();
	}

    void Start () {
        // Find screen size and calculate necessary offset
        Canvas cnvs = GameObject.FindObjectOfType<Canvas>();
        var rect = (cnvs.transform as RectTransform).rect;
        yOffset = -1 * rect.height / 2f;
        xOffset = -1 * rect.width / 2f;
        _handle_body.transform.position = transform.parent.gameObject.GetComponentInChildren<BezierCurve>().GetPointAt(startingPercent);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _handle_body.velocity = Vector2.zero;
    }



    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(yOffset);
        Debug.Log(xOffset);
        Vector2 scaled = new Vector2(eventData.position.x + xOffset, eventData.position.y + yOffset) / transform.localScale;
        _handle_body.MovePosition(scaled);
        // this.transform.localPosition = new Vector3(eventData.position.x + xOffset, eventData.position.y + yOffset, 0f);
        velocity = eventData.delta * transform.localScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _handle_body.velocity = velocity;
    }
}