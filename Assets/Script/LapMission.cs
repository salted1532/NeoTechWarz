using UnityEngine;

public class LapMission : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject.Find("EnemyAi").GetComponent<WinMission>().isLapBuild = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
