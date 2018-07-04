using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class sliderTouch : MonoBehaviour
{
    public GameObject yellowSlider = null;

    static bool hitYellow = false;

    AudioSource beepAudio;

    // Use this for initialization
    void Start()
    {
        beepAudio = GetComponent<AudioSource>();
        //if (beepAudio != null) beepAudio.clip.LoadAudioData();

        yellowSlider = GameObject.Find("yellow");
        Cursor.visible = true;
    }

    void Update()
    {
        RaycastHit hit = new RaycastHit();

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out hit))
            {
                string hitName = hit.transform.gameObject.name;
                Debug.Log(hitName);
                beepAudio = hit.transform.gameObject.GetComponent<AudioSource>();
                if (beepAudio != null) beepAudio.clip.LoadAudioData();
                if (hitName != "miss")
                {
                    if (beepAudio != null)
                    {
                        beepAudio.Play();
                    }

                }
            }
        }
    }
}
