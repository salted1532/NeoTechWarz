using UnityEngine;

public class MainMenuOption : MonoBehaviour
{
    public GameObject Optioninterface;

    public GameObject HowToPlay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OptionOn()
    {
        Optioninterface.SetActive(true);
    }
    public void Optionoff()
    {
        Optioninterface.SetActive(false);
    }
    public void HowtoPlayOn()
    {
        HowToPlay.SetActive(true);
    }
    public void HowtoPlayOff()
    {
        HowToPlay.SetActive(false);
    }
}
