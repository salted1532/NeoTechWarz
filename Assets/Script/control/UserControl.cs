using UnityEngine;
using UnityEngine.EventSystems;
public class UserControl : MonoBehaviour
{
	[SerializeField]
	private LayerMask layerUnit;
	[SerializeField]
	private LayerMask layerGround;
	[SerializeField]
	private LayerMask layerEnemy;
	[SerializeField]
	private LayerMask layerBuilding;
	[SerializeField]
	private LayerMask layerOre;
	[SerializeField]
	private Camera mainCamera;
	[SerializeField]
	private GameObject pointerPrefab;
	[SerializeField]
	private GameObject AttackPointPrefab;

	private RTSUnitController rtsUnitController;

	private UnitSpawner unitspawner;

	[SerializeField]
	private RectTransform dragRectangle;            // 마우스로 드래그한 범위를 가시화하는 Image UI의 RectTransform

	private Rect dragRect;              // 마우스로 드래그 한 범위 (xMin~xMax, yMin~yMax)
	private Vector2 start = Vector2.zero;   // 드래그 시작 위치
	private Vector2 end = Vector2.zero;     // 드래그 종료 위치

	private Vector3 mousepos;

	[SerializeField]
	private bool attackMode = false;
	[SerializeField]
	private bool moveMode = false;
	[SerializeField]
	private bool patrolMode = false;
	[SerializeField]
	private bool rallyMode = false;
	[SerializeField]
	private bool modeOn = false;

	private GameObject basicpointer;

	public PlacementSystem placementsystem;
	private void Awake()
	{
		mainCamera = Camera.main;
		rtsUnitController = GetComponent<RTSUnitController>();

		unitspawner = GetComponent<UnitSpawner>();

		// start, end가 (0, 0)인 상태로 이미지의 크기를 (0, 0)으로 설정해 화면에 보이지 않도록 함
		DrawDragRectangle();
	}

	private void Update()
	{
		// 마우스 왼쪽 클릭으로 유닛 선택 or 해제 / 마우스 드래그 이미지 시작지점 계산
		if (Input.GetMouseButtonDown(0))
		{
			//마우스 드래그 시작지점 지정
			start = Input.mousePosition;
			dragRect = new Rect();
			if (EventSystem.current.IsPointerOverGameObject())
			{
				return;
			}
			else
            {
				RaycastHit hit;
				Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

				// 광선에 부딪히는 오브젝트가 있을 때 (=유닛을 클릭했을 때)
				if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerUnit))
				{
					// 충돌한 오브젝트에 대해 처리
					if (hit.transform.GetComponent<UnitController>() == null) return;

					if (moveMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							Debug.Log("M키 이동");
							moveMode = false;
							modeOn = false;
							rtsUnitController.MoveSelectedUnits(hit.point);
							GameObject Pointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);
						}
					}
					if (attackMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							Debug.Log("A키 적공격");
							attackMode = false;
							modeOn = false;
							rtsUnitController.AttackGroundSelectedUnits(hit.point);
							GameObject Pointer = Instantiate(AttackPointPrefab, hit.point, Quaternion.identity);
						}
					}
					if (patrolMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							Debug.Log("P키 순찰");
							patrolMode = false;
							modeOn = false;
							rtsUnitController.PatrolSelectedUnits(hit.point);
							GameObject Pointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);

						}
					}
					if (rallyMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							if(rtsUnitController.isBuildingSelect == 1)
                            {
								Debug.Log("Y키 랠리지정");
								rallyMode = false;
								modeOn = false;
								rtsUnitController.SetRallyPoint(hit.point);
								GameObject Pointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);
							}
							
						}
					}
					if(modeOn == false) 
					{
						if (Input.GetKey(KeyCode.LeftShift))
						{
							rtsUnitController.ShiftClickSelectUnit(hit.transform.GetComponent<UnitController>());
						}
						else
						{
							Debug.Log("클릭 유닛");
							rtsUnitController.ClickSelectUnit(hit.transform.GetComponent<UnitController>());
						}
					}

				}
				// 광선에 부딪히는 오브젝트가 있을 때 (=유닛을 클릭했을 때)
				else if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerBuilding))
				{
					// 충돌한 오브젝트에 대해 처리
					if (hit.transform.GetComponent<BuildingController>() == null) return;

					if (attackMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							Debug.Log("바닥공격");
							attackMode = false;
							modeOn = false;
							rtsUnitController.AttackGroundSelectedUnits(hit.point);
							GameObject Pointer = Instantiate(AttackPointPrefab, hit.point, Quaternion.identity);

						}
					}
					if (patrolMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							Debug.Log("P키 순찰");
							patrolMode = false;
							modeOn = false;
							rtsUnitController.PatrolSelectedUnits(hit.point);
							GameObject Pointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);

						}
					}
					if (moveMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							Debug.Log("M키 이동");
							moveMode = false;
							modeOn = false;
							rtsUnitController.MoveSelectedUnits(hit.point);
							GameObject Pointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);
						}
					}
					if (rallyMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							if (rtsUnitController.isBuildingSelect == 1)
							{
								Debug.Log("Y키 랠리지정");
								rallyMode = false;
								modeOn = false;
								rtsUnitController.SetRallyPoint(hit.point);
								GameObject Pointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);
							}

						}
					}
					if (modeOn == false)
                    {
						if (Input.GetKey(KeyCode.LeftShift))
						{
							rtsUnitController.ShiftClickSelectBuilding(hit.transform.GetComponent<BuildingController>());
						}
						else
						{
							rtsUnitController.ClickSelectBuilding(hit.transform.GetComponent<BuildingController>());
						}
					}
				}
				// 광선에 부딪히는 오브젝트가 없을 때
				else
				{
					if (!Input.GetKey(KeyCode.LeftShift))
					{
						if(moveMode == false)
                        {
							if (attackMode == false)
							{
								if (patrolMode == false)
								{
									if (rallyMode == false)
                                    {
										rtsUnitController.DeselectAll();
									}
								}
							}
						}
					}

				}
				//광선에 부딪히는 오브젝트가 있을 때 (=적유닛을 클릭했을 때)
				if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerEnemy))
				{
					if (attackMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							Debug.Log("A키 적공격");
							attackMode = false;
							modeOn = false;
							rtsUnitController.AttackGroundSelectedUnits(hit.point);
							GameObject Pointer = Instantiate(AttackPointPrefab, hit.point, Quaternion.identity);
						}
					}
					if (patrolMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							Debug.Log("P키 순찰");
							patrolMode = false;
							modeOn = false;
							rtsUnitController.PatrolSelectedUnits(hit.point);
							GameObject Pointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);

						}
					}
					if (moveMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							Debug.Log("M키 이동");
							moveMode = false;
							modeOn = false;
							rtsUnitController.MoveSelectedUnits(hit.point);
							GameObject Pointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);
						}
					}
					if (rallyMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							if (rtsUnitController.isBuildingSelect == 1)
							{
								Debug.Log("Y키 랠리지정");
								rallyMode = false;
								modeOn = false;
								rtsUnitController.SetRallyPoint(hit.point);
								GameObject Pointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);
							}

						}
					}
				}
				//광선에 부딪히는 오브젝트가 없을 때 (=바닥을 클릭했을 때)
				if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerGround))
				{
					if (attackMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							Debug.Log("바닥공격");
							attackMode = false;
							modeOn = false;
							rtsUnitController.AttackGroundSelectedUnits(hit.point);
							GameObject Pointer = Instantiate(AttackPointPrefab, hit.point, Quaternion.identity);

						}
					}
					if (patrolMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							Debug.Log("P키 순찰");
							patrolMode = false;
							modeOn = false;
							rtsUnitController.PatrolSelectedUnits(hit.point);
							GameObject Pointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);

						}
					}
					if (moveMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							Debug.Log("M키 이동");
							moveMode = false;
							modeOn = false;
							rtsUnitController.MoveSelectedUnits(hit.point);
							GameObject Pointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);
						}
					}
					if (rallyMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							if (rtsUnitController.isBuildingSelect == 1)
							{
								Debug.Log("Y키 랠리지정");
								rallyMode = false;
								modeOn = false;
								rtsUnitController.SetRallyPoint(hit.point);
								GameObject Pointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);
							}

						}
					}
				}
			}
		}

		if (Input.GetMouseButton(0))
		{
			end = Input.mousePosition;

			// 마우스를 클릭한 상태로 드래그 하는 동안 드래그 범위를 이미지로 표현
			DrawDragRectangle();
		}

		if (Input.GetMouseButtonUp(0))
		{
			// 마우스 클릭을 종료할 때 드래그 범위 내에 있는 유닛 선택
			CalculateDragRect();
			SelectUnits();

			// 마우스 클릭을 종료할 때 드래그 범위가 보이지 않도록
			// start, end 위치를 (0, 0)으로 설정하고 드래그 범위를 그린다
			start = end = Vector2.zero;
			DrawDragRectangle();
		}

		// 마우스 오른쪽 클릭으로 유닛 이동
		if (Input.GetMouseButtonDown(1))
		{
			RaycastHit hit;
			Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

			attackMode = false;
			patrolMode = false;
			moveMode = false;
			rallyMode = false;
			modeOn = false;

			if (transform.GetComponent<RTSUnitController>().SelectMode == 1)
			{
				if(transform.GetComponent<RTSUnitController>().isUnitSelect == 1)
                {
					// 유닛 오브젝트(layerUnit)를 클릭했을 때
					if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerEnemy))
					{
						Debug.Log("그냥 적공격");
						rtsUnitController.AttackGroundSelectedUnits(hit.point);
						GameObject Pointer = Instantiate(AttackPointPrefab, hit.point, Quaternion.identity);
					}
					else if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerOre))
					{
						if (hit.collider.gameObject.tag == "Gas")
						{
							bool isGas = true;
							rtsUnitController.CollectionOreSelectedUnits(hit.point, isGas);
							GameObject Pointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);
						}
						else
						{
							bool isGas = false;
							rtsUnitController.CollectionOreSelectedUnits(hit.point, isGas);
							GameObject Pointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);
						}

					}
					else if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerGround))
					{
						rtsUnitController.MoveSelectedUnits(hit.point);
						GameObject Pointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);
					}
				}
				else if(transform.GetComponent<RTSUnitController>().isBuildingSelect == 1)
                {
					if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerEnemy))
					{
						rtsUnitController.SetRallyPoint(hit.point);
						GameObject Pointer = Instantiate(AttackPointPrefab, hit.point, Quaternion.identity);
					}
					else if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerOre))
					{
						rtsUnitController.SetRallyPoint(hit.point);
						GameObject Pointer = Instantiate(AttackPointPrefab, hit.point, Quaternion.identity);
					}
					else if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerGround))
					{
						rtsUnitController.SetRallyPoint(hit.point);
						GameObject Pointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);
					}
				}
			}


		}
		//건설모드
		if (Input.GetKeyDown(KeyCode.V))
		{
			if (rtsUnitController.MainBaseSelect == 1)
			{
				rtsUnitController.BuildMode = 1;
				placementsystem.StartPlacement(7);
			}
		}

		if (Input.GetKeyDown(KeyCode.A))
        {
			if (rtsUnitController.SelectMode == 1)
			{
				if(rtsUnitController.isUnitSelect == 0)
                {
					if (rtsUnitController.isBuildingSelect == 1)
					{
						if (rtsUnitController.WhatTier == 1)
						{
							rtsUnitController.RTSWhatUnit = 1;
							rtsUnitController.SpawnUnit(rtsUnitController.Spawnpos, rtsUnitController.Spawnrally);
						}
					}
				}
				if(rtsUnitController.isUnitSelect == 1)
                {
					attackMode = true;
					patrolMode = false;
					moveMode = false;
					rallyMode = false;
					modeOn = true;
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.C))
		{
			if (rtsUnitController.SelectMode == 1)
			{
				if (rtsUnitController.isBuildingSelect == 1)
				{
					if (rtsUnitController.WhatTier == 1)
					{
						rtsUnitController.RTSWhatUnit = 1;
						rtsUnitController.SpawnUnit(rtsUnitController.Spawnpos, rtsUnitController.Spawnrally);
					}
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			if (rtsUnitController.SelectMode == 1)
			{
				if (rtsUnitController.isBuildingSelect == 1)
				{
					if (rtsUnitController.WhatTier == 2)
					{
						rtsUnitController.RTSWhatUnit = 3;
						rtsUnitController.SpawnUnit(rtsUnitController.Spawnpos, rtsUnitController.Spawnrally);
					}
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			if (rtsUnitController.SelectMode == 1)
			{
				if (rtsUnitController.isUnitSelect == 0)
				{
					if (rtsUnitController.isBuildingSelect == 1)
					{
						if (rtsUnitController.WhatTier == 1)
						{
							rtsUnitController.RTSWhatUnit = 2;
							rtsUnitController.SpawnUnit(rtsUnitController.Spawnpos, rtsUnitController.Spawnrally);
						}
						if (rtsUnitController.WhatTier == 3)
						{
							rtsUnitController.RTSWhatUnit = 6;
							rtsUnitController.SpawnUnit(rtsUnitController.Spawnpos, rtsUnitController.Spawnrally);
						}
					}

				}
				if (rtsUnitController.isUnitSelect == 1)
				{
					rtsUnitController.StopSelectedUnits();
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.D))
		{
			if (rtsUnitController.SelectMode == 1)
			{
				if (rtsUnitController.isBuildingSelect == 1)
				{
					if (rtsUnitController.WhatTier == 4)
					{
						rtsUnitController.RTSWhatUnit = 0;
						rtsUnitController.SpawnUnit(rtsUnitController.Spawnpos, rtsUnitController.Spawnrally);
					}
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			if (rtsUnitController.SelectMode == 1)
			{
				if (rtsUnitController.isBuildingSelect == 1)
				{
					if (rtsUnitController.WhatTier == 3)
					{
						rtsUnitController.RTSWhatUnit = 5;
						rtsUnitController.SpawnUnit(rtsUnitController.Spawnpos, rtsUnitController.Spawnrally);
					}
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.F))
		{
			if (rtsUnitController.SelectMode == 1)
			{
				if (rtsUnitController.isBuildingSelect == 1)
				{
					if (rtsUnitController.WhatTier == 3)
					{
						rtsUnitController.RTSWhatUnit = 5;
						rtsUnitController.SpawnUnit(rtsUnitController.Spawnpos, rtsUnitController.Spawnrally);
					}
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.G))
		{
			if (rtsUnitController.SelectMode == 1)
			{
				if (rtsUnitController.isBuildingSelect == 1)
				{
					if (rtsUnitController.WhatTier == 3)
					{
						rtsUnitController.RTSWhatUnit = 6;
						rtsUnitController.SpawnUnit(rtsUnitController.Spawnpos, rtsUnitController.Spawnrally);
					}
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.M))
		{
			if (rtsUnitController.isUnitSelect == 1)
			{
				attackMode = false;
				patrolMode = false;
				moveMode = true;
				rallyMode = false;
				modeOn = true;
			}
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			if (rtsUnitController.SelectMode == 1)
			{
				if (rtsUnitController.isUnitSelect == 0)
				{
					if (rtsUnitController.isBuildingSelect == 1)
					{
						if (rtsUnitController.WhatTier == 2)
						{
							rtsUnitController.RTSWhatUnit = 4;
							rtsUnitController.SpawnUnit(rtsUnitController.Spawnpos, rtsUnitController.Spawnrally);
						}
					}
				}
				if (rtsUnitController.isUnitSelect == 1)
				{
					patrolMode = true;
					attackMode = false;
					moveMode = false;
					rallyMode = false;
					modeOn = true;
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.H))
		{
			if (rtsUnitController.SelectMode == 1)
			{
				if (rtsUnitController.isUnitSelect == 0)
				{
					if (rtsUnitController.isBuildingSelect == 1)
					{
						if (rtsUnitController.WhatTier == 2)
						{
							rtsUnitController.RTSWhatUnit = 4;
							rtsUnitController.SpawnUnit(rtsUnitController.Spawnpos, rtsUnitController.Spawnrally);
						}
					}
				}
				if (rtsUnitController.isUnitSelect == 1)
				{
					rtsUnitController.HoldSelectedUnits();
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.Y))
		{
			if (rtsUnitController.isBuildingSelect == 1)
			{
				attackMode = false;
				patrolMode = false;
				moveMode = false;
				rallyMode = true;
			}
		}

		if (Input.GetKeyDown(KeyCode.I))
		{
			if (rtsUnitController.SelectMode == 1)
			{
				if (rtsUnitController.isBuildingSelect == 1)
				{
					if (rtsUnitController.WhatTier == 2)
					{
						rtsUnitController.RTSWhatUnit = 3;
						rtsUnitController.SpawnUnit(rtsUnitController.Spawnpos, rtsUnitController.Spawnrally);
					}
				}
			}
		}

		if (attackMode == true)
        {
			if (transform.GetComponent<RTSUnitController>().SelectMode == 1)
			{
				mousepos = Input.mousePosition;

				RaycastHit hit;
				Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

				if (basicpointer == null)
				{
					if (Physics.Raycast(ray, out hit))
					{
						basicpointer = Instantiate(AttackPointPrefab, hit.point, Quaternion.identity);
					}
				}
				else
				{
					// 포인터가 있는 경우, 삭제
					Destroy(basicpointer);
					basicpointer = null;
				}

				if (basicpointer != null)
				{
					if (Physics.Raycast(ray, out hit))
					{
						basicpointer.transform.position = hit.point;
					}
				}
			}
		}
		if (patrolMode == true)
		{
			if (transform.GetComponent<RTSUnitController>().SelectMode == 1)
			{
				mousepos = Input.mousePosition;

				RaycastHit hit;
				Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

				if (basicpointer == null)
				{
					if (Physics.Raycast(ray, out hit))
					{
						basicpointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);
					}
				}
				else
				{
					// 포인터가 있는 경우, 삭제
					Destroy(basicpointer);
					basicpointer = null;
				}

				if (basicpointer != null)
				{
					if (Physics.Raycast(ray, out hit))
					{
						basicpointer.transform.position = hit.point;
					}
				}
			}
		}

		if (moveMode == true)
		{
			if (transform.GetComponent<RTSUnitController>().SelectMode == 1)
			{
				mousepos = Input.mousePosition;

				RaycastHit hit;
				Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

				if (basicpointer == null)
				{
					if (Physics.Raycast(ray, out hit))
					{
						basicpointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);
					}
				}
				else
				{
					// 포인터가 있는 경우, 삭제
					Destroy(basicpointer);
					basicpointer = null;
				}

				if (basicpointer != null)
				{
					if (Physics.Raycast(ray, out hit))
					{
						basicpointer.transform.position = hit.point;
					}
				}
			}
		}

		if (rallyMode == true)
		{
			if (rtsUnitController.SelectMode == 1)
			{
				if(rtsUnitController.isBuildingSelect == 1)
                {
					mousepos = Input.mousePosition;

					RaycastHit hit;
					Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

					if (basicpointer == null)
					{
						if (Physics.Raycast(ray, out hit))
						{
							basicpointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);
						}
					}
					else
					{
						// 포인터가 있는 경우, 삭제
						Destroy(basicpointer);
						basicpointer = null;
					}

					if (basicpointer != null)
					{
						if (Physics.Raycast(ray, out hit))
						{
							basicpointer.transform.position = hit.point;
						}
					}
				}
			}
		}
	}

	private void DrawDragRectangle()
	{
		// 드래그 범위를 나타내는 Image UI의 위치
		dragRectangle.position = (start + end) * 0.5f;
		// 드래그 범위를 나타내는 Image UI의 크기
		dragRectangle.sizeDelta = new Vector2(Mathf.Abs(start.x - end.x), Mathf.Abs(start.y - end.y));
	}

	private void CalculateDragRect()
	{
		if (Input.mousePosition.x < start.x)
		{
			dragRect.xMin = Input.mousePosition.x;
			dragRect.xMax = start.x;
		}
		else
		{
			dragRect.xMin = start.x;
			dragRect.xMax = Input.mousePosition.x;
		}

		if (Input.mousePosition.y < start.y)
		{
			dragRect.yMin = Input.mousePosition.y;
			dragRect.yMax = start.y;
		}
		else
		{
			dragRect.yMin = start.y;
			dragRect.yMax = Input.mousePosition.y;
		}
	}
	public void Abuttons()
	{
		moveMode = false;
		attackMode = true;
		patrolMode = false;
		rallyMode = false;
		modeOn = true;
	}

	public void Mbuttons()
	{
		moveMode = true;
		attackMode = false;
		patrolMode = false;
		rallyMode = false;
		modeOn = true;
	}

	public void Pbuttons()
	{
		moveMode = false;
		attackMode = false;
		patrolMode = true;
		rallyMode = false;
		modeOn = true;
	}

	public void Ybuttons()
	{
		attackMode = false;
		patrolMode = false;
		moveMode = false;
		rallyMode = true;
		modeOn = true;
	}

	public void Vbuttons()
	{
		attackMode = false;
		patrolMode = false;
		moveMode = false;
		rallyMode = false;
		modeOn = false;
		rtsUnitController.BuildMode = 1;
	}

	private void SelectUnits()
	{
		// 모든 유닛을 검사
		foreach (UnitController unit in rtsUnitController.UnitList)
		{
			// 유닛의 월드 좌표를 화면 좌표로 변환해 드래그 범위 내에 있는지 검사
			if (dragRect.Contains(mainCamera.WorldToScreenPoint(unit.transform.position)))
			{
				rtsUnitController.DragSelectUnit(unit);
			}
		}

		int i = Random.Range(3, 7);
		rtsUnitController.SelectedUnitPlayVoice(i);
	}
}

