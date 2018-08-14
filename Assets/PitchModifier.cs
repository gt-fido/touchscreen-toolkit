using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class PitchModifier : MonoBehaviour {
    public float pitchValue = 1.0f;
    public AudioClip mySong;

    private AudioSource audioSource;
    private float low = 0.75f;
    private float high = 1.25f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = mySong;
        audioSource.loop = true;
    }

	public void IncreasePitch(float percentage){
		audioSource.pitch = low + (percentage * 0.5f);
	}

	public void DecreasePitch(float percentage){
		audioSource.pitch = high - (percentage * 0.5f);
	}
}
