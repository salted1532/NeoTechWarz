using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    public int EnemyRange;
    public int EnemyAttackDamage;

    private Transform nearestPlayer;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") ||
            other.gameObject.CompareTag("MainBase") ||
            other.gameObject.CompareTag("Tier1") ||
            other.gameObject.CompareTag("Tier2") ||
            other.gameObject.CompareTag("Tier3") ||
            other.gameObject.CompareTag("Lap") ||
            other.gameObject.CompareTag("Supply Depot"))
        {
            Transform currentEnemy = other.transform;
            float minDistance = Mathf.Infinity;
            Vector3 currentPosition = transform.position;

            float distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPlayer = currentEnemy;
            }

            if (minDistance > EnemyRange)
            {
                //Debug.Log("추적중!!");
                transform.parent.GetComponent<EnemyControl>().EnemyMove(nearestPlayer.transform.position);
            }
            else
            {
                //Debug.Log("공격중!!!");
                transform.parent.GetComponent<EnemyControl>().EnemyAttack(nearestPlayer.transform.position, EnemyAttackDamage);
            }
        }
    }
}
