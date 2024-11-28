using System.Collections.Generic;
using UnityEngine;

public class CollectRange : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (transform.parent.GetComponent<UnitController>().collectioning == true)
        {
            if (other.gameObject.CompareTag("MainBase"))
            {

                transform.parent.GetComponent<UnitController>().endCollect();

            }
        }
    }
}
