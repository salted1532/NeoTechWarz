using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class RTSUnitController : MonoBehaviour
{

	[SerializeField]
	private UnitSpawner unitSpawner;
	[SerializeField]
	public List<UnitController> selectedUnitList;            // 플레이어가 클릭 or 드래그로 선택한 유닛

	public List<GameObject> MainBase;

	public GameObject MainBasePrefab;

	public GameObject StartingPoint;

	[SerializeField]
	public List<BuildingController> selectedBuildingList;

	private float waitTime; // 대기 시간 (초)
	[SerializeField]
	private float timer = 0f;

	public List<UnitController> UnitList { private set; get; }  // 맵에 존재하는 모든 유닛

	public List<BuildingController> BuildingList { private set; get; }

	public int SelectMode = 0;
	public int isUnitSelect = 0;
	public int isBuildingSelect = 0;
	public int MainBaseSelect = 0;
	public int BuildMode = 0;

	public int WhatTier = 0;

	private UiButtonMapiing uibuttonmaping;

	public Vector3 Spawnpos;
	public Vector3 Spawnrally;

	bool tierSet = false;

	public int RTSWhatUnit = 0;

	private int AttackUpgradeCount = 0;
	private int DefenseUpgradeCount = 0;
	private void Awake()
	{
		selectedUnitList = new List<UnitController>();
		selectedBuildingList = new List<BuildingController>();

		uibuttonmaping = GetComponent<UiButtonMapiing>();

		UnitList = unitSpawner.SpawnUnits(MainBasePrefab.transform.position, MainBasePrefab.transform.position, 7);

		BuildingList = unitSpawner.StartBuilding();

	}

    protected void Update()
	{
		if (selectedUnitList.Count == 0)
		{
			if (isBuildingSelect == 0)
			{
				SelectMode = 0;
			}

		}
		if (selectedBuildingList.Count == 0)
		{
			if (isUnitSelect == 0)
			{
				SelectMode = 0;
			}
		}

		if (SelectMode == 1)
		{
			if (isBuildingSelect == 1)
			{
				if (isUnitSelect == 0)
				{
					if (WhatTier == 1)
					{
						uibuttonmaping.Tier1On();
					}
					else if (WhatTier == 2)
					{
						uibuttonmaping.Tier2On();
					}
					else if (WhatTier == 3)
					{
						uibuttonmaping.Tier3On();
					}
					else if (WhatTier == 4)
					{
						if (MainBaseSelect == 1)
						{
							if (BuildMode == 1)
							{
								uibuttonmaping.BuildOn();
							}
							else if (BuildMode == 0)
							{
								uibuttonmaping.MainbaseOn();
							}
							else
							{
								uibuttonmaping.AllOff();
							}
						}
					}
					else if (WhatTier == 5)
                    {
						uibuttonmaping.SupplyDepotOn();

					}
					else if (WhatTier == 6)
                    {
						uibuttonmaping.LapOn();
					}
					else
					{
						uibuttonmaping.AllOff();
					}
				}
				else
				{
					uibuttonmaping.AllOff();
				}
			}
			else if (isUnitSelect == 1)
			{
				uibuttonmaping.UnitOn();
			}
			else
			{
				uibuttonmaping.AllOff();
			}
		}
		else
		{
			uibuttonmaping.AllOff();
		}
	}

	public void SpawnUnit(Vector3 end, Vector3 rally)
	{
		Debug.Log("RTS Vector3 :" + end);

		if (selectedBuildingList.Count == 0)
		{
			Debug.LogWarning("No buildings selected for spawning units.");
			return;
		}

		for (int i = 0; i < selectedBuildingList.Count; ++i)
		{
			BuildingController building = selectedBuildingList[i];

			// 선택된 건물과 유닛 타입에 따라 스폰 시간 결정
			switch (RTSWhatUnit)
			{
				case 0:
					waitTime = 12f;
					break;
				case 1:
					waitTime = 15f;
					break;
				case 2:
					waitTime = 19f;
					break;
				case 3:
					waitTime = 25f;
					break;
				case 4:
					waitTime = 31f;
					break;
				case 5:
					waitTime = 37f;
					break;
				case 6:
					waitTime = 63f;
					break;
				default:
					Debug.LogError("Invalid unit type.");
					continue; // 잘못된 유닛 타입인 경우 다음 루프로 이동
			}

			// 해당 건물의 QueueUnitSpawn 호출 전에 대기열이 꽉 차 있는지 확인
			if (building.IsQueueFull())
			{
				Debug.LogWarning($"Building {building.name}'s queue is full. Skipping unit spawn.");
				continue; // 대기열이 가득 차 있는 경우 다음 건물로 이동
			}

			// 선택된 건물의 대기열에 유닛 추가
			building.QueueUnitSpawn(end, rally, RTSWhatUnit, waitTime);

			Debug.Log($"Unit of type {RTSWhatUnit} added to building {building.name}'s queue with wait time {waitTime}.");
		}
	}
	/// <summary>
	/// 마우스 클릭으로 유닛을 선택할 때 호출
	/// </summary>
	public void ClickSelectUnit(UnitController newUnit)
	{
		int i = Random.Range(3, 7);
		newUnit.PlayVoice(i);
		// 기존에 선택되어 있는 모든 유닛 해제
		DeselectAll();
		SelectMode = 1;
		SelectUnit(newUnit);
	}

	/// <summary>
	/// Shift+마우스 클릭으로 유닛을 선택할 때 호출
	/// </summary>
	public void ShiftClickSelectUnit(UnitController newUnit)
	{
		// 기존에 선택되어 있는 유닛을 선택했으면
		if (selectedUnitList.Contains(newUnit))
		{
			DeselectUnit(newUnit);
		}
		// 새로운 유닛을 선택했으면
		else
		{
			SelectMode = 1;
			SelectUnit(newUnit);
		}
	}

	/// <summary>
	/// 마우스 드래그로 유닛을 선택할 때 호출
	/// </summary>
	public void DragSelectUnit(UnitController newUnit)
	{
		// 새로운 유닛을 선택했으면
		if (!selectedUnitList.Contains(newUnit))
		{
			SelectMode = 1;
			SelectUnit(newUnit);
		}
	}

	/// <summary>
	/// 모든 유닛의 선택을 해제할 때 호출
	/// </summary>
	public void DeselectAll()
	{
		if (BuildMode == 0)
		{
			SelectMode = 0;
			MainBaseSelect = 0;
			isUnitSelect = 0;
			isBuildingSelect = 0;
			WhatTier = 0;
			tierSet = false;

			//Debug.Log("디설렉 올");
			for (int i = 0; i < selectedUnitList.Count; ++i)
			{
				selectedUnitList[i].DeselectUnit();
			}
			for (int i = 0; i < selectedBuildingList.Count; ++i)
			{
				selectedBuildingList[i].DeselectBuilding();
			}

			selectedUnitList.Clear();
			selectedBuildingList.Clear();
		}
	}

	/// <summary>
	/// 매개변수로 받아온 newUnit 선택 설정
	/// </summary>
	private void SelectUnit(UnitController newUnit)
	{
		SelectMode = 1;
		isUnitSelect = 1;
		// 유닛이 선택되었을 때 호출하는 메소드
		newUnit.SelectUnit();
		// 선택한 유닛 정보를 리스트에 저장
		selectedUnitList.Add(newUnit);
	}

	/// <summary>
	/// 매개변수로 받아온 newUnit 선택 해제 설정
	/// </summary>
	private void DeselectUnit(UnitController newUnit)
	{
		// 유닛이 해제되었을 때 호출하는 메소드
		newUnit.DeselectUnit();
		// 선택한 유닛 정보를 리스트에서 삭제
		selectedUnitList.Remove(newUnit);
	}

	/// <summary>
	/// 선택된 유닛들 명령 구간
	/// </summary>
	public void MoveSelectedUnits(Vector3 end)
	{
		for (int i = 0; i < selectedUnitList.Count; ++i)
		{
			selectedUnitList[i].MoveTo(end);
		}

		int j = Random.Range(7, 11);
		selectedUnitList[0].PlayVoice(j);

	}

	public void AttackSelectedUnits(Vector3 end)
	{
		for (int i = 0; i < selectedUnitList.Count; ++i)
		{
			selectedUnitList[i].AttackToUnit(end);
		}

		int j = Random.Range(7, 11);
		selectedUnitList[0].PlayVoice(j);
	}

	public void AttackGroundSelectedUnits(Vector3 end)
	{
		for (int i = 0; i < selectedUnitList.Count; ++i)
		{
			selectedUnitList[i].AttackToGround(end);
		}

		int j = Random.Range(7, 11);
		selectedUnitList[0].PlayVoice(j);
	}

	public void StopSelectedUnits()
	{
		for (int i = 0; i < selectedUnitList.Count; ++i)
		{
			selectedUnitList[i].StopUnit();
		}
	}

	public void PatrolSelectedUnits(Vector3 end)
	{
		for (int i = 0; i < selectedUnitList.Count; ++i)
		{
			selectedUnitList[i].PatrolUnit(end);
		}

		int j = Random.Range(7, 11);
		selectedUnitList[0].PlayVoice(j);
	}

	public void HoldSelectedUnits()
	{
		for (int i = 0; i < selectedUnitList.Count; ++i)
		{
			selectedUnitList[i].HoldUnit();
		}
	}

	public void CollectionOreSelectedUnits(Vector3 end, bool gas)
	{
		Debug.Log("자원재취 명령");

		for (int i = 0; i < selectedUnitList.Count; ++i)
		{
			selectedUnitList[i].CollectOre(end, gas);
		}

		int j = Random.Range(7, 11);
		selectedUnitList[0].PlayVoice(j);
	}

	public void SetRallyPoint(Vector3 end)
	{
		for (int i = 0; i < selectedBuildingList.Count; ++i)
		{
			Spawnrally = end;
			selectedBuildingList[i].RallyPoint(end);
		}
	}

	public void SelectedUnitPlayVoice(int i)
	{
		if (SelectMode == 1)
		{
			if (isUnitSelect == 1)
			{
				selectedUnitList[0].PlayVoice(i);
			}
		}

	}
	//건물 선택 및 명령 구간

	public void ClickSelectBuilding(BuildingController newUnit)
	{
		if (BuildMode == 0)
		{
			DeselectAll();
			SelectMode = 1;
			SelectBuilding(newUnit);
		}
	}

	public void ShiftClickSelectBuilding(BuildingController newUnit)
	{
		if (BuildMode == 0)
		{
			// 기존에 선택되어 있는 유닛을 선택했으면
			if (selectedBuildingList.Contains(newUnit))
			{
				DeselectBuilding(newUnit);
			}
			// 새로운 유닛을 선택했으면
			else
			{
				SelectMode = 1;
				SelectBuilding(newUnit);
			}
		}
	}

	private void SelectBuilding(BuildingController newUnit)
	{
		if (BuildMode == 0)
		{
			SelectMode = 1;
			isBuildingSelect = 1;

			uibuttonmaping.BuildingPos(newUnit.transform.position, newUnit.myRallyPoint);

			Spawnpos = newUnit.transform.position;
			Spawnrally = newUnit.myRallyPoint;

			if (newUnit.CompareTag("Tier1"))
			{
				MainBaseSelect = 0;
				if (tierSet)
				{
					WhatTier = 0;
				}
				else
				{
					WhatTier = 1;
					tierSet = true;
				}

			}
			else if (newUnit.CompareTag("Tier2"))
			{
				MainBaseSelect = 0;
				if (tierSet)
				{
					WhatTier = 0;
				}
				else
				{
					WhatTier = 2;
					tierSet = true;
				}

			}
			else if (newUnit.CompareTag("Tier3"))
			{
				MainBaseSelect = 0;
				if (tierSet)
				{
					WhatTier = 0;
				}
				else
				{
					WhatTier = 3;
					tierSet = true;
				}

			}
			else if (newUnit.CompareTag("MainBase"))
			{
				MainBaseSelect = 1;
				if (tierSet)
				{
					WhatTier = 0;
				}
				else
				{
					WhatTier = 4;
					tierSet = true;
				}

			}
			else if (newUnit.CompareTag("Supply Depot"))
			{
				MainBaseSelect = 0;
				if (tierSet)
				{
					WhatTier = 0;
				}
				else
				{
					WhatTier = 5;
					tierSet = true;
				}

			}
			else if (newUnit.CompareTag("Lap"))
			{
				MainBaseSelect = 0;
				if (tierSet)
				{
					WhatTier = 0;
				}
				else
				{
					WhatTier = 6;
					tierSet = true;
				}

			}
			else
			{
				WhatTier = 0;
				MainBaseSelect = 0;
			}
			// 유닛이 선택되었을 때 호출하는 메소드
			newUnit.SelectBuilding();
			// 선택한 유닛 정보를 리스트에 저장
			selectedBuildingList.Add(newUnit);
		}
	}

	private void DeselectBuilding(BuildingController newUnit)
	{
		SelectMode = 0;
		// 유닛이 해제되었을 때 호출하는 메소드
		newUnit.DeselectBuilding();
		// 선택한 유닛 정보를 리스트에서 삭제
		selectedBuildingList.Remove(newUnit);
	}

	public void RefundBuilding()
	{
		for (int i = 0; i < selectedBuildingList.Count; ++i)
		{
			selectedBuildingList[i].isRefund = true;
			selectedBuildingList[i].RemoveBuilding();
		}
	}

	public void Upgraded(int end)
    {
		if(end == 1)
        {
			AttackUpgradeCount += 1;
			uibuttonmaping.PriceCalculation(100 * AttackUpgradeCount, 100 * AttackUpgradeCount, 0);
		}
		if (end == 2)
		{
			DefenseUpgradeCount += 1;
			uibuttonmaping.PriceCalculation(100 * DefenseUpgradeCount, 100 * DefenseUpgradeCount, 0);
		}

		
		StartCoroutine(DelayedExecution(end));
		
	}
	private IEnumerator DelayedExecution(int end)
	{
		// 30초 대기
		yield return new WaitForSeconds(250f);
		if(end == 1)
        {
			UnitHealth.UpgradedAttackPower += 1;
		}
		if (end == 2)
        {
			UnitHealth.UpgradedDefense += 1;
		}

		Debug.Log("업그레이드 완료");
	}
}
