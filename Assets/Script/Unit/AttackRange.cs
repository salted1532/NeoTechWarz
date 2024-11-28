using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public int UnitRange;
    public int AttackDamage;

    private Transform nearestEnemy;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Transform currentEnemy = other.transform;
            float minDistance = Mathf.Infinity;
            Vector3 currentPosition = transform.position;

            // 거리 계산
            float distance = Vector3.Distance(currentPosition, currentEnemy.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = currentEnemy;
            }

            if (transform.parent.GetComponent<UnitController>().idleMode == 1)
            {
                if (minDistance > UnitRange)
                {
                    if (transform.parent.GetComponent<UnitController>().MoveMode == 0)
                    {
                        //Debug.Log("추적중!!");
                        transform.parent.GetComponent<UnitController>().AttackToGround(nearestEnemy.position);
                    }
                }
                else
                {
                    //Debug.Log("공격중!!!");
                    transform.parent.GetComponent<UnitController>().Attack(nearestEnemy.position, AttackDamage);
                }
            }
        }
    }
}
