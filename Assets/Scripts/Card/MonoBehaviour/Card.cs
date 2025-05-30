using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("组件")]
    public SpriteRenderer cardSprite;
    public TextMeshPro nameText, costText, descriptionText, typeText;

    public CardDataSO cardData;
    

    [Header("原始数据")]
    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public int originalLayerOrder;
    [Header("抽出数据")]
    public float enterPosY = -3.5f;
    public bool isAnimating;
    public bool isAvailable;

    public Player player;

    [Header("广播事件")]
    public ObjectEventSO discardCardEvet;
    public IntEventSO cardCostEvent;

    private void Start()
    {
        Init(cardData);
    }

    public void Init(CardDataSO data)
    {
        cardData = data;
        // 初始化卡牌的属性、效果等
        cardSprite.sprite = cardData.cardImage;
        nameText.text = cardData.cardName;
        costText.text = cardData.cost.ToString();
        descriptionText.text = cardData.description;
        typeText.text = cardData.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "能力",
            CardType.Abilities => "技能",
            _ => throw new System.NotImplementedException(),
        };

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    /// <summary>
    /// 更新原始位置
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public void UpdatePositionRotation(Vector3 position, Quaternion rotation)
    {
        originalPosition = position;
        originalRotation = rotation;
        originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isAnimating) return;
        // transform.position = originalPosition + Vector3.up;
        // 卡牌位置上移到固定高度显示
        
        transform.position = new Vector3(originalPosition.x, enterPosY, 0);
        transform.rotation = Quaternion.identity;
        GetComponent<SortingGroup>().sortingOrder = 20;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isAnimating) return;
        // 重置卡牌位置
        ResetCardTransform();
        
    }

    /// <summary>
    /// 重置卡牌位置
    /// </summary>
    public void ResetCardTransform()
    {
        transform.SetPositionAndRotation(originalPosition, originalRotation);
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;

        // 重置动画状态
        isAnimating = false;
    }

    /// <summary>
    /// 执行卡牌效果
    /// </summary>
    /// <param name="from"></param>
    /// <param name="target"></param>
    public void ExecuteCardEffects(CharacterBase from, CharacterBase target)
    {
        // 减少对应能量
        cardCostEvent.RaiseEvent(cardData.cost, this);

        // 通知回收卡牌
        discardCardEvet.RaiseEvent(this, this);
        foreach (var effect in cardData.cardEffects)
        {
            effect.Execute(from, target);
        }
    }

    public void UpdateCardState()
    {
        isAvailable = cardData.cost <= player.CurrentMana;
        costText.color = isAvailable ? Color.green : Color.red;
    }
}
