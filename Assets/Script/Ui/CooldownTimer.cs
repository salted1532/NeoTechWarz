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
        imageCooldown.fillAmount = 0f; // 시작할 때 쿨다운 이미지를 초기화
    }

    public void CooldownStart()
    {
        UpgradeCount += 1;
        if (!isCooldown) // 쿨다운이 이미 진행 중인 경우 다시 시작하지 않음
        {
            isCooldown = true;
            transform.GetComponent<Button>().interactable = false;
            imageCooldown.fillAmount = 1f; // 쿨다운 시작 시 이미지를 꽉 채우기
        }
    }

    void Update()
    {
        if (isCooldown)
        {
            imageCooldown.fillAmount -= 1f / cooldown * Time.deltaTime; // 채워진 이미지를 서서히 줄임

            if (imageCooldown.fillAmount <= 0f)
            {
                imageCooldown.fillAmount = 0f;
                isCooldown = false;
                transform.GetComponent<Button>().interactable = true; // 쿨다운이 끝나면 버튼 활성화

                if(UpgradeCount == 3)
                {
                    transform.gameObject.SetActive(false);
                }
            }
        }
    }
}
