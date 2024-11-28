using UnityEngine;
using UnityEngine.UI;

public class UnitHealth : MonoBehaviour
{
    public float CurrentHp;
    public Slider Hpslider;
    public GameObject HpCanvas;

    Camera mainCamera;

    public bool isEnemy;
    public bool isBuilding;

    public static float UpgradedAttackPower = 0;
    public static float UpgradedDefense = 0;

    public void Start()
    {
        Hpslider.maxValue = CurrentHp;
        Hpslider.value = CurrentHp;

        mainCamera = Camera.main;
    }
    //데미지 받아오기 
    public void TakeDamage(float amount)
    {
        Debug.Log("데미지 받음");
        if(isEnemy == true)
        {
            float TDamge = amount + UpgradedAttackPower;

            if (TDamge > 0)
            {
                CurrentHp -= TDamge;
            }
            if (TDamge < 0)
            {
                CurrentHp -= 1;
            }
            Hpslider.value = CurrentHp;
            Debug.Log(Hpslider.value);
        }
        else
        {
            float TDamge = amount - UpgradedDefense;

            if (TDamge > 0)
            {
                CurrentHp -= TDamge;
            }
            if (TDamge < 0)
            {
                CurrentHp -= 1;
            }
            Hpslider.value = CurrentHp;
            Debug.Log(Hpslider.value);
        }

    }

    private void Update()
    {

        // 카메라를 바라보는 방향 벡터를 계산합니다.
        Vector3 lookDirection = mainCamera.transform.position - transform.position;
        lookDirection.y = 0; // y 축 회전을 고정하려면 y 값을 0으로 설정합니다.

        // 방향 벡터를 기준으로 회전을 계산합니다.
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        // X 값을 -90으로 고정합니다.
        targetRotation *= Quaternion.Euler(-90f, 0f, 0f);

        // 회전을 적용합니다.
        HpCanvas.transform.rotation = targetRotation;

        //HpCanvas.transform.rotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));

        //죽었는지 확인
        if (CurrentHp <= 0)
        {
            if(isEnemy == false)
            {
                if(isBuilding == false)
                {
                    transform.GetComponent<UnitController>().RemoveUnit();
                }
                else if(isBuilding == true)
                {
                    transform.GetComponent<BuildingController>().RemoveBuilding();
                }
            }
            else if(isEnemy == true)
            {
                transform.GetComponent<EnemyControl>().RemoveEnemy();
            }

        }
    }
}
