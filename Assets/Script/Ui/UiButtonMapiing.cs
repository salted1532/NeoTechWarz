using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiButtonMapiing : MonoBehaviour
{
    public List<Button> buttons;

    private List<Image> buttonImages;

    public List<Image> QueueImages;

    public List<Sprite> NTAUnitImages;
    public List<Sprite> OCUnitImages;

    public GameObject MainBaseinterface;
    public GameObject BuildingSelectinterface;
    public GameObject tier1interface;
    public GameObject tier2interface;
    public GameObject tier3interface;
    public GameObject Lapinterface;
    public GameObject Buildinterface;
    public GameObject UnitSelectinterface;
    public GameObject Queueinterface;

    public GameObject Optioninterface;
    public GameObject MissionObject;


    public TextMeshProUGUI ResourceText;
    public TextMeshProUGUI GasText;
    public TextMeshProUGUI PopulationText;

    public int Resource = 0;
    public int Gas = 0;
    public int Population = -1;

    public int SpawnOK = 0;

    [SerializeField]
    private GameObject placementsys;

    private RTSUnitController rtsunitcontroller;

    private UnitSpawner unitspawner;

    private Vector3 pos;

    public GameObject Winimage;

    public int maxPopulationCount = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonImages = new List<Image>();

        rtsunitcontroller = GetComponent<RTSUnitController>();
        unitspawner = GetComponent<UnitSpawner>();

        for (int i = 0; i < buttons.Count; ++i)
        {
            buttonImages.Add(buttons[i].GetComponent<Image>());
        }
    }

    public void Optionoff()
    {
        Optioninterface.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ResourceText.text = Resource.ToString();
        GasText.text = Gas.ToString();
        PopulationText.text = Population.ToString() + "/" + maxPopulationCount.ToString();

        if(Input.GetKeyDown(KeyCode.F10))
        {
            Optioninterface.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Optioninterface.SetActive(false);
        }

        if (transform.GetComponent<RTSUnitController>().SelectMode == 1)
        {
            if (rtsunitcontroller.isUnitSelect == 1)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    buttonImages[2].color = buttons[2].colors.pressedColor;
                }
                if (Input.GetKeyUp(KeyCode.A))
                {
                    buttonImages[2].color = buttons[2].colors.normalColor;
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    buttonImages[1].color = buttons[1].colors.pressedColor;
                }
                if (Input.GetKeyUp(KeyCode.S))
                {
                    buttonImages[1].color = buttons[1].colors.normalColor;
                }
                if (Input.GetKeyDown(KeyCode.H))
                {
                    buttonImages[6].color = buttons[6].colors.pressedColor;
                }
                if (Input.GetKeyUp(KeyCode.H))
                {
                    buttonImages[6].color = buttons[6].colors.normalColor;
                }
                if (Input.GetKeyDown(KeyCode.M))
                {
                    buttonImages[0].color = buttons[0].colors.pressedColor;
                }
                if (Input.GetKeyUp(KeyCode.M))
                {
                    buttonImages[0].color = buttons[0].colors.normalColor;
                }
                if (Input.GetKeyDown(KeyCode.P))
                {
                    buttonImages[5].color = buttons[5].colors.pressedColor;
                }
                if (Input.GetKeyUp(KeyCode.P))
                {
                    buttonImages[5].color = buttons[5].colors.normalColor;
                }
            }
            if (rtsunitcontroller.isBuildingSelect == 1)
            {

                if (Input.GetKeyDown(KeyCode.D))
                {
                    buttonImages[7].color = buttons[7].colors.pressedColor;
                }
                if (Input.GetKeyUp(KeyCode.D))
                {
                    buttonImages[7].color = buttons[7].colors.normalColor;
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    buttonImages[9].color = buttons[9].colors.pressedColor;
                }
                if (Input.GetKeyUp(KeyCode.A))
                {
                    buttonImages[9].color = buttons[9].colors.normalColor;
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    buttonImages[10].color = buttons[10].colors.pressedColor;
                }
                if (Input.GetKeyUp(KeyCode.S))
                {
                    buttonImages[10].color = buttons[10].colors.normalColor;
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    buttonImages[11].color = buttons[11].colors.pressedColor;
                }
                if (Input.GetKeyUp(KeyCode.I))
                {
                    buttonImages[11].color = buttons[11].colors.normalColor;
                }
                if (Input.GetKeyDown(KeyCode.P))
                {
                    buttonImages[12].color = buttons[12].colors.pressedColor;
                }
                if (Input.GetKeyUp(KeyCode.P))
                {
                    buttonImages[12].color = buttons[12].colors.normalColor;
                }
                if (Input.GetKeyDown(KeyCode.B))
                {
                    buttonImages[15].color = buttons[15].colors.pressedColor;
                }
                if (Input.GetKeyUp(KeyCode.B))
                {
                    buttonImages[15].color = buttons[15].colors.normalColor;
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if(rtsunitcontroller.BuildMode == 1)
                    {
                        buttonImages[16].color = buttons[16].colors.pressedColor;
                    }
                    else
                    {
                        buttonImages[13].color = buttons[13].colors.pressedColor;
                    }
                    
                }
                if (Input.GetKeyUp(KeyCode.F))
                {
                    if (rtsunitcontroller.BuildMode == 1)
                    {
                        buttonImages[16].color = buttons[16].colors.normalColor;
                    }
                    else
                    {
                        buttonImages[13].color = buttons[13].colors.pressedColor;
                    }
                        
                }
                if (Input.GetKeyDown(KeyCode.G))
                {
                    if (rtsunitcontroller.BuildMode == 1)
                    {
                        buttonImages[17].color = buttons[17].colors.pressedColor;
                    }
                    else
                    {
                        buttonImages[14].color = buttons[14].colors.pressedColor;
                    }
                        
                }
                if (Input.GetKeyUp(KeyCode.G))
                {
                    if (rtsunitcontroller.BuildMode == 1)
                    {
                        buttonImages[17].color = buttons[17].colors.normalColor;
                    }
                    else
                    {
                        buttonImages[14].color = buttons[14].colors.pressedColor;
                    }
                }
            }
        }
    }
    public void MissionObjectOn()
    {
        MissionObject.SetActive(true);
        Optioninterface.SetActive(false);
    }
    public void BackToOption()
    {
        MissionObject.SetActive(false);
        Optioninterface.SetActive(true);
    }

    public void BuildingPos(Vector3 end, Vector3 rally)
    {
        pos = end;
        Debug.Log("Vector3 값: " + pos);
    }

    public void WorkerSpawn()
    {
        transform.GetComponent<RTSUnitController>().RTSWhatUnit = 0;
        transform.GetComponent<RTSUnitController>().SpawnUnit(pos, rtsunitcontroller.Spawnrally);
    }

    public void Tier11Spawn()
    {
        transform.GetComponent<RTSUnitController>().RTSWhatUnit = 1;
        transform.GetComponent<RTSUnitController>().SpawnUnit(pos, rtsunitcontroller.Spawnrally);
    }

    public void Tier12Spawn()
    {
        transform.GetComponent<RTSUnitController>().RTSWhatUnit = 2;
        transform.GetComponent<RTSUnitController>().SpawnUnit(pos, rtsunitcontroller.Spawnrally);
    }

    public void Tier21Spawn()
    {
        transform.GetComponent<RTSUnitController>().RTSWhatUnit = 3;
        transform.GetComponent<RTSUnitController>().SpawnUnit(pos, rtsunitcontroller.Spawnrally);
    }

    public void Tier22Spawn()
    {
        transform.GetComponent<RTSUnitController>().RTSWhatUnit = 4;
        transform.GetComponent<RTSUnitController>().SpawnUnit(pos, rtsunitcontroller.Spawnrally);
    }

    public void Tier31Spawn()
    {
        transform.GetComponent<RTSUnitController>().RTSWhatUnit = 5;
        transform.GetComponent<RTSUnitController>().SpawnUnit(pos, rtsunitcontroller.Spawnrally);
    }

    public void Tier32Spawn()
    {
        transform.GetComponent<RTSUnitController>().RTSWhatUnit = 6;
        transform.GetComponent<RTSUnitController>().SpawnUnit(pos, rtsunitcontroller.Spawnrally);
    }


    public void PriceCalculation(int min, int gas, int pop)
    {
        if(Resource >= min)
        {
            if(Gas >= gas)
            {
                if((Population + pop) <= maxPopulationCount)
                {
                    Resource -= min;
                    Gas -= gas;
                    Population += pop;
                    SpawnOK = 1;
                }
                else
                {
                    SpawnOK = 0;
                }
            }
            else
            {
                SpawnOK = 0;
            }
        }
        else
        {
            SpawnOK = 0;
        }
    }

    public void RefundCalculation(int whatB)
    {
        if (whatB == 0)
        {
            Resource += 75;
        }
        else if (whatB == 1)
        {
            Resource += 100;
            Gas += 50;

        }
        else if (whatB == 2)
        {
            Resource += 75;
            Gas += 50;

        }
        else if (whatB == 3)
        {
            Resource += 200;

        }
        else if (whatB == 4)
        {
            Resource += 50;

        }
        else if (whatB == 5)
        {
            Resource += 65;

        }
        else if (whatB == 6)
        {

        }
        else
        {
            Debug.LogError("Unhandled selectedOnbjectIndex value: " + whatB);
            return;
        }
    }

    public void UnitQueue(List<int> queue, int whatU)
    {

        for (int i = 0; i < QueueImages.Count; i++)
        {
            if (i < queue.Count)
            {
                int unitType = queue[i];
                QueueImages[i].sprite = NTAUnitImages[unitType];
            }
            else
            {
                QueueImages[i].sprite = null; // 빈 슬롯은 null 이미지로 처리
            }
        }
    }

    public void MainbaseOn()
    {
        MainBaseinterface.SetActive(true);
        tier1interface.SetActive(false);
        tier2interface.SetActive(false);
        tier3interface.SetActive(false);
        Buildinterface.SetActive(false);
        UnitSelectinterface.SetActive(false);
        Queueinterface.SetActive(true);
        BuildingSelectinterface.SetActive(true);
        Lapinterface.SetActive(false);
    }
    public void Tier1On()
    {
        MainBaseinterface.SetActive(false);
        tier1interface.SetActive(true);
        tier2interface.SetActive(false);
        tier3interface.SetActive(false);
        Buildinterface.SetActive(false);
        UnitSelectinterface.SetActive(false);
        Queueinterface.SetActive(true);
        BuildingSelectinterface.SetActive(true);
        Lapinterface.SetActive(false);
    }
    public void Tier2On()
    {
        MainBaseinterface.SetActive(false);
        tier1interface.SetActive(false);
        tier2interface.SetActive(true);
        tier3interface.SetActive(false);
        Buildinterface.SetActive(false);
        UnitSelectinterface.SetActive(false);
        Queueinterface.SetActive(true);
        BuildingSelectinterface.SetActive(true);
        Lapinterface.SetActive(false);
    }
    public void Tier3On()
    {
        MainBaseinterface.SetActive(false);
        tier1interface.SetActive(false);
        tier2interface.SetActive(false);
        tier3interface.SetActive(true);
        Buildinterface.SetActive(false);
        UnitSelectinterface.SetActive(false);
        Queueinterface.SetActive(true);
        BuildingSelectinterface.SetActive(true);
        Lapinterface.SetActive(false);
    }
    public void SupplyDepotOn()
    {
        MainBaseinterface.SetActive(false);
        tier1interface.SetActive(false);
        tier2interface.SetActive(false);
        tier3interface.SetActive(false);
        Buildinterface.SetActive(false);
        UnitSelectinterface.SetActive(false);
        Queueinterface.SetActive(false);
        BuildingSelectinterface.SetActive(true);
        Lapinterface.SetActive(false);
    }
    public void LapOn()
    {
        MainBaseinterface.SetActive(false);
        tier1interface.SetActive(false);
        tier2interface.SetActive(false);
        tier3interface.SetActive(false);
        Buildinterface.SetActive(false);
        UnitSelectinterface.SetActive(false);
        Queueinterface.SetActive(false);
        BuildingSelectinterface.SetActive(true);
        Lapinterface.SetActive(true);
    }
    public void BuildOn()
    {
        MainBaseinterface.SetActive(false);
        tier1interface.SetActive(false);
        tier2interface.SetActive(false);
        tier3interface.SetActive(false);
        Buildinterface.SetActive(true);
        UnitSelectinterface.SetActive(false);
        Queueinterface.SetActive(false);
        BuildingSelectinterface.SetActive(false);
        Lapinterface.SetActive(false);
    }
    public void UnitOn()
    {
        MainBaseinterface.SetActive(false);
        tier1interface.SetActive(false);
        tier2interface.SetActive(false);
        tier3interface.SetActive(false);
        Buildinterface.SetActive(false);
        UnitSelectinterface.SetActive(true);
        Queueinterface.SetActive(false);
        BuildingSelectinterface.SetActive(false);
        Lapinterface.SetActive(false);
    }
    public void AllOff()
    {
        MainBaseinterface.SetActive(false);
        tier1interface.SetActive(false);
        tier2interface.SetActive(false);
        tier3interface.SetActive(false);
        Buildinterface.SetActive(false);
        UnitSelectinterface.SetActive(false);
        Queueinterface.SetActive(false);
        BuildingSelectinterface.SetActive(false);
        Lapinterface.SetActive(false);
    }
}
    
