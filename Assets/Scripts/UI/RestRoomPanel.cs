using System;
using UnityEngine;
using UnityEngine.UIElements;

public class RestRoomPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button restButton, backToMapButton;

    public CardEffect restEffect;

    private CharacterBase player;

    [Header("广播事件")]
    public ObjectEventSO LoadMapEvent;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        restButton = rootElement.Q<Button>("RestButton");
        backToMapButton = rootElement.Q<Button>("BackToMapButton");

        player = FindAnyObjectByType<Player>(FindObjectsInactive.Include);

        restButton.clicked += OnRestButtonClicked;
        backToMapButton.clicked += OnBackToMapButtonClicked;
    }

    private void OnBackToMapButtonClicked()
    {
        LoadMapEvent.RaiseEvent(null, this);
    }

    private void OnRestButtonClicked()
    {
        restEffect.Execute(player, null);
        restButton.SetEnabled(false);
    }
}
