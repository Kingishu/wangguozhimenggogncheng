using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Card Effect/DamageEffect")]
public class DamageEffect : CardEffect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (target == null) return;
        switch (targetType)
        {
            case EffectTargetType.Self:
                break;
            // 单体目标
            case EffectTargetType.Target:
                // 计算伤害加成
                int damage = (int)math.round(from.baseStrength * value);
               
                target.TakeDamage(damage);
                break;
            // 群体目标
            case EffectTargetType.All:
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<CharacterBase>().TakeDamage(value);
                }
                break;
        }
    }
}