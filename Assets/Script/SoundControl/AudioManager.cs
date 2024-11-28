using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audiosource;

    [SerializeField]
    private AudioClip[] clip;

    private int WhatClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        WhatClip = Random.Range(0, 4);
    }

    void Update()
    {
        // If the audio source is not playing and there are more clips to play, play the next clip
        if (!audiosource.isPlaying)
        {
            PlayNextClip();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayNextClip();
        }
    }

    private void PlayNextClip()
    {
        if (clip.Length == 0)
        {
            Debug.LogWarning("No audio clips assigned to the AudioManager.");
            return;
        }

        // Play the next clip
        audiosource.clip = clip[WhatClip];
        audiosource.Play();

        // Move to the next clip index, and loop back to the beginning if necessary
        WhatClip = (WhatClip + 1) % clip.Length;
    }
}
