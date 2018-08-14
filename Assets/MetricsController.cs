using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine;

public class MetricsController : MonoBehaviour
     , IPointerDownHandler // 2
     , IPointerUpHandler
     , IDragHandler {

	public int sample_rate = 10;
	public int max_buffer_size = 100;
	public string fileName = "metrics.txt";

	private int num_frames = 0;
	private StreamWriter file;

	private TouchEvent cur_event;

	private Queue<TouchEvent> buffer;

    public void OnDrag(PointerEventData eventData)
    {
		// TODO: fix for variable framerate
		if(Time.frameCount % sample_rate == 0)
			cur_event.path.Add(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
		cur_event = new TouchEvent();
		cur_event.start_point = eventData.pressPosition;
		cur_event.start_t = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
		cur_event.end_point = eventData.position;
		cur_event.end_t = Time.time;
		buffer.Enqueue(cur_event);
    }

    // Use this for initialization
    void Start () {
		buffer = new Queue<TouchEvent>();
		file = File.CreateText(fileName);
	}
	
	// Update is called once per frame
	void Update () {
		if(buffer.Count > max_buffer_size){
			file.WriteLine(buffer.ToString());
			buffer.Clear();
		}
	}
}

internal class TouchEvent
{
	public float start_t;
	public float end_t;
	public Vector2 start_point;
	public Vector2 end_point;
	public List<Vector2> path = new List<Vector2>();
	public int sample_rate;

	public override string ToString(){
		return start_t + ", " + end_t + ", " + sample_rate + ", " + start_point + ", " + end_point + ", " + path + "\n";
	}
}