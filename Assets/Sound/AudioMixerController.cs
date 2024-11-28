using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    // 슬라이더를 연결하기 위한 변수
    public Slider volumeSlider;

    void Start()
    {
        // 슬라이더의 초기값을 현재 오디오 리스너 볼륨 값으로 설정
        volumeSlider.value = AudioListener.volume;

        // 슬라이더에 리스너를 추가하여 값이 변경될 때마다 호출되는 메서드 연결
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // 슬라이더 값이 변경될 때 호출되는 메서드
    public void SetVolume(float volume)
    {
        // AudioListener의 볼륨을 슬라이더 값에 맞게 조정
        AudioListener.volume = volume;
    }
}
