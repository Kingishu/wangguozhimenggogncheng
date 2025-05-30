using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject arrowPrefab;
    private GameObject currentArrow;
    private Card currentCard;
    private bool canMove;           // 能否移动
    private bool canExecute;        // 能否执行效果
    private CharacterBase targetCharacter;

    private void Awake()
    {
        currentCard = GetComponent<Card>();
    }

    private void OnDisable()
    {
        canMove = false;
        canExecute = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 卡牌不可用
        if(!currentCard.isAvailable)
        {
            return;
        }

        switch (currentCard.cardData.cardType)
        {
            case CardType.Attack:
                currentArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
                canMove = false;
                break;
            case CardType.Abilities:
            case CardType.Defense:
                canMove = true;
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 卡牌不可用
        if(!currentCard.isAvailable)
        {
            return;
        }

        // 跟随鼠标移动
        if (canMove)
        {
            currentCard.isAnimating = true;
            Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

            currentCard.transform.position = worldPos;
            // 到达屏幕上方区域即可执行
            canExecute = worldPos.y > 0f;
        }
        // 攻击牌指针的情况
        else
        {
            if (eventData.pointerEnter == null) return;
            // 指向敌人
            if (eventData.pointerEnter.CompareTag("Enemy"))
            {
                canExecute = true;
                targetCharacter = eventData.pointerEnter.GetComponent<CharacterBase>();
                return;
            }
            canExecute = false;
            targetCharacter = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 卡牌不可用
        if(!currentCard.isAvailable)
        {
            return;
        }

        if (currentArrow != null)
        {
            Destroy(currentArrow);
        }
        if (canExecute)
        {
            // 执行卡牌效果
            currentCard.ExecuteCardEffects(currentCard.player, targetCharacter);
        }
        else
        {
            currentCard.ResetCardTransform();
        }
    }


}
