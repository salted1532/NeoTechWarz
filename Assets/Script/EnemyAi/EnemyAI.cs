using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public List<EnemyControl> Leftdefence = new List<EnemyControl>();
    public List<EnemyControl> Rightdefence = new List<EnemyControl>();
    public List<EnemyControl> Frontdefence = new List<EnemyControl>();
    public List<EnemyControl> AttackPlayer = new List<EnemyControl>();
    public List<EnemyControl> Building = new List<EnemyControl>();

    public GameObject MainObject;

    [SerializeField]
    private GameObject AttackPoint;
    [SerializeField]
    private GameObject SpawnPoint;
    [SerializeField]
    private GameObject RallyPoint;

    public float totalTime;

    [SerializeField]
    private float elapsedTime = 0f;
    [SerializeField]
    private float ReAttackTimer = 0;

    private bool isTimerRunning = false;

    public List<GameObject> EnemyPrefabs;

    [SerializeField]
    private int AttackerCount = 10;
    [SerializeField]
    private float Enemyspawntimer = 0;
    [SerializeField]
    private int WaveCount = 1;

    private bool Attacking = false;

    private void Awake()
    {
        Leftdefence = new List<EnemyControl>();
        Rightdefence = new List<EnemyControl>();
        Frontdefence = new List<EnemyControl>();
    }

    void Start()
    {
        Debug.Log(AttackPoint.transform.position);
        // AttackPoint ����
        if (AttackPoint.transform.position == Vector3.zero)
        {
            Debug.LogWarning("AttackPoint�� �������� �ʾҽ��ϴ�");
        }
        StartTimer();
    }


    public void LeftDefenceEnemy(Vector3 end)
    {
        if (Leftdefence.Count == 0)
        {
            Debug.LogWarning("EnemyList�� ����ֽ��ϴ�. ���� �߰��ϼ���.");
            return;
        }

        for (int i = 0; i < Leftdefence.Count / 3; ++i)
        {
            if (Leftdefence[i] != null)
            {
                try
                {
                    Leftdefence[i].EnemyMove(end);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"EnemyList[{i}]���� ���� �߻�: {e.Message}");
                }
            }
            else
            {
                Debug.LogWarning($"EnemyList[{i}]�� null�Դϴ�.");
            }
        }
    }

    public void RightDefenceEnemy(Vector3 end)
    {
        if (Rightdefence.Count == 0)
        {
            Debug.LogWarning("EnemyList�� ����ֽ��ϴ�. ���� �߰��ϼ���.");
            return;
        }

        for (int i = 0; i < Rightdefence.Count / 3; ++i)
        {
            if (Rightdefence[i] != null)
            {
                try
                {
                    Rightdefence[i].EnemyMove(end);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"EnemyList[{i}]���� ���� �߻�: {e.Message}");
                }
            }
            else
            {
                Debug.LogWarning($"EnemyList[{i}]�� null�Դϴ�.");
            }
        }
    }

    public void FrontDefenceEnemy(Vector3 end)
    {
        if (Frontdefence.Count == 0)
        {
            Debug.LogWarning("EnemyList�� ����ֽ��ϴ�. ���� �߰��ϼ���.");
            return;
        }

        for (int i = 0; i < Frontdefence.Count / 3; ++i)
        {
            if (Frontdefence[i] != null)
            {
                try
                {
                    Frontdefence[i].EnemyMove(end);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"EnemyList[{i}]���� ���� �߻�: {e.Message}");
                }
            }
            else
            {
                Debug.LogWarning($"EnemyList[{i}]�� null�Դϴ�.");
            }
        }
    }

    public void AttackPlayerEnemy(Vector3 end)
    {
        Debug.Log("������ ����");
        if (AttackPlayer.Count == 0)
        {
            Debug.LogWarning("EnemyList�� ����ֽ��ϴ�. ���� �߰��ϼ���.");
            return;
        }

        for (int i = 0; i < AttackPlayer.Count; ++i)
        {
            if (AttackPlayer[i] != null)
            {
                try
                {
                    AttackPlayer[i].EnemyMove(end);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"EnemyList[{i}]���� ���� �߻�: {e.Message}");
                }
            }
            else
            {
                Debug.LogWarning($"EnemyList[{i}]�� null�Դϴ�.");
            }
        }
    }

    public void SpawnEnemy(int end, Vector3 pos, int whatdefence, float spawntime, bool isrespawn)
    {
        StartCoroutine(SpawnEnemyCoroutine(end, pos, whatdefence, spawntime, isrespawn));
    }

    private IEnumerator SpawnEnemyCoroutine(int end, Vector3 pos, int whatdefence, float spawntime, bool isrespawn)
    {
        // 5�� ���
        yield return new WaitForSeconds(spawntime);

        // ���� SpawnEnemy �޼����� ����
        // �� �������� null���� Ȯ��
        if (EnemyPrefabs[end] == null)
        {
            Debug.LogError($"Enemy prefab at index {end} is null. Please check the array assignment.");
            yield break;
        }

        GameObject clone = Instantiate(EnemyPrefabs[end], SpawnPoint.transform.position, Quaternion.identity);

        // ������ �� ���� ������Ʈ�� EnemyControl ������Ʈ�� Ȯ��
        EnemyControl enemy = clone.GetComponent<EnemyControl>();

        enemy.isrespawn = isrespawn;

        if (enemy == null)
        {
            Debug.LogError("Spawned enemy does not have an EnemyControl component.");
            yield break;
        }

        // ���� ���� �÷��� ����
        switch (whatdefence)
        {
            case 1:
                enemy.isLeftEnemy = true;
                Leftdefence.Add(enemy);
                break;
            case 2:
                enemy.isRightEnemy = true;
                Rightdefence.Add(enemy);
                break;
            case 3:
                enemy.isFrontEnemy = true;
                Frontdefence.Add(enemy);
                break;
            case 4:
                enemy.isAttackEnemy = true;
                AttackPlayer.Add(enemy);
                break;
            default:
                Debug.LogWarning("Invalid defence type provided.");
                break;
        }

        // ��ǥ ��ġ�� �̵���Ű��
        enemy.EnemyGoRally(pos);
    }
    void Update()
    {
        if(Building.Count > 0)
        {
            if (isTimerRunning)
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime >= totalTime)
                {
                    AttackPlayerEnemy(AttackPoint.transform.position);
                    Debug.Log("MoveEnemy ȣ��");

                    Attacking = true;
                    isTimerRunning = false;
                    AttackerCount += 5;
                    WaveCount += 1;

                    if (WaveCount > 5)
                    {
                        WaveCount = 5;
                    }
                }
            }

            if (Attacking == true)
            {

                ReAttackTimer += Time.deltaTime;

                if (ReAttackTimer >= 7)
                {
                    AttackPlayerEnemy(AttackPoint.transform.position);
                    Debug.Log("�����");
                    ReAttackTimer = 0;
                }
            }

            if (AttackPlayer.Count < 1)
            {
                Attacking = false;
                StartTimer();
            }

            if (Attacking == false)
            {
                if (AttackPlayer.Count < AttackerCount)
                {
                    Enemyspawntimer += Time.deltaTime;

                    if (Enemyspawntimer >= 15)
                    {
                        SpawnEnemy(Random.Range(WaveCount, 2 + WaveCount), RallyPoint.transform.position, 4, 15f, true);
                        Enemyspawntimer = 0;
                    }
                }
            }
        }
    }

    public void StartTimer()
    {
        isTimerRunning = true;
        elapsedTime = 0f;
    }
}
