using System.Collections;
using UnityEngine;

public class EnemyBase : CharacterBase
{
    public EnemyActionSO actionDataSO;
    public EnemyAction currentAction;
    
    

    protected Player player;

    /// <summary>
    /// 事件函数，玩家回合开始时更新敌人意图
    /// </summary>
    public virtual void OnPlayerTurnBegin()
    {
        int randomIndex = Random.Range(0, actionDataSO.actions.Count);
        currentAction = actionDataSO.actions[randomIndex];
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public virtual void OnEnemyTurnBegin()
    {
        ResetDefense();
        switch (currentAction.effect.targetType)
        {
            case EffectTargetType.Self:
                Skill();
                break;
            case EffectTargetType.Target:
                Attack();
                break;
            case EffectTargetType.All:
                break;
        }
    }

    public virtual void Attack()
    {
        StartCoroutine(DelayAction("attack"));
    }

    public virtual void Skill()
    {
        StartCoroutine(DelayAction("skill"));
    }

    IEnumerator DelayAction(string actionName)
    {
        animator.SetTrigger(actionName);
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f > 0.6f &&
            !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).IsName(actionName)
        );

        if (actionName == "attack")
            currentAction.effect.Execute(this, player);
        else
            currentAction.effect.Execute(this, this);
    }

}
