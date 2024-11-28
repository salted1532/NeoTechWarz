using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float movementTime;
    public float movementSpeed;

    public float panBorderThickness = 10f; // 화면 가장자리 두께
    public Vector2 panLimit; // 카메라 이동 제한

    public Vector3 newPosition;

    // 카메라의 orthographic 컴포넌트를 가져오기 위한 변수
    private Camera cam;

    public float zoomSpeed = 1f;
    // 줌 범위를 설정하는 변수
    public float minZoom = 5f;
    public float maxZoom = 20f;

    private Vector3 MainBasepos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        newPosition = transform.position;
        cam = GetComponent<Camera>();

        // 마우스 커서 잠금
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

        // 카메라의 orthographic size 조정
        cam.orthographicSize -= scrollData * zoomSpeed;

        // orthographic size 값이 minZoom과 maxZoom 사이에 있도록 제한
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);

        // 이동 제한 추가
        newPosition.x = Mathf.Clamp(newPosition.x, -130, 40);
        newPosition.z = Mathf.Clamp(newPosition.z, -130, 40);

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }
}
