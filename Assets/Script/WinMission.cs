using System.Collections;
using UnityEngine;

public class WinMission : MonoBehaviour
{
    private UiButtonMapiing uibuttonmaping;

    public bool AnnihilationBattle;
    public bool Gatherresources;
    public bool Collectobjects;
    public bool Destroymainobject;
    public bool BuildingConstruct;

    public int GatherGAS;
    public int GatherResource;

    public bool isLapBuild = false;

    public int Winrate;

    private int rateCount = 0;

    public int whichThisMission; 

    public int WhatRace;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;

        try
        {
            uibuttonmaping = GameObject.Find("RTSUnitControlSystem").GetComponent<UiButtonMapiing>();
        }
        catch
        {
            Debug.LogErrorFormat("Failed to fetch csFogWar component. " +
                "Please rename the gameobject that the module is attached to as \"RTSunitControlSystem\", " +
                "or change the implementation located in the csFogVisibilityAgent.cs script.");
            return; // Early exit if fogWar is not found
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.I))
            {
                if (Input.GetKey(KeyCode.N))
                {
                    Debug.Log("½Â¸®!");
                    rateCount += 2;
                }
            }
        }

        if(rateCount >= Winrate)
        {
            uibuttonmaping.Winimage.SetActive(true);
            Time.timeScale = 0.01f;
            StartCoroutine(InvokeGameScnesCutScenceAfterDelay(0.03f));
        }

        if(Destroymainobject == true)
        {
            if (transform.GetComponent<EnemyAI>().MainObject == null)
            {
                rateCount += 1;
            }
        }

        if (AnnihilationBattle == true)
        {
            if (transform.GetComponent<EnemyAI>().Building.Count == 0)
            {
                rateCount += 1;
            }
        }

        if(Collectobjects == true)
        {
            rateCount += 1;
        }

        if(Gatherresources == true)
        {
            if(uibuttonmaping.Gas >= GatherGAS)
            {
                if(uibuttonmaping.Resource >= GatherResource)
                {
                    rateCount += 1;
                    Gatherresources = false;
                }
            }
        }
        
        if(BuildingConstruct == true)
        {
            if(isLapBuild == true)
            {
                rateCount += 1;
                BuildingConstruct = false;
            }
        }
    }

    IEnumerator InvokeGameScnesCutScenceAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(WhatRace == 0)
        {
            MainMenu.NTAMissionClear += 1;
        }
        if(WhatRace == 1)
        {
            MainMenu.OCMissionClear += 1;
        }
        transform.GetComponent<MainMenu>().GameScnesCutScence(whichThisMission);
    }
}
