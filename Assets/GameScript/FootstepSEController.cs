using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class FootstepSEController : MonoBehaviour
{
    [SerializeField]
    AudioClip[] clips;

    [SerializeField]
    bool randomizePitch = true;

    [SerializeField]
    float pitchRange = 0.1f;

    AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    void PlayFootstepSE()
    {
        if(randomizePitch == true)
        {
            source.pitch = 1.0f + Random.Range(-pitchRange, pitchRange);
        }

        source.PlayOneShot(clips[Random.Range(0, clips.Length)], 0.5f);
    }
}
