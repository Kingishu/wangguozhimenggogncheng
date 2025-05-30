using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager layoutManager;
    public Vector3 deckPosition;
    [Header("抽卡动画事件")]
    public float duration;

    [SerializeField]
    private List<CardDataSO> drawDeck = new List<CardDataSO>();     // 抽牌堆
    private List<CardDataSO> discardDeck = new List<CardDataSO>();  // 弃牌堆
    private List<Card> handCardList = new List<Card>();             // 当前手牌

    [Header("广播事件")]
    public IntEventSO drawCountEvent;
    public IntEventSO discardCountEvent;

    // TODO: 测试使用，后续删除
    private void Start()
    {
        InitDeck();
    }

    public void NewTurnDrawCards()
    {
        DrawCard(4);
    }

    /// <summary>
    /// 初始化牌堆
    /// </summary>
    public void InitDeck()
    {
        drawDeck.Clear();
        foreach (var entry in cardManager.currentCardLibrary.cardLibraryList)
        {
            for (int i = 0; i < entry.amount; i++)
            {
                drawDeck.Add(entry.cardData);
            }
        }

        ShuffleCard();
    }

    [ContextMenu("测试抽牌")]
    public void TestDrawCard()
    {
        DrawCard(1);
    }

    /// <summary>
    /// 抽牌
    /// </summary>
    /// <param name="amount"></param>
    public void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // 抽牌堆空了, 洗重新牌
            if (drawDeck.Count == 0)
            {
                foreach (var item in discardDeck)
                {
                    drawDeck.Add(item);
                }
                ShuffleCard();
            }
            // 获得抽牌堆的第一张牌
            var currentcardData = drawDeck[0];
            drawDeck.RemoveAt(0);

            // 更新UI显示数字
            drawCountEvent.RaiseEvent(drawDeck.Count, this);

            // 生成手牌
            var card = cardManager.GetCardObject().GetComponent<Card>();

            // 初始化
            card.Init(currentcardData);
            card.transform.position = deckPosition;

            handCardList.Add(card);

            var delay = i * 0.2f;
            SetCardLayout(delay);
        }
    }

    /// <summary>
    /// 设置卡牌布局
    /// </summary>
    /// <param name="delay"></param>
    private void SetCardLayout(float delay)
    {
        for (int i = 0; i < handCardList.Count; i++)
        {
            Card currentCard = handCardList[i];

            CardTransform cardTransform = layoutManager.GetCardTransforms(i, handCardList.Count);
            // currentCard.transform.SetPositionAndRotation(cardTransform.pos, cardTransform.rotation);

            // 卡牌能量判断
            currentCard.UpdateCardState();

            currentCard.isAnimating = true;
            currentCard.transform.DOScale(Vector3.one, duration * 0.5f).SetDelay(delay).onComplete = () =>
            {
                currentCard.transform.DOMove(cardTransform.pos, duration)
                .onComplete = () => currentCard.isAnimating = false;
                currentCard.transform.DORotateQuaternion(cardTransform.rotation, duration);
            };

            // 设置卡牌排序
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            currentCard.UpdatePositionRotation(cardTransform.pos, cardTransform.rotation);
        }
    }

    /// <summary>
    /// 洗牌
    /// </summary>
    private void ShuffleCard()
    {
        discardDeck.Clear();
        // 更新 UI 显示数量
        drawCountEvent.RaiseEvent(drawDeck.Count, this);
        discardCountEvent.RaiseEvent(discardDeck.Count, this);

        for (int i = 0; i < drawDeck.Count; i++)
        {
            CardDataSO temp = drawDeck[i];
            int randomIndex = Random.Range(i, drawDeck.Count);
            drawDeck[i] = drawDeck[randomIndex];
            drawDeck[randomIndex] = temp;
        }
    }

    /// <summary>
    /// 弃牌，事件函数
    /// </summary>
    /// <param name="card"></param>
    public void DiscardCard(object obj)
    {
        Card card = obj as Card;

        // 将手牌移除到弃牌堆
        discardDeck.Add(card.cardData);
        handCardList.Remove(card);

        // 回收卡牌对象
        cardManager.DiscardCard(card.gameObject);

        // 更新UI显示数字
        discardCountEvent.RaiseEvent(discardDeck.Count, this);

        // 重新设置布局
        SetCardLayout(0f);
    }

    /// <summary>
    /// 玩家回合结束弃牌，事件函数
    /// </summary>
    public void OnPlayerTurnEnd()
    {
        for (int i = 0; i < handCardList.Count; i++)
        {
            // 将手牌移除到弃牌堆
            discardDeck.Add(handCardList[i].cardData);
            // 回收卡牌对象
            cardManager.DiscardCard(handCardList[i].gameObject);
        }

        handCardList.Clear();
        // 更新UI显示数字
        discardCountEvent.RaiseEvent(discardDeck.Count, this);
    }

    /// <summary>
    /// 回收所有卡牌，事件函数，游戏胜利时调用
    /// </summary>
    /// <param name="obj"></param>
    public void ReleaseAllCards(object obj)
    {
        foreach (var card in handCardList)
        {
            cardManager.DiscardCard(card.gameObject);
        }

        handCardList.Clear();
        InitDeck();

    }

}
