using UnityEngine;

public class Bicon : MonoBehaviour
{
    [SerializeField]
    private bool Obj1 = false;
    [SerializeField]
    private bool Obj2 = false;
    [SerializeField]
    private bool Obj3 = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Obj1 == true)
        {
            if(Obj2 == true)
            {
                if(Obj3 == true)
                {
                    transform.parent.GetComponent<WinMission>().Collectobjects = true;
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Obj1"))
        {
            Obj1 = true;
        }
        if (other.gameObject.CompareTag("Obj2"))
        {
            Obj2 = true;
        }
        if (other.gameObject.CompareTag("Obj3"))
        {
            Obj3 = true;
        }
    }
}
