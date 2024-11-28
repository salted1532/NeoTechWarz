using UnityEngine;

public class MainMenuSound : MonoBehaviour
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
        WhatClip = 0;
        audiosource.clip = clip[WhatClip];
        audiosource.Play();
    }

    public void BriefingRoom()
    {
        audiosource.Stop();
        WhatClip = 1;
        audiosource.clip = clip[WhatClip];
        audiosource.Play();
    }
    // Update is called once per frame
    void Update()
    {
        if(audiosource.isPlaying == false)
        {
            audiosource.clip = clip[WhatClip];
            audiosource.Play();
        }
    }
}
