using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> NTAPrefab;

	[SerializeField]
	private List<GameObject> OCPrefab;

	public List<UnitController> unitList = new List<UnitController>();

	public List<BuildingController> buildingList = new List<BuildingController>();

	private RTSUnitController rtsUnitController;

	public int WhatRace;

	[SerializeField]
	public float spawntime = 0;

	private void Awake()
	{
		List<UnitController> unitList = new List<UnitController>();
		List<BuildingController> buildingList = new List<BuildingController>();
	}

	void Start()
    {
		rtsUnitController = GetComponent<RTSUnitController>();

		GameObject mainbase = Instantiate(rtsUnitController.MainBasePrefab,rtsUnitController.StartingPoint.transform.position, Quaternion.identity);
		BuildingController building = mainbase.GetComponent<BuildingController>();

		rtsUnitController.MainBase.Add(mainbase);

		building.EyesOn();

		buildingList.Add(building);

		for(int i = 0;i < 4; i++)
        {
			if(WhatRace == 0)
            {
				GameObject clone = Instantiate(NTAPrefab[0], rtsUnitController.StartingPoint.transform.position, Quaternion.identity);
				UnitController unit = clone.GetComponent<UnitController>();

				transform.GetComponent<UiButtonMapiing>().Population += 1;

				unitList.Add(unit);
			}
			else if(WhatRace == 1)
            {
				GameObject clone = Instantiate(OCPrefab[0], rtsUnitController.StartingPoint.transform.position, Quaternion.identity);
				UnitController unit = clone.GetComponent<UnitController>();

				transform.GetComponent<UiButtonMapiing>().Population += 1;

				unitList.Add(unit);
			}
		}
	}

	public List<UnitController> SpawnUnits(Vector3 end, Vector3 rally, int whatU)
	{
		if(whatU == 0)
        {
			transform.GetComponent<UiButtonMapiing>().PriceCalculation(50, 0, 1);
			spawntime = 12f;
		}
		else if(whatU == 1)
        {
			transform.GetComponent<UiButtonMapiing>().PriceCalculation(50, 0, 1);
			spawntime = 15f;
		}
		else if (whatU == 2)
		{
			transform.GetComponent<UiButtonMapiing>().PriceCalculation(75, 0, 2);
			spawntime = 19f;
		}
		else if (whatU == 3)
		{
			transform.GetComponent<UiButtonMapiing>().PriceCalculation(100, 50, 2);
			spawntime = 25f;
		}
		else if (whatU == 4)
		{
			transform.GetComponent<UiButtonMapiing>().PriceCalculation(150, 100, 2);
			spawntime = 31f;
		}
		else if (whatU == 5)
		{
			transform.GetComponent<UiButtonMapiing>().PriceCalculation(150, 100, 2);
			spawntime = 37f;
		}
		else if (whatU == 6)
		{
			transform.GetComponent<UiButtonMapiing>().PriceCalculation(400, 300, 6);
			spawntime = 63f;
		}
		if (transform.GetComponent<UiButtonMapiing>().SpawnOK == 1)
        {
			StartCoroutine(NTAUnitSpawn(end, rally, whatU));
		}
		return unitList;
	}



	public IEnumerator NTAUnitSpawn(Vector3 end, Vector3 rally, int whatU)
	{
		// 유닛에 따라 가격 계산과 스폰 시간 설정
		if (whatU == 0)
		{
			spawntime = 12f;
		}
		else if (whatU == 1)
		{
			spawntime = 15f;
		}
		else if (whatU == 2)
		{
			spawntime = 19f;
		}
		else if (whatU == 3)
		{
			spawntime = 25f;
		}
		else if (whatU == 4)
		{
			spawntime = 31f;
		}
		else if (whatU == 5)
		{

			spawntime = 37f;
		}
		else if (whatU == 6)
		{
			spawntime = 63f;
		} 

		// spawntime 값 확인을 위한 로그 출력
		Debug.Log("Spawntime: " + spawntime);

		if (transform.GetComponent<UiButtonMapiing>().SpawnOK == 1)
		{
			yield return new WaitForSeconds(spawntime);

			GameObject clone = Instantiate(NTAPrefab[whatU], end, Quaternion.identity);

			UnitController unit = clone.GetComponent<UnitController>();

			unit.myUnitType = whatU;

			unit.GoToRallyPoint(rally);

			unitList.Add(unit);
		}
	}

	public IEnumerator OCUnitSpawn(Vector3 end, Vector3 rally, int whatU)
	{
		if (whatU == 0)
		{
			transform.GetComponent<UiButtonMapiing>().PriceCalculation(50, 0, 1);
			spawntime = 12f;
		}
		else if (whatU == 1)
		{
			transform.GetComponent<UiButtonMapiing>().PriceCalculation(50, 0, 1);
			spawntime = 15f;
		}
		else if (whatU == 2)
		{
			transform.GetComponent<UiButtonMapiing>().PriceCalculation(75, 0, 2);
			spawntime = 19f;
		}
		else if (whatU == 3)
		{
			transform.GetComponent<UiButtonMapiing>().PriceCalculation(100, 50, 2);
			spawntime = 25f;
		}
		else if (whatU == 4)
		{
			transform.GetComponent<UiButtonMapiing>().PriceCalculation(150, 100, 2);
			spawntime = 31f;
		}
		else if (whatU == 5)
		{
			transform.GetComponent<UiButtonMapiing>().PriceCalculation(150, 100, 2);
			spawntime = 37f;
		}
		else if (whatU == 6)
		{
			transform.GetComponent<UiButtonMapiing>().PriceCalculation(400, 300, 6);
			spawntime = 63f;
		}

		yield return new WaitForSeconds(spawntime);

		GameObject clone = Instantiate(OCPrefab[whatU], end, Quaternion.identity);

		UnitController unit = clone.GetComponent<UnitController>();

		unit.GoToRallyPoint(rally);

		unitList.Add(unit);
	}

	public List<BuildingController> StartBuilding()
    {
		return buildingList;
	}
}
