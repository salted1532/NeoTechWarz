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
	private RectTransform dragRectangle;            // ���콺�� �巡���� ������ ����ȭ�ϴ� Image UI�� RectTransform

	private Rect dragRect;              // ���콺�� �巡�� �� ���� (xMin~xMax, yMin~yMax)
	private Vector2 start = Vector2.zero;   // �巡�� ���� ��ġ
	private Vector2 end = Vector2.zero;     // �巡�� ���� ��ġ

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

		// start, end�� (0, 0)�� ���·� �̹����� ũ�⸦ (0, 0)���� ������ ȭ�鿡 ������ �ʵ��� ��
		DrawDragRectangle();
	}

	private void Update()
	{
		// ���콺 ���� Ŭ������ ���� ���� or ���� / ���콺 �巡�� �̹��� �������� ���
		if (Input.GetMouseButtonDown(0))
		{
			//���콺 �巡�� �������� ����
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

				// ������ �ε����� ������Ʈ�� ���� �� (=������ Ŭ������ ��)
				if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerUnit))
				{
					// �浹�� ������Ʈ�� ���� ó��
					if (hit.transform.GetComponent<UnitController>() == null) return;

					if (moveMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							Debug.Log("MŰ �̵�");
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
							Debug.Log("AŰ ������");
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
							Debug.Log("PŰ ����");
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
								Debug.Log("YŰ ��������");
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
							Debug.Log("Ŭ�� ����");
							rtsUnitController.ClickSelectUnit(hit.transform.GetComponent<UnitController>());
						}
					}

				}
				// ������ �ε����� ������Ʈ�� ���� �� (=������ Ŭ������ ��)
				else if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerBuilding))
				{
					// �浹�� ������Ʈ�� ���� ó��
					if (hit.transform.GetComponent<BuildingController>() == null) return;

					if (attackMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							Debug.Log("�ٴڰ���");
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
							Debug.Log("PŰ ����");
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
							Debug.Log("MŰ �̵�");
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
								Debug.Log("YŰ ��������");
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
				// ������ �ε����� ������Ʈ�� ���� ��
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
				//������ �ε����� ������Ʈ�� ���� �� (=�������� Ŭ������ ��)
				if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerEnemy))
				{
					if (attackMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							Debug.Log("AŰ ������");
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
							Debug.Log("PŰ ����");
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
							Debug.Log("MŰ �̵�");
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
								Debug.Log("YŰ ��������");
								rallyMode = false;
								modeOn = false;
								rtsUnitController.SetRallyPoint(hit.point);
								GameObject Pointer = Instantiate(pointerPrefab, hit.point, Quaternion.identity);
							}

						}
					}
				}
				//������ �ε����� ������Ʈ�� ���� �� (=�ٴ��� Ŭ������ ��)
				if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerGround))
				{
					if (attackMode == true)
					{
						if (rtsUnitController.SelectMode == 1)
						{
							Debug.Log("�ٴڰ���");
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
							Debug.Log("PŰ ����");
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
							Debug.Log("MŰ �̵�");
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
								Debug.Log("YŰ ��������");
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

			// ���콺�� Ŭ���� ���·� �巡�� �ϴ� ���� �巡�� ������ �̹����� ǥ��
			DrawDragRectangle();
		}

		if (Input.GetMouseButtonUp(0))
		{
			// ���콺 Ŭ���� ������ �� �巡�� ���� ���� �ִ� ���� ����
			CalculateDragRect();
			SelectUnits();

			// ���콺 Ŭ���� ������ �� �巡�� ������ ������ �ʵ���
			// start, end ��ġ�� (0, 0)���� �����ϰ� �巡�� ������ �׸���
			start = end = Vector2.zero;
			DrawDragRectangle();
		}

		// ���콺 ������ Ŭ������ ���� �̵�
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
					// ���� ������Ʈ(layerUnit)�� Ŭ������ ��
					if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerEnemy))
					{
						Debug.Log("�׳� ������");
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
		//�Ǽ����
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
					// �����Ͱ� �ִ� ���, ����
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
					// �����Ͱ� �ִ� ���, ����
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
					// �����Ͱ� �ִ� ���, ����
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
						// �����Ͱ� �ִ� ���, ����
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
		// �巡�� ������ ��Ÿ���� Image UI�� ��ġ
		dragRectangle.position = (start + end) * 0.5f;
		// �巡�� ������ ��Ÿ���� Image UI�� ũ��
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
		// ��� ������ �˻�
		foreach (UnitController unit in rtsUnitController.UnitList)
		{
			// ������ ���� ��ǥ�� ȭ�� ��ǥ�� ��ȯ�� �巡�� ���� ���� �ִ��� �˻�
			if (dragRect.Contains(mainCamera.WorldToScreenPoint(unit.transform.position)))
			{
				rtsUnitController.DragSelectUnit(unit);
			}
		}

		int i = Random.Range(3, 7);
		rtsUnitController.SelectedUnitPlayVoice(i);
	}
}

