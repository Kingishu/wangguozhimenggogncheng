using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PickCardPanel : MonoBehaviour
{
    public CardManager cardManager;
    private VisualElement rootElement;
    public VisualTreeAsset cardTemplate;
    private VisualElement cardContainer;
    private CardDataSO currentCardData;
    public int cardAmount;

    private Button confirmButton;

    private List<Button> cardButtons = new List<Button>();

    [Header("广播事件")]
    public ObjectEventSO finishPickCardsEvent;
    public ObjectEventSO ButtonHoverEvent;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        cardContainer = rootElement.Q<VisualElement>("Container");
        confirmButton = rootElement.Q<Button>("ConfirmButton");

        confirmButton.clicked += OnConfirmButtonClicked;
        confirmButton.RegisterCallback<MouseEnterEvent>(OnMouseHover);
        
        for (int i = 0; i < cardAmount; i++)
        {
            var card = cardTemplate.Instantiate();
            var data = cardManager.GetNewCardData();
            // 初始化
            InitCard(card, data);
            var cardButton = card.Q<Button>("Card");
            cardContainer.Add(card);
            cardButtons.Add(cardButton);

            cardButton.clicked += () => OnCardClicked(cardButton, data);
            cardButton.RegisterCallback<MouseEnterEvent>(OnMouseHover);
        }
    }

    private void OnMouseHover(MouseEnterEvent evt)
    {
        ButtonHoverEvent.RaiseEvent(null,this);
    }
    private void OnConfirmButtonClicked()
    {
        cardManager.UnlockCard(currentCardData);
        finishPickCardsEvent.RaiseEvent(null, this);
    }

    private void OnCardClicked(Button cardButton, CardDataSO data)
    {
        currentCardData = data;

       
        for (int i = 0; i < cardButtons.Count; i++)
        {
            if (cardButtons[i] == cardButton)
            {
                cardButtons[i].SetEnabled(false);
            }
            else
            {
                cardButtons[i].SetEnabled(true);
            }
        }

    }

    public void InitCard(VisualElement card, CardDataSO cardData)
    {
        card.dataSource = cardData;

        var cardSpriteElement = card.Q<VisualElement>("CardSprite");
        var cardName = card.Q<Label>("CardName");
        var cardCost = card.Q<Label>("EnergyCost");
        var cardDescription = card.Q<Label>("CardDescription");
        var cardType = card.Q<Label>("CardType");

        cardSpriteElement.style.backgroundImage = new StyleBackground(cardData.cardImage);

        cardName.text = cardData.cardName;
        cardCost.text = cardData.cost.ToString();
        cardDescription.text = cardData.description;
        cardType.text = cardData.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "能力",
            CardType.Abilities => "技能",
            _ => throw new System.NotImplementedException(),
        };
    }
}
