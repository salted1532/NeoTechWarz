using UnityEngine;
using UnityEngine.UI;

public class CooldownTimer : MonoBehaviour
{
    public Image imageCooldown;

    public float cooldown = 250f;

    bool isCooldown = false;

    public int UpgradeCount = 0;

    void Start()
    {
        imageCooldown.fillAmount = 0f; // ������ �� ��ٿ� �̹����� �ʱ�ȭ
    }

    public void CooldownStart()
    {
        UpgradeCount += 1;
        if (!isCooldown) // ��ٿ��� �̹� ���� ���� ��� �ٽ� �������� ����
        {
            isCooldown = true;
            transform.GetComponent<Button>().interactable = false;
            imageCooldown.fillAmount = 1f; // ��ٿ� ���� �� �̹����� �� ä���
        }
    }

    void Update()
    {
        if (isCooldown)
        {
            imageCooldown.fillAmount -= 1f / cooldown * Time.deltaTime; // ä���� �̹����� ������ ����

            if (imageCooldown.fillAmount <= 0f)
            {
                imageCooldown.fillAmount = 0f;
                isCooldown = false;
                transform.GetComponent<Button>().interactable = true; // ��ٿ��� ������ ��ư Ȱ��ȭ

                if(UpgradeCount == 3)
                {
                    transform.gameObject.SetActive(false);
                }
            }
        }
    }
}
