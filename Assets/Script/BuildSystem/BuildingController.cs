using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using FischlWorks_FogWar;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour
{
    [SerializeField]
    private GameObject buildingMarker;

    public Vector3 myRallyPoint;

    private Queue<UnitSpawnCommand> spawnQueue = new Queue<UnitSpawnCommand>();

    [SerializeField]
    private bool isSpawning = false;

    [SerializeField]
    private csFogWar fogWar = null;

    private UiButtonMapiing uiButtonMapiing;

    private List<int> unitQueue = new List<int>();
    private List<float> spawntimer = new List<float>();

    bool ismeseleceted;

    int UnitType;

    private UnitSpawner unitspawner;
    private RTSUnitController rtsunitcontroller;

    public int myBuildingType;

    public bool isRefund;

    public Vector3Int myGridpos;

    public Slider UnitSpawnSlider;

    public int MainBaseint;

    public void Start()
    {
        // This part is meant to be modified following the project's scene structure later...
        try
        {
            fogWar = GameObject.Find("FogWar").GetComponent<csFogWar>();
        }
        catch
        {
            Debug.LogErrorFormat("Failed to fetch csFogWar component. " +
                "Please rename the gameobject that the module is attachted to as \"FogWar\", " +
                "or change the implementation located in the csFogVisibilityAgent.cs script.");
        }

        try
        {
            uiButtonMapiing = GameObject.Find("RTSUnitControlSystem").GetComponent<UiButtonMapiing>();
        }
        catch
        {
            Debug.LogErrorFormat("Failed to fetch UiButtonMapiing component. " +
                "Please rename the gameobject that the module is attached to as \"RTSUnitControlSystem\", " +
                "or change the implementation located in the csFogVisibilityAgent.cs script.");
        }

        try
        {
            unitspawner = GameObject.Find("RTSUnitControlSystem").GetComponent<UnitSpawner>();
        }
        catch
        {
            Debug.LogErrorFormat("Failed to fetch UnitSpawner component. " +
                "Please rename the gameobject that the module is attached to as \"RTSUnitControlSystem\", " +
                "or change the implementation located in the csFogVisibilityAgent.cs script.");
        }

        try
        {
            rtsunitcontroller = GameObject.Find("RTSUnitControlSystem").GetComponent<RTSUnitController>();
        }
        catch
        {
            Debug.LogErrorFormat("Failed to fetch RTSUnitController component. " +
                "Please rename the gameobject that the module is attached to as \"RTSUnitControlSystem\", " +
                "or change the implementation located in the csFogVisibilityAgent.cs script.");
        }

        myRallyPoint = transform.position;
    }

    public void SelectBuilding()
    {
        // ���� ������ ��⿭�� UI�� ǥ��
        uiButtonMapiing.UnitQueue(unitQueue, -1);

        buildingMarker.SetActive(true);
        ismeseleceted = true;
    }

    public void DeselectBuilding()
    {
        buildingMarker.SetActive(false);
        ismeseleceted = false;
    }

    public void QueueUnitSpawn(Vector3 end, Vector3 rally, int whatU, float spawntime)
    {
        UiButtonMapiing uiButtonMapping = GameObject.Find("RTSUnitControlSystem").GetComponent<UiButtonMapiing>();
        if (uiButtonMapping == null)
        {
            Debug.LogError("UiButtonMapiing component not found.");
            return;
        }

        // ��⿭�� ���� á�� ��� �Լ� ����
        if (IsQueueFull())
        {
            Debug.Log("Queue is full. Command not added.");
            return;
        }

        // �ڿ� ���� ��� �� ���� ���� ���� Ȯ��
        switch (whatU)
        {
            case 0:
            case 1:
                uiButtonMapping.PriceCalculation(50, 0, 1);
                break;
            case 2:
                uiButtonMapping.PriceCalculation(75, 0, 2);
                break;
            case 3:
                uiButtonMapping.PriceCalculation(100, 50, 2);
                break;
            case 4:
            case 5:
                uiButtonMapping.PriceCalculation(150, 100, 2);
                break;
            case 6:
                uiButtonMapping.PriceCalculation(400, 300, 6);
                break;
            default:
                Debug.LogError("Invalid unit type.");
                return;
        }

        // �ڿ��� ������ ���, ��⿭�� �߰����� ����
        if (uiButtonMapping.SpawnOK != 1)
        {
            Debug.Log("Not enough resources to spawn unit.");
            return;
        }

        // ������ ��⿭�� �߰�
        spawnQueue.Enqueue(new UnitSpawnCommand(end, rally, whatU, spawntime));
        unitQueue.Add(whatU);
        spawntimer.Add(spawntime);

        Debug.Log("Current queue size: " + spawnQueue.Count);

        // ���õ� ���¶�� UI ������Ʈ
        if (ismeseleceted)
        {
            uiButtonMapping.UnitQueue(unitQueue, whatU);
        }

        // ��⿭ ó�� ���� �ƴϸ� �ڷ�ƾ ����
        if (!isSpawning)
        {
            UnitSpawnSlider.maxValue = spawntime;
            StartCoroutine(ProcessQueue());
        }
        else
        {
            Debug.Log("ProcessQueue is already running.");
        }
    }

    private IEnumerator ProcessQueue()
    {
        Debug.Log("Starting ProcessQueue");
        isSpawning = true;

        while (spawnQueue.Count > 0)
        {
            UnitSpawnCommand command = spawnQueue.Peek(); // ��⿭���� ������ �������� �������� ����
            UnitSpawnSlider.maxValue = command.SpawnTimer;

            // �����̴� ������Ʈ �ڷ�ƾ�� ����
            StartCoroutine(UpdateSlider(command.SpawnTimer));

            // ���� ������ �����ϰ� ��ٸ�
            yield return StartCoroutine(unitspawner.NTAUnitSpawn(command.EndPosition, command.RallyPoint, command.PrefabIndex));

            // ���� �� ��⿭���� ���� ����
            spawnQueue.Dequeue();
            unitQueue.RemoveAt(0);
            spawntimer.RemoveAt(0);

            // UI ������Ʈ
            if (ismeseleceted)
            {
                uiButtonMapiing.UnitQueue(unitQueue, -1);
            }

            Debug.Log("Remaining queue size: " + spawnQueue.Count);
        }

        Debug.Log("Ending ProcessQueue");
        isSpawning = false;
    }

    public bool IsQueueFull()
    {
        // �ִ� ��⿭ ũ�⸦ 4�� ����
        return spawnQueue.Count >= 4;
    }

    private IEnumerator UpdateSlider(float spawnTimer)
    {
        UnitSpawnSlider.maxValue = spawnTimer;
        UnitSpawnSlider.value = 0; // �����̴� �ʱ�ȭ

        float elapsedTime = 0;
        while (elapsedTime < spawnTimer)
        {
            elapsedTime += Time.deltaTime;
            UnitSpawnSlider.value = elapsedTime;

            yield return null; // ���� �����ӱ��� ���
        }

        UnitSpawnSlider.value = 0;
    }

    public void EyesOn()
    {
        Invoke(nameof(AddFOG), 0.4f);
    }

    public void AddFOG()
    {
        fogWar.AddFogRevealer(new csFogWar.FogRevealer(transform, 30, false));
    }

    public void RallyPoint(Vector3 end)
    {
        myRallyPoint = end;
    }

    public void RemoveBuilding()
    {
        // �ڽ��� �����Ǳ� ���� ������ ����
        if (unitspawner != null)
        {
            unitspawner.buildingList.Remove(this);
        }

        if (rtsunitcontroller != null)
        {
            rtsunitcontroller.selectedBuildingList.Remove(this);
        }

        if (transform.CompareTag("Supply Depot"))
        {
            uiButtonMapiing.maxPopulationCount -= 8;
        }
        if (transform.CompareTag("MainBase"))
        {
            rtsunitcontroller.MainBase.RemoveAt(MainBaseint);
        }

        if (isRefund)
        {
            uiButtonMapiing.RefundCalculation(myBuildingType);
        }

        GameObject.Find("PlacementSystem").GetComponent<PlacementSystem>().RemoveGrid(myGridpos, myBuildingType);

        if (fogWar != null)
        {
            // ���� fogRevealers ����Ʈ���� �ڽ��� ã�� �����մϴ�.
            for (int i = 0; i < fogWar._FogRevealers.Count; i++)
            {
                var fogRevealer = fogWar._FogRevealers[i];
                if (fogRevealer._RevealerTransform == transform)
                {
                    fogWar.RemoveFogRevealer(i); // �ڽ��� ����Ʈ���� ����
                    Debug.Log("Removed from FogRevealers");
                    break; // ���� �� ���� ����
                }
            }
        }

        Debug.Log("���");

        // ���� ������Ʈ �ı�
        Destroy(gameObject);
    }

}

public class UnitSpawnCommand
{
    public Vector3 EndPosition { get; private set; }
    public Vector3 RallyPoint { get; private set; }
    public int PrefabIndex { get; private set; }

    public float SpawnTimer { get; private set; }

    public UnitSpawnCommand(Vector3 endPosition, Vector3 rallyPoint, int prefabIndex, float spawntime)
    {
        EndPosition = endPosition;
        RallyPoint = rallyPoint;
        PrefabIndex = prefabIndex;
        SpawnTimer = spawntime;
    }
}