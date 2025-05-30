using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameWinPanel : MonoBehaviour
{
    public VisualElement rootEltment;
    private Button pickCardButton;
    private Button backToMapButton;

    [Header("广播事件")]
    public ObjectEventSO loadMapEvent;
    public ObjectEventSO pickCardEvent;
    public ObjectEventSO ButtonHoverEvent;

    private void OnEnable()
    {

        rootEltment = GetComponent<UIDocument>().rootVisualElement;
        pickCardButton = rootEltment.Q<Button>("PickCardButton");
        backToMapButton = rootEltment.Q<Button>("BackToMapButton");

        backToMapButton.clicked += OnBackToMapButtonClicked;
        pickCardButton.clicked += OnPickCardButtonClicked;
        
        backToMapButton.RegisterCallback<MouseEnterEvent>(OnMouseHover);
        pickCardButton.RegisterCallback<MouseEnterEvent>(OnMouseHover);
    }
    
    private void OnMouseHover(MouseEnterEvent evt)
    {
        ButtonHoverEvent.RaiseEvent(null,this);
    }
    
    private void OnPickCardButtonClicked()
    {

        pickCardEvent.RaiseEvent(null, this);
    }

    private void OnBackToMapButtonClicked()
    {
        loadMapEvent.RaiseEvent(null, this);
    }

    public void OnFinishPickCardEvent()
    {
        pickCardButton.style.display = DisplayStyle.None;
    }
}
