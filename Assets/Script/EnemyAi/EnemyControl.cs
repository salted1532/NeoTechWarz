using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyControl : MonoBehaviour
{
    private EnemyAI enemyai;

    public bool alreadyAttacked = false;
    public float timeBetweenAttacks;

    private NavMeshAgent navMeshAgent;

    public bool isAttacker;
    public bool isBuilding;

    [SerializeField]
    private GameObject ShootingEffect;

    [SerializeField]
    private AudioSource audiosource;
    [SerializeField]
    private AudioClip[] clip;
    [SerializeField]
    private int WhatClip = 0;

    public int WhatEnemy;
    public bool isLeftEnemy;
    public bool isRightEnemy;
    public bool isFrontEnemy;
    public bool isAttackEnemy;

    private Vector3 mypos;

    private int whatisdefence;

    public bool isrespawn = false;

    // NavMeshAgent 초기화는 Awake에서 수행
    private void Awake()
    {
        if (isBuilding == false)
        {
            // NavMeshAgent 초기화
            navMeshAgent = GetComponent<NavMeshAgent>();
            if (navMeshAgent == null)
            {
                Debug.LogError("NavMeshAgent가 할당되지 않았습니다. NavMeshAgent를 추가하세요.");
                return;
            }
        }

    }

    void Start()
    {
        // EnemyAI 컴포넌트 가져오기
        enemyai = GameObject.Find("EnemyAi").GetComponent<EnemyAI>();
        if (enemyai == null)
        {
            Debug.LogError("EnemyAI가 할당되지 않았습니다. Inspector에서 설정하세요.");
            return;
        }

        if (isBuilding == false)
        {
            // AudioSource 초기화 확인
            if (audiosource == null || clip.Length == 0)
            {
                Debug.LogError("AudioSource나 AudioClip이 올바르게 설정되지 않았습니다.");
            }
        }

        mypos = transform.position;
        Debug.Log("방어선 설정");
        // 방어선 설정
        if (isLeftEnemy)
        {
            enemyai.Leftdefence.Add(this);
            whatisdefence = 1;
        }
        if (isRightEnemy)
        {
            enemyai.Rightdefence.Add(this);
            whatisdefence = 2;
        }
        if (isFrontEnemy)
        {
            enemyai.Frontdefence.Add(this);
            whatisdefence = 3;
        }
        if (isAttackEnemy)
        {
            enemyai.AttackPlayer.Add(this);
            whatisdefence = 4;
        }
        if (isBuilding)
        {
            Debug.Log("나는건물");
            enemyai.Building.Add(this);
            Debug.Log("Building List Count: " + enemyai.Building.Count);
        }
    }

    void Update()
    {
        // Update 로직 추가 가능
    }

    public void EnemyGoRally(Vector3 end)
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(end);
            mypos = end;
        }
        else
        {
            Debug.LogError("NavMeshAgent가 초기화되지 않았습니다.");
        }
    }

    public void EnemyMove(Vector3 end)
    {
        if (isAttacker)
        {
            if (navMeshAgent != null)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(end);
            }
            else
            {
                Debug.LogError("NavMeshAgent가 초기화되지 않았습니다.");
            }
        }
    }

    public void EnemyAttack(Vector3 end, int damage)
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
        }

        if (!alreadyAttacked)
        {
            Vector3 direction = end - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1000f);

            if (audiosource != null && clip.Length > 0)
            {
                WhatClip = 0;
                audiosource.clip = clip[WhatClip];
                audiosource.Play();
            }

            if (ShootingEffect != null)
            {
                ShootingEffect.SetActive(true);
                Invoke(nameof(ResetEffect), 0.4f);
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            float distance = Vector3.Distance(transform.position, end);
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 1f);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Player") ||
                    hit.collider.CompareTag("MainBase") ||
                    hit.collider.CompareTag("Tier1") ||
                    hit.collider.CompareTag("Tier2") ||
                    hit.collider.CompareTag("Tier3") ||
                    hit.collider.CompareTag("Lap") ||
                    hit.collider.CompareTag("Supply Depot"))
                {
                    Debug.Log("공격당함");
                    UnitHealth playerHealth = hit.collider.GetComponent<UnitHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(damage);
                    }
                    else
                    {
                        Debug.LogError("Player 오브젝트에 UnitHealth 컴포넌트가 없습니다.");
                    }
                }
            }
        }
    }

    private void ResetEffect()
    {
        if (ShootingEffect != null)
        {
            ShootingEffect.SetActive(false);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void RemoveEnemy()
    {
        if (isLeftEnemy)
        {
            enemyai.Leftdefence.Remove(this);
        }
        if (isRightEnemy)
        {
            enemyai.Rightdefence.Remove(this);
        }
        if (isFrontEnemy)
        {
            enemyai.Frontdefence.Remove(this);
        }
        if (isAttackEnemy)
        {
            enemyai.AttackPlayer.Remove(this);
        }
        if (isBuilding)
        {
            enemyai.Building.Remove(this);
        }

        if(isrespawn == false)
        {
            enemyai.SpawnEnemy(WhatEnemy, mypos, whatisdefence, 25f, true);
        }

        Destroy(gameObject);
    }

}
