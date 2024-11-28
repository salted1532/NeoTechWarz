using UnityEngine;

public class MissionObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Worker"))
        {
            if (other == null)
            {
                transform.position = transform.position;
            }
            else
            {
                Vector3 mypos = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z + 2);
                transform.position = mypos;
            }
        }
    }
}
