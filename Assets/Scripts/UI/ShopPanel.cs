using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopPanel : MonoBehaviour
{
    public VisualElement rootEltment;
    private Button buyCardButton;
    private Button backToMapButton;

    [Header("广播事件")]
    public ObjectEventSO loadMapEvent;
    public ObjectEventSO ButtonHoverEvent;
    public ObjectEventSO buyCardEvent;

    private void OnEnable()
    {

        rootEltment = GetComponent<UIDocument>().rootVisualElement;
        buyCardButton = rootEltment.Q<Button>("BuyCardButton");
        backToMapButton = rootEltment.Q<Button>("BackToMapButton");

        backToMapButton.clicked += OnBackToMapButtonClicked;
        buyCardButton.clicked += OnBuyCardButtonClicked;
        
        backToMapButton.RegisterCallback<MouseEnterEvent>(OnMouseHover);
        buyCardButton.RegisterCallback<MouseEnterEvent>(OnMouseHover);
    }
    
    private void OnMouseHover(MouseEnterEvent evt)
    {
        ButtonHoverEvent.RaiseEvent(null,this);
    }
    
    private void OnBuyCardButtonClicked()
    {
        //执行商店相关逻辑
       buyCardEvent.RaiseEvent(null, this);
    }

    private void OnBackToMapButtonClicked()
    {
        loadMapEvent.RaiseEvent(null, this);
    }

    public void OnFinishBuyCardEvent()
    {
        buyCardButton.style.display = DisplayStyle.None;
    }


}