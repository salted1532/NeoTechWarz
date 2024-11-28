using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    // �����̴��� �����ϱ� ���� ����
    public Slider volumeSlider;

    void Start()
    {
        // �����̴��� �ʱⰪ�� ���� ����� ������ ���� ������ ����
        volumeSlider.value = AudioListener.volume;

        // �����̴��� �����ʸ� �߰��Ͽ� ���� ����� ������ ȣ��Ǵ� �޼��� ����
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // �����̴� ���� ����� �� ȣ��Ǵ� �޼���
    public void SetVolume(float volume)
    {
        // AudioListener�� ������ �����̴� ���� �°� ����
        AudioListener.volume = volume;
    }
}
