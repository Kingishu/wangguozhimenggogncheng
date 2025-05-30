using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    public PlayableDirector director;
    public Button stopButton;

    [Header("广播事件")]
    public ObjectEventSO loadMenuEvent;

    private void Awake()
    {
        director.GetComponent<PlayableDirector>();
        director.stopped += OnPlayableDirectorStopped;

        stopButton.onClick.AddListener(OnPointerDown);
    }

    private void OnPointerDown()
    {
        if (director.state == PlayState.Playing)
        {
            director.Stop();
        }
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {

        loadMenuEvent.RaiseEvent(null, this);
    }
}
