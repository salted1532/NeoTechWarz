using UnityEngine;
using FischlWorks_FogWar;
using UnityEngine.UI;

public class ConstructionObj : MonoBehaviour
{
    [SerializeField]
    private float dieTime = 1;

    [SerializeField]
    private Slider slider;
    [SerializeField]
    private GameObject Canvas;

    Camera mainCamera;

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
        InvokeRepeating("PlayAudioClip", 0f, 3f);

        mainCamera = Camera.main;

        if (GameObject.Find("PlacementSystem").GetComponent<PlacementSystem>().selectedOnbjectIndex == 0)
        {
            dieTime = 37;
        }
        if (GameObject.Find("PlacementSystem").GetComponent<PlacementSystem>().selectedOnbjectIndex == 1)
        {
            dieTime = 37;
        }
        if (GameObject.Find("PlacementSystem").GetComponent<PlacementSystem>().selectedOnbjectIndex == 2)
        {
            dieTime = 37;
        }
        if (GameObject.Find("PlacementSystem").GetComponent<PlacementSystem>().selectedOnbjectIndex == 3)
        {
            dieTime = 50;
        }
        if (GameObject.Find("PlacementSystem").GetComponent<PlacementSystem>().selectedOnbjectIndex == 4)
        {
            dieTime = 25;
        }
        if (GameObject.Find("PlacementSystem").GetComponent<PlacementSystem>().selectedOnbjectIndex == 5)
        {
            dieTime = 37;
        }
        if (GameObject.Find("PlacementSystem").GetComponent<PlacementSystem>().selectedOnbjectIndex == 6)
        {
            dieTime = 0;
        }

        slider.maxValue = dieTime;
        slider.value = 0;

        Destroy(gameObject, dieTime);

    }

    void PlayAudioClip()
    {
        audiosource.Play();
    }

    // Update is called once per frame
    void Update()
    {

        slider.value += Time.deltaTime;
        // 카메라를 바라보는 방향 벡터를 계산합니다.
        Vector3 lookDirection = mainCamera.transform.position - transform.position;
        lookDirection.y = 0; // y 축 회전을 고정하려면 y 값을 0으로 설정합니다.

        // 방향 벡터를 기준으로 회전을 계산합니다.
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        // X 값을 -90으로 고정합니다.
        targetRotation *= Quaternion.Euler(-90f, 0f, 0f);

        // 회전을 적용합니다.
        Canvas.transform.rotation = targetRotation;
    }
}
