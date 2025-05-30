using UnityEngine;

[CreateAssetMenu(fileName = "DefenseEffect", menuName = "Card Effect/DefenseEffect")]
public class DefenseEffect : CardEffect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        switch (targetType)
        {
            case EffectTargetType.Self:
                from.UpdateDefense(value);
                break;
            case EffectTargetType.Target:
                target.UpdateDefense(value);
                break;
            case EffectTargetType.All:
                break;
        }
    }

}