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
        // ī�޶� �ٶ󺸴� ���� ���͸� ����մϴ�.
        Vector3 lookDirection = mainCamera.transform.position - transform.position;
        lookDirection.y = 0; // y �� ȸ���� �����Ϸ��� y ���� 0���� �����մϴ�.

        // ���� ���͸� �������� ȸ���� ����մϴ�.
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        // X ���� -90���� �����մϴ�.
        targetRotation *= Quaternion.Euler(-90f, 0f, 0f);

        // ȸ���� �����մϴ�.
        Canvas.transform.rotation = targetRotation;
    }
}
