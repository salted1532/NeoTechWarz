using UnityEngine;

public class Pointer : MonoBehaviour
{

    public float rotationSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,rotationSpeed * Time.deltaTime, 0));
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(gameObject);
        }
    }
}
