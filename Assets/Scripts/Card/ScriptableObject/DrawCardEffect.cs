using UnityEngine;

[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "Card Effect/DrawCardEffect")]
public class DrawCardEffect : CardEffect
{
    public IntEventSO drawCardEffect;
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        // 实现抽卡效果
        drawCardEffect?.RaiseEvent(value, this);
    }
}