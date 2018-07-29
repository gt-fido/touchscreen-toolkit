using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandleController : MonoBehaviour
     , IPointerDownHandler // 2
     , IPointerUpHandler
     , IDragHandler{

	private Handle _handle;
    private BezierCurve _bezier;
    private float yOffset;
    private float xOffset;
    [SerializeField]
    private float startingPercent = 0f;
    private Vector3 velocity;

	// Use this for initialization
	void Awake () {
		_handle = gameObject.GetComponent<Handle>();
        _bezier = transform.parent.gameObject.GetComponentInChildren<BezierCurve>();
	}

    void Start () {
        // Find screen size and calculate necessary offset
        Canvas cnvs = GameObject.FindObjectOfType<Canvas>();
        var rect = (cnvs.transform as RectTransform).rect;
        yOffset = -1 * rect.height / 2f;
        xOffset = -1 * rect.width / 2f;

        // TODO: Have handle start at beginning of slider
        _handle.transform.position = _bezier.GetPointAt(startingPercent);
    }

    void Update(){
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _handle.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }



    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(yOffset);
        Debug.Log(xOffset);
        Vector2 scaled = new Vector2(eventData.position.x + xOffset, eventData.position.y + yOffset) / transform.localScale;
        _handle.GetComponent<Rigidbody2D>().MovePosition(scaled);
        // this.transform.localPosition = new Vector3(eventData.position.x + xOffset, eventData.position.y + yOffset, 0f);
        velocity = eventData.delta * transform.localScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _handle.GetComponent<Rigidbody2D>().velocity = velocity;
    }
}