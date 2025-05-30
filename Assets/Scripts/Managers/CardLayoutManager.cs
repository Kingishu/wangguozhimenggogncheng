using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CardLayoutManager : MonoBehaviour
{
    public bool isHorizontal;
    public float maxWidth = 7f;
    public float cardSpacing = 2f;
    [Header("弧形参数")]
    public float angleBetweenCards = 7f;
    public float radius = 17f;
    public Vector3 centerPoint;

    [SerializeField]
    private List<Vector3> cardPosition = new List<Vector3>();
    private List<quaternion> cardRotation = new List<quaternion>();

    private void Awake()
    {
        centerPoint = isHorizontal ? Vector3.up * -4.5f : Vector3.up * -21.5f;
    }

    public CardTransform GetCardTransforms(int index, int totalCards)
    {
        CalculatePosition(totalCards, isHorizontal);

        return new CardTransform(cardPosition[index], cardRotation[index]);
    }

    /// <summary>
    /// 计算卡牌位置
    /// </summary>
    /// <param name="numberOfCards"></param>
    /// <param name="isHorizontal"></param>
    private void CalculatePosition(int numberOfCards, bool isHorizontal)
    {
        cardPosition.Clear();
        cardRotation.Clear();

        // 纵向排列
        if (isHorizontal)
        {
            float currentWidth = cardSpacing * (numberOfCards - 1);
            float totalWidth = Mathf.Min(currentWidth, maxWidth);

            float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfCards - 1) : 0;

            for (int i = 0; i < numberOfCards; i++)
            {
                float posX = 0 - (totalWidth / 2) + (i * currentSpacing);

                Vector3 pos = new Vector3(posX, centerPoint.y, 0);
                Quaternion rotation = Quaternion.identity;

                cardPosition.Add(pos);
                cardRotation.Add(rotation);
            }
        }
        // 扇形排列
        else
        {
            float cardAngle = (numberOfCards - 1) * angleBetweenCards / 2;

            for (int i = 0; i < numberOfCards; i++)
            {
                var pos = FanCardPosition(cardAngle - i * angleBetweenCards);

                var rotation = Quaternion.Euler(0, 0, cardAngle - i * angleBetweenCards);
                cardPosition.Add(pos);
                cardRotation.Add(rotation);
            }
        }
    }

    private Vector3 FanCardPosition(float angle)
    {
        return new Vector3(
            centerPoint.x - Mathf.Sin(Mathf.Deg2Rad * angle) * radius,
            centerPoint.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
            0
        );
    }
}
