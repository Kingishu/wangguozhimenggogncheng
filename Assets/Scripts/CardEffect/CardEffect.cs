using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public int value;
    public EffectTargetType targetType;

    /// <summary>
    /// 执行卡牌效果
    /// </summary>
    /// <param name="from"></param>
    /// <param name="target"></param>
    public abstract void Execute(CharacterBase from, CharacterBase target);
}