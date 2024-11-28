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
    //������ �޾ƿ��� 
    public void TakeDamage(float amount)
    {
        Debug.Log("������ ����");
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

        // ī�޶� �ٶ󺸴� ���� ���͸� ����մϴ�.
        Vector3 lookDirection = mainCamera.transform.position - transform.position;
        lookDirection.y = 0; // y �� ȸ���� �����Ϸ��� y ���� 0���� �����մϴ�.

        // ���� ���͸� �������� ȸ���� ����մϴ�.
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        // X ���� -90���� �����մϴ�.
        targetRotation *= Quaternion.Euler(-90f, 0f, 0f);

        // ȸ���� �����մϴ�.
        HpCanvas.transform.rotation = targetRotation;

        //HpCanvas.transform.rotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));

        //�׾����� Ȯ��
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
