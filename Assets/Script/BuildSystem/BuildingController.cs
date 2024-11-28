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
        // 현재 빌딩의 대기열을 UI에 표시
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

        // 대기열이 가득 찼을 경우 함수 종료
        if (IsQueueFull())
        {
            Debug.Log("Queue is full. Command not added.");
            return;
        }

        // 자원 가격 계산 및 스폰 가능 여부 확인
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

        // 자원이 부족할 경우, 대기열에 추가하지 않음
        if (uiButtonMapping.SpawnOK != 1)
        {
            Debug.Log("Not enough resources to spawn unit.");
            return;
        }

        // 유닛을 대기열에 추가
        spawnQueue.Enqueue(new UnitSpawnCommand(end, rally, whatU, spawntime));
        unitQueue.Add(whatU);
        spawntimer.Add(spawntime);

        Debug.Log("Current queue size: " + spawnQueue.Count);

        // 선택된 상태라면 UI 업데이트
        if (ismeseleceted)
        {
            uiButtonMapping.UnitQueue(unitQueue, whatU);
        }

        // 대기열 처리 중이 아니면 코루틴 시작
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
            UnitSpawnCommand command = spawnQueue.Peek(); // 대기열에서 유닛을 꺼내지만 제거하지 않음
            UnitSpawnSlider.maxValue = command.SpawnTimer;

            // 슬라이더 업데이트 코루틴을 시작
            StartCoroutine(UpdateSlider(command.SpawnTimer));

            // 유닛 스폰을 진행하고 기다림
            yield return StartCoroutine(unitspawner.NTAUnitSpawn(command.EndPosition, command.RallyPoint, command.PrefabIndex));

            // 스폰 후 대기열에서 유닛 제거
            spawnQueue.Dequeue();
            unitQueue.RemoveAt(0);
            spawntimer.RemoveAt(0);

            // UI 업데이트
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
        // 최대 대기열 크기를 4로 가정
        return spawnQueue.Count >= 4;
    }

    private IEnumerator UpdateSlider(float spawnTimer)
    {
        UnitSpawnSlider.maxValue = spawnTimer;
        UnitSpawnSlider.value = 0; // 슬라이더 초기화

        float elapsedTime = 0;
        while (elapsedTime < spawnTimer)
        {
            elapsedTime += Time.deltaTime;
            UnitSpawnSlider.value = elapsedTime;

            yield return null; // 다음 프레임까지 대기
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
        // 자신이 삭제되기 전에 참조를 해제
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
            // 현재 fogRevealers 리스트에서 자신을 찾아 제거합니다.
            for (int i = 0; i < fogWar._FogRevealers.Count; i++)
            {
                var fogRevealer = fogWar._FogRevealers[i];
                if (fogRevealer._RevealerTransform == transform)
                {
                    fogWar.RemoveFogRevealer(i); // 자신을 리스트에서 제거
                    Debug.Log("Removed from FogRevealers");
                    break; // 제거 후 루프 종료
                }
            }
        }

        Debug.Log("사망");

        // 게임 오브젝트 파괴
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