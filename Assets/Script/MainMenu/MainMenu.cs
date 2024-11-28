using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuButtons;
    public GameObject RaceButtons;
    public GameObject NTAMissionButtons;
    public GameObject OCMissionButtons;

    public GameObject NTAWIn;
    public GameObject OCWIn;
    public GameObject Mission1ex;
    public GameObject Mission2ex;
    public GameObject Mission3ex;
    public GameObject Mission4ex;
    public GameObject Mission5ex;
    public GameObject NTAEnding;
    public GameObject OCEnding;

    public static int NTAMissionClear = 1;
    public static int OCMissionClear = 1;

    public bool isMainMenu = false;
    public bool isCutScence = false;

    public static int WhatMissionis = 0;

    public static int WhatRaceisMission;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetResolution();

        Debug.Log("지금 미션 뭐야" + WhatMissionis);
        Debug.Log("미션 몇개 깻어" + NTAMissionClear);
        if (isMainMenu == true)
        {
            MainMenuButtons.SetActive(true);
            RaceButtons.SetActive(false);
            NTAMissionButtons.SetActive(false);
            OCMissionButtons.SetActive(false);
        }
        if (isCutScence == true)
        {
            if (WhatRaceisMission == 0)
            {
                NTAWIn.SetActive(true);
                OCWIn.SetActive(false);
                Mission1ex.SetActive(false);
                Mission2ex.SetActive(false);
                Mission3ex.SetActive(false);
                Mission4ex.SetActive(false);
                Mission5ex.SetActive(false);
                NTAEnding.SetActive(false);
                OCEnding.SetActive(false);
            }
            else if (WhatRaceisMission == 1)
            {
                NTAWIn.SetActive(false);
                OCWIn.SetActive(true);
                Mission1ex.SetActive(false);
                Mission2ex.SetActive(false);
                Mission3ex.SetActive(false);
                Mission4ex.SetActive(false);
                Mission5ex.SetActive(false);
                NTAEnding.SetActive(false);
                OCEnding.SetActive(false);
            }
        }
    }

    public void SetResolution()
    {
        int setWidth = 1920; // 화면 너비
        int setHeight = 1080; // 화면 높이

        //해상도를 설정값에 따라 변경
        //3번째 파라미터는 풀스크린 모드를 설정 > true : 풀스크린, false : 창모드
        Screen.SetResolution(setWidth, setHeight, true);
    }

    // Update is called once per frame
    void Update()
    {
        if(NTAMissionClear > 5)
        {
            NTAMissionClear = 5;
        }
    }

    public void BackToMainMenu()
    {
        MainMenuButtons.SetActive(true);
        RaceButtons.SetActive(false);
        NTAMissionButtons.SetActive(false);
        OCMissionButtons.SetActive(false);
    }

    public void SelectRace()
    {
        MainMenuButtons.SetActive(false);
        RaceButtons.SetActive(true);
        NTAMissionButtons.SetActive(false);
        OCMissionButtons.SetActive(false);
    }

    public void SelectNTAMission()
    {
        MainMenuButtons.SetActive(false);
        RaceButtons.SetActive(false);
        NTAMissionButtons.SetActive(true);
        OCMissionButtons.SetActive(false);
    }

    public void SelectOCMission()
    {
        MainMenuButtons.SetActive(false);
        RaceButtons.SetActive(false);
        NTAMissionButtons.SetActive(false);
        OCMissionButtons.SetActive(true);
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void GameScnesMission1()
    {
        SceneManager.LoadScene("Mission1");
    }
    public void GameScnesMission2()
    {
        SceneManager.LoadScene("Mission2");
    }
    public void GameScnesMission3()
    {
        SceneManager.LoadScene("Mission3");
    }
    public void GameScnesMission4()
    {
        SceneManager.LoadScene("Mission4");
    }
    public void GameScnesMission5()
    {
        SceneManager.LoadScene("Mission5");
    }
    public void GameScnesCutScence(int end)
    {
        Debug.Log("컷씬으로 이동");
        WhatMissionis = end;
        SceneManager.LoadScene("CutScene");
        Debug.Log(WhatMissionis);
    }
    public void GameScnesMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void WhatRaceis(int end)
    {
        WhatRaceisMission = end;
    }

    public void NextMission()
    {
        if(WhatRaceisMission == 0)
        {
            if (WhatMissionis == 0)
            {
                NTAWIn.SetActive(false);
                OCWIn.SetActive(false);
                Mission1ex.SetActive(true);
                Mission2ex.SetActive(false);
                Mission3ex.SetActive(false);
                Mission4ex.SetActive(false);
                Mission5ex.SetActive(false);
                NTAEnding.SetActive(false);
                OCEnding.SetActive(false);
            }
            if (WhatMissionis == 1)
            {
                NTAWIn.SetActive(false);
                OCWIn.SetActive(false);
                Mission1ex.SetActive(false);
                Mission2ex.SetActive(true);
                Mission3ex.SetActive(false);
                Mission4ex.SetActive(false);
                Mission5ex.SetActive(false);
                NTAEnding.SetActive(false);
                OCEnding.SetActive(false);
            }
            if (WhatMissionis == 2)
            {
                NTAWIn.SetActive(false);
                OCWIn.SetActive(false);
                Mission1ex.SetActive(false);
                Mission2ex.SetActive(false);
                Mission3ex.SetActive(true);
                Mission4ex.SetActive(false);
                Mission5ex.SetActive(false);
                NTAEnding.SetActive(false);
                OCEnding.SetActive(false);
            }
            if (WhatMissionis == 3)
            {
                NTAWIn.SetActive(false);
                OCWIn.SetActive(false);
                Mission1ex.SetActive(false);
                Mission2ex.SetActive(false);
                Mission3ex.SetActive(false);
                Mission4ex.SetActive(true);
                Mission5ex.SetActive(false);
                NTAEnding.SetActive(false);
                OCEnding.SetActive(false);
            }
            if (WhatMissionis == 4)
            {
                NTAWIn.SetActive(false);
                OCWIn.SetActive(false);
                Mission1ex.SetActive(false);
                Mission2ex.SetActive(false);
                Mission3ex.SetActive(false);
                Mission4ex.SetActive(false);
                Mission5ex.SetActive(true);
                NTAEnding.SetActive(false);
                OCEnding.SetActive(false);
            }
            if (WhatMissionis == 5)
            {
                NTAWIn.SetActive(false);
                OCWIn.SetActive(false);
                Mission1ex.SetActive(false);
                Mission2ex.SetActive(false);
                Mission3ex.SetActive(false);
                Mission4ex.SetActive(false);
                Mission5ex.SetActive(false);
                NTAEnding.SetActive(true);
                OCEnding.SetActive(false);
            }
        }
        else if(WhatRaceisMission == 1)
        {
            if (WhatMissionis == 0)
            {

            }
            if (WhatMissionis == 1)
            {

            }
            if (WhatMissionis == 2)
            {

            }
            if (WhatMissionis == 3)
            {

            }
            if (WhatMissionis == 4)
            {

            }
            if (WhatMissionis == 5)
            {
                NTAWIn.SetActive(false);
                OCWIn.SetActive(false);
                Mission1ex.SetActive(false);
                Mission2ex.SetActive(false);
                Mission3ex.SetActive(false);
                Mission4ex.SetActive(false);
                Mission5ex.SetActive(false);
                NTAEnding.SetActive(false);
                OCEnding.SetActive(true);
            }
        }
      
    }
}
