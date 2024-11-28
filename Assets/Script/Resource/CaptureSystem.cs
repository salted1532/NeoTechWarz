using System.Collections.Generic;
using UnityEngine;

public class CaptureSystem : MonoBehaviour
{
    [SerializeField]
    private float OccupationTimer = 0;
    [SerializeField]
    private float EnemyoccupationTimer = 0;
    [SerializeField]
    private float recaptureTimer = 0;
    [SerializeField]
    private float ResourceTimer = 0;
    [SerializeField]
    private float EnemyrecaptureTimer = 0;
    [SerializeField]
    private bool PlayerCapture = false;
    [SerializeField]
    private bool EnemyCapture = false;
    [SerializeField]
    private bool neutralityCapture = true;

    UiButtonMapiing Uibutton;

    [SerializeField]
    private GameObject idleMarker;
    [SerializeField]
    private GameObject PlayerMarker;
    [SerializeField]
    private GameObject EnemyMarker;

    [SerializeField]
    private GameObject PlacementAreas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(OccupationTimer > 5f)
        {
            idleMarker.SetActive(false);
            PlayerMarker.SetActive(true);
            EnemyMarker.SetActive(false);
            PlacementAreas.SetActive(true);

            PlayerCapture = true;
            neutralityCapture = false;
            //Debug.Log("Á¡·ÉµÊ!!");
            OccupationTimer = 0;
        }

        if (EnemyoccupationTimer > 5f)
        {
            idleMarker.SetActive(false);
            PlayerMarker.SetActive(false);
            EnemyMarker.SetActive(true);
            PlacementAreas.SetActive(false);

            EnemyCapture = true;
            neutralityCapture = false;
            //Debug.Log("Á¡·ÉµÊ!!");
            EnemyoccupationTimer = 0;
        }

        if (recaptureTimer > 5f)
        {
            EnemyCapture = false;
            neutralityCapture = true;
            //Debug.Log("ÀçÅ»È¯µÊ!!");
            recaptureTimer = 0;
        }
        if (EnemyrecaptureTimer > 5f)
        {
            PlayerCapture = false;
            neutralityCapture = true;
            //Debug.Log("ÀçÅ»È¯µÊ!!");
            EnemyrecaptureTimer = 0;
        }

        if (neutralityCapture == true)
        {
            idleMarker.SetActive(true);
            PlayerMarker.SetActive(false);
            EnemyMarker.SetActive(false);
            PlacementAreas.SetActive(false);
        }
    }

    void OnTriggerStay(Collider other)
     {

         if (other.gameObject.CompareTag("Player"))
         {
             //Debug.Log("ÇÃ·¹ÀÌ¾î °¨ÁöµÊ");
             if (neutralityCapture == true)
             {
                 OccupationTimer += Time.deltaTime;
             }
             if (EnemyCapture == true)
             {
                 recaptureTimer += Time.deltaTime;
             }
         }
         else if (other.gameObject.CompareTag("Enemy"))
         {
             //Debug.Log("Àû °¨ÁöµÊ");
             if (neutralityCapture == true)
             {
                 EnemyoccupationTimer += Time.deltaTime;
             }
             if (PlayerCapture == true)
             {
                 EnemyrecaptureTimer += Time.deltaTime;
             }
         }
     }
}
