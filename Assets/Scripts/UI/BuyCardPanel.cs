using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuyCardPanel: MonoBehaviour
{
    public CardManager cardManager;
    private VisualElement rootElement;
    public VisualTreeAsset cardTemplate;
    private VisualElement cardContainer;
    private Label coinLabel,error;
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
        coinLabel = rootElement.Q<Label>("CoinLabel");
        error = rootElement.Q<Label>("Error");

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

    private void Update()
    {
        coinLabel.text = CoinManager.coins.ToString();
    }

    private void OnMouseHover(MouseEnterEvent evt)
    {
        ButtonHoverEvent.RaiseEvent(null,this);
    }
    private void OnConfirmButtonClicked()
    {
        
        finishPickCardsEvent.RaiseEvent(null, this);
    }

    private void OnCardClicked(Button cardButton, CardDataSO data)
    {
        currentCardData = data;
        //金币足够,可以购买
        if (CoinManager.coins>=50)
        {
            cardManager.UnlockCard(currentCardData);
            CoinManager.coins -= 50;
            for (int i = 0; i < cardButtons.Count; i++)
            {
                if (cardButtons[i] == cardButton)
                {
                    cardButtons[i].SetEnabled(false);
                }
                
            }
        }
        //金币不足,不能购买
        else
        {
            Debug.Log("金币不足,不能购买");
            StartCoroutine(DelayDisPlay());

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

    IEnumerator DelayDisPlay()
    {
        error.style.display = DisplayStyle.Flex;
        yield return new WaitForSeconds(1.5f);
        error.style.display = DisplayStyle.None;
    }
}
