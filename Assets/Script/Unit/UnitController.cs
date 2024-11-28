using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using FischlWorks_FogWar;

public class UnitController : MonoBehaviour
{
	[SerializeField]
	private GameObject unitMarker;
	[SerializeField]
	private GameObject ShootingEffect;
	[SerializeField]
	private GameObject SmallOre;
	[SerializeField]
	private GameObject SmallGas;

	private NavMeshAgent navMeshAgent;

	public bool alreadyAttacked = false;
	public float timeBetweenAttacks;

	public int idleMode = 1;
	public int attackMode = 0;
	public int MoveMode = 0;

	private Vector3 startPoint;
	private Vector3 endPoint;

	public bool isWorker;

	public bool isFlyUnit;

	private bool patrolling = false;
	public bool collectioning = false;
	private bool goingToEnd = true;
	private bool holding = false;
	public bool ishaveOre = false;
	public bool ishaveGas = false;
	public bool isGas = false;

	[SerializeField]
	private AudioSource audiosource;
	[SerializeField]
	private AudioClip[] clip;
	[SerializeField]
	private int WhatClip = 0;

	private UnitSpawner unitspawner;
	private RTSUnitController rtsunitcontroller;
	private UiButtonMapiing uibuttonmaping;

	[SerializeField]
	private csFogWar fogWar = null;

	private GameObject currentTarget;

	private bool isAudioPlaying = false;

	public int myUnitType;


	private bool isdead = false;
	private void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();

		audiosource = GetComponent<AudioSource>();
	}

    private void Start()
    {
		// This part is meant to be modified following the project's scene structure later...
		try
		{
			fogWar = GameObject.Find("FogWar").GetComponent<csFogWar>();
		}
		catch
		{
			Debug.LogErrorFormat("Failed to fetch csFogWar component. " +
				"Please rename the gameobject that the module is attached to as \"FogWar\", " +
				"or change the implementation located in the csFogVisibilityAgent.cs script.");
			return; // Early exit if fogWar is not found
		}
		try
		{
			uibuttonmaping = GameObject.Find("RTSUnitControlSystem").GetComponent<UiButtonMapiing>();
		}
		catch
		{
			Debug.LogErrorFormat("Failed to fetch csFogWar component. " +
				"Please rename the gameobject that the module is attached to as \"FogWar\", " +
				"or change the implementation located in the csFogVisibilityAgent.cs script.");
			return; // Early exit if fogWar is not found
		}


		// Add the fog revealer and get its index
		fogWar.AddFogRevealer(new csFogWar.FogRevealer(transform, 30, true));

		unitspawner = GameObject.Find("RTSUnitControlSystem").GetComponent<UnitSpawner>();
		rtsunitcontroller = GameObject.Find("RTSUnitControlSystem").GetComponent<RTSUnitController>();
	}

    protected void Update()
	{

		if (collectioning == true)
		{
			if (isWorker == true)
			{
				navMeshAgent.radius = 0.3f;
			}
		}
		else
		{
			if (isWorker == true)
			{
				navMeshAgent.radius = 1.2f;
			}

		}

		if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
		{
			if(holding == false)
            {
				if (goingToEnd == false)
				{
					if(patrolling == true)
                    {
						//Debug.Log("복귀");
						PatrolUnit(endPoint);
					}
				}
				else if (patrolling == true)
				{
					//Debug.Log("순찰1");
					navMeshAgent.SetDestination(startPoint);
					goingToEnd = false;
				}
				else if(collectioning == true)
                {
					StartCoroutine(WaitAndExecuteCollection());
				}
				else
				{
					navMeshAgent.isStopped = true;
					idleMode = 1;
					MoveMode = 0;
					attackMode = 0;
					navMeshAgent.isStopped = false;
				}
			}
		}
	}
	public void endCollect()
    {
		if (collectioning == true)
		{
			//Debug.Log("복귀");
			if (ishaveGas == true)
			{
				GameObject.Find("RTSUnitControlSystem").GetComponent<UiButtonMapiing>().Gas += 5;
				ishaveGas = false;
				SmallGas.SetActive(false);
			}
			if (ishaveOre == true)
			{
				GameObject.Find("RTSUnitControlSystem").GetComponent<UiButtonMapiing>().Resource += 5;
				ishaveOre = false;
				SmallOre.SetActive(false);
			}
			CollectOre(endPoint, isGas);
		}
	}

	public IEnumerator WaitAndExecuteCollection()
	{
		if (ishaveOre == false)
		{
			if (ishaveGas == false)
            {
				//Debug.Log("자원채취 대기 시작");
				yield return new WaitForSeconds(1.5f);
				if (isGas == true)
				{
					if(ishaveOre == false) 
					{
						//Debug.Log("가스채취");
						SmallGas.SetActive(true);
						ishaveGas = true;
					}
				}
				else
				{
					if(ishaveGas == false)
                    {
						//Debug.Log("미네랄채취");
						SmallOre.SetActive(true);
						ishaveOre = true;
					}
				}
			}
		}
		//Debug.Log("자원채취");
		GameObject nearestBase = FindNearestObject();
		if (nearestBase != null)
		{
			//Debug.Log("가장 가까운 오브젝트는: " + nearestBase.name);
		}
		else
		{
			//Debug.LogError("가까운 오브젝트를 찾을 수 없습니다!");
		}
		startPoint = nearestBase.transform.position;
		if (navMeshAgent != null)
		{
			// 목적지 설정
			navMeshAgent.SetDestination(startPoint);
		}
		collectioning = true;
		goingToEnd = false;
	}

	public void SelectUnit()
	{
		//Debug.Log("선택됨");
		unitMarker.SetActive(true);
	}

	public void DeselectUnit()
	{
		//Debug.Log("디선택 됨");
		unitMarker.SetActive(false);
	}

	public void MoveTo(Vector3 end)
	{
		idleMode = 0;
		MoveMode = 1;
		attackMode = 0;
		navMeshAgent.isStopped = false;
		patrolling = false;
		collectioning = false;
		goingToEnd = true;
		holding = false;
		if (navMeshAgent != null)
        {
			navMeshAgent.SetDestination(end);
		}
			
	}

	public void AttackToGround(Vector3 end)
	{
		idleMode = 1;
		MoveMode = 0;
		attackMode = 0;
		navMeshAgent.isStopped = false;
		patrolling = false;
		collectioning = false;
		goingToEnd = true;
		holding = false;
		if (navMeshAgent != null)
		{
			navMeshAgent.SetDestination(end);
		}
		Debug.Log("바닥공격!");
	}

	public void AttackToUnit(Vector3 end)
	{
		attackMode = 1;
		idleMode = 0;
		MoveMode = 0;
		navMeshAgent.isStopped = false;
		patrolling = false;
		collectioning = false;
		goingToEnd = true;
		holding = false;
		if (navMeshAgent != null)
		{
			navMeshAgent.SetDestination(end);
		}
		Debug.Log("공격!");

	}

	public void Attack(Vector3 end, int damage)
	{
		navMeshAgent.isStopped = true;

		if (!alreadyAttacked)
		{
			Vector3 direction = end - transform.position;
			Quaternion targetRotation = Quaternion.LookRotation(direction);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1000f);

			if (isAudioPlaying == false)
            {
				WhatClip = 14;
				audiosource.clip = clip[WhatClip];
				audiosource.Play();
			}

			ShootingEffect.SetActive(true);

			Invoke(nameof(ResetEffect), 0.4f);
			//공격 모션
			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;

			float distance = Vector3.Distance(transform.position, end);

			Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 1f);

			alreadyAttacked = true;
			Invoke(nameof(ResetAttack), timeBetweenAttacks);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity))
			{
				// 충돌한 오브젝트에 대해 처리
				Debug.Log("Hit " + hit.collider.name);

				// 충돌한 오브젝트가 적일 경우 (적을 식별하는 방법에 따라 조정)
				// 예를 들어, 적이 'Enemy' 태그를 가지고 있다고 가정
				if (hit.collider.CompareTag("Enemy"))
				{
					Debug.Log("공격당함");
					// 적에게 데미지를 입히는 방법 (적의 스크립트에 공격 처리 로직을 넣어야 함)
					hit.collider.GetComponent<UnitHealth>().TakeDamage(damage);
				}
			}
		}
	}
	private void ResetEffect()
	{
		ShootingEffect.SetActive(false);
	}
	private void ResetAttack()
	{
		alreadyAttacked = false;
	}

	public void StopUnit()
    {
		idleMode = 1;
		MoveMode = 0;
		attackMode = 0;
		holding = false;
		navMeshAgent.isStopped = true;

	}

	public void PatrolUnit(Vector3 end)
	{
		navMeshAgent.isStopped = false;
		idleMode = 1;
		MoveMode = 1;
		attackMode = 0;
		endPoint = end;
		startPoint = transform.position;
		patrolling = true;
		collectioning = false;
		goingToEnd = true;
		holding = false;
		if (navMeshAgent != null)
		{
			navMeshAgent.SetDestination(end);
		}

	}

	public void HoldUnit()
	{
		idleMode = 1;
		MoveMode = 1;
		attackMode = 0;
		holding = true;
		navMeshAgent.isStopped = true;
	}

	public void CollectOre(Vector3 end, bool gas)
	{
		if (isWorker)
		{
			Debug.Log(navMeshAgent.remainingDistance);

			idleMode = 0;
			MoveMode = 1;
			attackMode = 0;
			holding = false;
			patrolling = false;
			collectioning = true;
			goingToEnd = true;
			navMeshAgent.isStopped = false;
			isGas = gas;
			endPoint = end;

			GameObject nearestBase = FindNearestObject();

			if (nearestBase != null)
			{
				startPoint = nearestBase.transform.position;

				if (navMeshAgent != null)
				{
					navMeshAgent.SetDestination(end);
				}
			}
			else
			{
				Debug.LogWarning("No bases found!");
			}
		}
	}

	GameObject FindNearestObject()
	{
		GameObject nearestBase = null;
		float nearestDistance = Mathf.Infinity; // Start with a large distance

		// RTSUnitController의 MainBase 리스트를 순회
		foreach (GameObject baseObject in rtsunitcontroller.MainBase)
		{
			float distance = Vector3.Distance(transform.position, baseObject.transform.position);

			if (distance < nearestDistance)
			{
				nearestDistance = distance;
				nearestBase = baseObject; // 가장 가까운 베이스 저장
			}
		}

		return nearestBase; // 가장 가까운 베이스 반환
	}

public void PlayVoice(int i)
    {
		if(isAudioPlaying == false)
        {
			if (audiosource.isPlaying)
			{
				if (WhatClip < 4)
				{
					audiosource.clip = clip[i];
					audiosource.Play();
					WhatClip = i;
				}
			}
			else
			{
				WhatClip = i;
				audiosource.clip = clip[WhatClip];
				audiosource.Play();
			}
		}
	}


	public void GoToRallyPoint(Vector3 end)
    {
		WhatClip = Random.Range(0, 3);
		audiosource.clip = clip[WhatClip];
		audiosource.Play();
		if (navMeshAgent != null)
		{
			navMeshAgent.SetDestination(end);
		}
	}

	public void RemoveUnit()
	{
		// 자신이 삭제되기 전에 참조를 해제
		if (!isAudioPlaying && audiosource != null)
		{
			isAudioPlaying = true; // 오디오 재생 중임을 표시
			audiosource.Stop();
			WhatClip = 15;
			audiosource.clip = clip[WhatClip];
			audiosource.Play();
		}

		if (unitspawner != null)
		{
			unitspawner.unitList.Remove(this);
		}

		if (rtsunitcontroller != null)
		{
			rtsunitcontroller.selectedUnitList.Remove(this);
		}

		if(isdead == false)
        {
			if (myUnitType == 0)
			{
				uibuttonmaping.Population -= 1;

			}
			if (myUnitType == 1)
			{
				uibuttonmaping.Population -= 1;

			}
			if (myUnitType == 2)
			{
				uibuttonmaping.Population -= 2;
			}
			if (myUnitType == 3)
			{
				uibuttonmaping.Population -= 2;
			}
			if (myUnitType == 4)
			{
				uibuttonmaping.Population -= 2;
			}
			if (myUnitType == 5)
			{
				uibuttonmaping.Population -= 2;
			}
			if (myUnitType == 6)
			{
				uibuttonmaping.Population -= 6;
			}
			isdead = true;
		}

		

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

		// 오디오 재생이 완료된 후 오브젝트를 삭제하는 코루틴 시작
		StartCoroutine(DestroyAfterAudio());
	}

	private IEnumerator DestroyAfterAudio()
	{
		// 오디오 소스가 재생 중일 때까지 대기
		if (audiosource != null)
		{
			yield return new WaitWhile(() => audiosource.isPlaying);
		}

		isAudioPlaying = false; // 오디오 재생이 끝난 후 재생 중 상태 해제

		// 게임 오브젝트 파괴
		Destroy(gameObject);
	}
}