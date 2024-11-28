using UnityEngine;

public class DefenceCoillder : MonoBehaviour
{
    public bool isLeft;
    public bool isRight;
    public bool isFront;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isLeft == true)
            {
                Debug.Log("왼쪽 공격");
                transform.parent.GetComponent<EnemyAI>().LeftDefenceEnemy(transform.position);
            }
            if (isRight == true)
            {
                Debug.Log("오른쪽 공격");
                transform.parent.GetComponent<EnemyAI>().RightDefenceEnemy(transform.position);
            }
            if (isFront == true)
            {
                Debug.Log("정면 공격");
                transform.parent.GetComponent<EnemyAI>().FrontDefenceEnemy(transform.position);
            }
        }
    }
}
