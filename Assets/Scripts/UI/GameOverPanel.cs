using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverPanel : MonoBehaviour
{
    public Button backToMenuButton;
    public ObjectEventSO loadMenuEvent;

    private void OnEnable()
    {
        GetComponent<UIDocument>().rootVisualElement.Q<Button>("BackToMenuButton").clicked += BackToMenu;
    }

    private void BackToMenu()
    {
        loadMenuEvent.RaiseEvent(null, this);
    }
}
