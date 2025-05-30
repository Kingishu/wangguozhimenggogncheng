using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyActionSO", menuName = "Enemy/EnemyActionSO")]
public class EnemyActionSO : ScriptableObject
{
    public List<EnemyAction> actions;
}

[System.Serializable]
public struct EnemyAction
{
    public Sprite intentSprite;
    public CardEffect effect;
}