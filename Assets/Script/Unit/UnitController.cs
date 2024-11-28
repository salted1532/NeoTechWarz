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
						//Debug.Log("����");
						PatrolUnit(endPoint);
					}
				}
				else if (patrolling == true)
				{
					//Debug.Log("����1");
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
			//Debug.Log("����");
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
				//Debug.Log("�ڿ�ä�� ��� ����");
				yield return new WaitForSeconds(1.5f);
				if (isGas == true)
				{
					if(ishaveOre == false) 
					{
						//Debug.Log("����ä��");
						SmallGas.SetActive(true);
						ishaveGas = true;
					}
				}
				else
				{
					if(ishaveGas == false)
                    {
						//Debug.Log("�̳׶�ä��");
						SmallOre.SetActive(true);
						ishaveOre = true;
					}
				}
			}
		}
		//Debug.Log("�ڿ�ä��");
		GameObject nearestBase = FindNearestObject();
		if (nearestBase != null)
		{
			//Debug.Log("���� ����� ������Ʈ��: " + nearestBase.name);
		}
		else
		{
			//Debug.LogError("����� ������Ʈ�� ã�� �� �����ϴ�!");
		}
		startPoint = nearestBase.transform.position;
		if (navMeshAgent != null)
		{
			// ������ ����
			navMeshAgent.SetDestination(startPoint);
		}
		collectioning = true;
		goingToEnd = false;
	}

	public void SelectUnit()
	{
		//Debug.Log("���õ�");
		unitMarker.SetActive(true);
	}

	public void DeselectUnit()
	{
		//Debug.Log("���� ��");
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
		Debug.Log("�ٴڰ���!");
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
		Debug.Log("����!");

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
			//���� ���
			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;

			float distance = Vector3.Distance(transform.position, end);

			Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 1f);

			alreadyAttacked = true;
			Invoke(nameof(ResetAttack), timeBetweenAttacks);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity))
			{
				// �浹�� ������Ʈ�� ���� ó��
				Debug.Log("Hit " + hit.collider.name);

				// �浹�� ������Ʈ�� ���� ��� (���� �ĺ��ϴ� ����� ���� ����)
				// ���� ���, ���� 'Enemy' �±׸� ������ �ִٰ� ����
				if (hit.collider.CompareTag("Enemy"))
				{
					Debug.Log("���ݴ���");
					// ������ �������� ������ ��� (���� ��ũ��Ʈ�� ���� ó�� ������ �־�� ��)
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

		// RTSUnitController�� MainBase ����Ʈ�� ��ȸ
		foreach (GameObject baseObject in rtsunitcontroller.MainBase)
		{
			float distance = Vector3.Distance(transform.position, baseObject.transform.position);

			if (distance < nearestDistance)
			{
				nearestDistance = distance;
				nearestBase = baseObject; // ���� ����� ���̽� ����
			}
		}

		return nearestBase; // ���� ����� ���̽� ��ȯ
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
		// �ڽ��� �����Ǳ� ���� ������ ����
		if (!isAudioPlaying && audiosource != null)
		{
			isAudioPlaying = true; // ����� ��� ������ ǥ��
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

		// ����� ����� �Ϸ�� �� ������Ʈ�� �����ϴ� �ڷ�ƾ ����
		StartCoroutine(DestroyAfterAudio());
	}

	private IEnumerator DestroyAfterAudio()
	{
		// ����� �ҽ��� ��� ���� ������ ���
		if (audiosource != null)
		{
			yield return new WaitWhile(() => audiosource.isPlaying);
		}

		isAudioPlaying = false; // ����� ����� ���� �� ��� �� ���� ����

		// ���� ������Ʈ �ı�
		Destroy(gameObject);
	}
}