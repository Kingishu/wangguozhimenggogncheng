using UnityEngine;

[CreateAssetMenu(fileName = "StrengthEffect", menuName = "Card Effect/StrengthEffect")]
public class StrengthEffect : CardEffect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        switch (targetType)
        {
            case EffectTargetType.Self:
                from.SetupStrength(value, true);
                break;
            case EffectTargetType.Target:
                target.SetupStrength(value, false);
                break;
            case EffectTargetType.All:
                break;
        }
    }
}