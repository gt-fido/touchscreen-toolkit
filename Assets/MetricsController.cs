using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine;

public class MetricsController : MonoBehaviour
{

    public int sample_rate = 10;
    public int max_buffer_size = 1;
    public string fileName = "metrics.txt";

    private int num_frames = 0;
    private StreamWriter file;

    private TouchEvent cur_event;

    private Queue<TouchEvent> buffer;

	private Thread write_thread;

    // Use this for initialization
    void Start()
    {
        buffer = new Queue<TouchEvent>();
        file = File.CreateText(fileName);
		write_thread = new Thread(new ThreadStart(WriteToLogThread));

    }

	private void WriteToLogThread() {
		if(buffer.Count == 0)
			return;
		TouchEvent tch;
		lock(buffer) {
			tch = buffer.Dequeue();
		}
		Debug.Log(tch);
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle movements based on TouchPhase
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    // Record initial touch position.
                    cur_event = new TouchEvent();
                    cur_event.start_point = touch;
                    cur_event.start_t = Time.time;
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    // Determine direction by comparing the current touch position with the initial one
                    if (Time.frameCount % sample_rate == 0)
                        cur_event.path.Add(touch);
                    break;

                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
					cur_event.end_point = touch;
					cur_event.end_t = Time.time;
					buffer.Enqueue(cur_event);
					if (buffer.Count > max_buffer_size) {
						Debug.Log(buffer.ToString());
						file.WriteLine(buffer.ToString());
						buffer.Clear();
					}
                    break;
            }
        }
    }
}

internal class TouchEvent
{
    public float start_t;
    public float end_t;
    public Touch start_point;
    public Touch end_point;
    public List<Touch> path = new List<Touch>();
    public int sample_rate;

    public override string ToString()
    {
        return start_t + ", " + end_t + ", " + sample_rate + ", " + start_point.ToString() + ", " + end_point.ToString() + ", " + path.ToString() + "\n";
    }
}