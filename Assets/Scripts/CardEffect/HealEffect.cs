using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Card Effect/HealEffect")]
public class HealEffect : CardEffect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        switch (targetType)
        {
            case EffectTargetType.Self:
                from.HealHealth(value);
                break;
            case EffectTargetType.Target:
                target.HealHealth(value);
                break;
            case EffectTargetType.All:
                break;
        }
    }
}