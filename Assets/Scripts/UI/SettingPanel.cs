using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class SettingPanel : MonoBehaviour
{
    public VisualElement rootElement;
    public Slider slider;
    public AudioMixer audioMixer;
    public Button back,exit;
    public ObjectEventSO backSettingEvent;
    

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        slider = rootElement.Q<Slider>("Slider");
        back = rootElement.Q<Button>("Back");
        exit = rootElement.Q<Button>("Exit");
        
        back.clicked += BackSettingEvent;
        exit.clicked += ExitGame;
        
        slider.RegisterValueChangedCallback(OnSliderChange);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void BackSettingEvent()
    {
        backSettingEvent.RaiseEvent(null,this);
    }


    private void OnSliderChange(ChangeEvent<float> evt)
    {
        audioMixer.SetFloat("MasterVolume", evt.newValue);
        print(evt.newValue);
    }
}
