using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float movementTime;
    public float movementSpeed;

    public float panBorderThickness = 10f; // ȭ�� �����ڸ� �β�
    public Vector2 panLimit; // ī�޶� �̵� ����

    public Vector3 newPosition;

    // ī�޶��� orthographic ������Ʈ�� �������� ���� ����
    private Camera cam;

    public float zoomSpeed = 1f;
    // �� ������ �����ϴ� ����
    public float minZoom = 5f;
    public float maxZoom = 20f;

    private Vector3 MainBasepos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        newPosition = transform.position;
        cam = GetComponent<Camera>();

        // ���콺 Ŀ�� ���
        Cursor.lockState = CursorLockMode.Confined;

        MainBasepos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
    }

    void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            newPosition = MainBasepos;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += ((transform.up + transform.forward).normalized * movementSpeed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }
        if ( Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += ((transform.up + transform.forward).normalized * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }
        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            newPosition += ((transform.up + transform.forward).normalized * movementSpeed);
        }
        if (Input.mousePosition.y <= panBorderThickness)
        {
            newPosition += ((transform.up + transform.forward).normalized * -movementSpeed);
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            newPosition += (transform.right * movementSpeed);
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            newPosition += (transform.right * -movementSpeed);
        }
        float scrollData = Input.GetAxis("Mouse ScrollWheel");

        // ī�޶��� orthographic size ����
        cam.orthographicSize -= scrollData * zoomSpeed;

        // orthographic size ���� minZoom�� maxZoom ���̿� �ֵ��� ����
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);

        // �̵� ���� �߰�
        newPosition.x = Mathf.Clamp(newPosition.x, -130, 40);
        newPosition.z = Mathf.Clamp(newPosition.z, -130, 40);

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }
}
