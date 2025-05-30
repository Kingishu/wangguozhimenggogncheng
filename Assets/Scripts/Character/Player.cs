using UnityEngine;

public class Player : CharacterBase
{
    public IntVariable playerMana;          // 玩家能量
    public int maxMana;

    public int CurrentMana
    {
        get => playerMana.currentValue;
        set => playerMana.SetValue(value);
    }

    private void OnEnable()
    {
        playerMana.maxValue = maxMana;
        CurrentMana = playerMana.maxValue;
    }

    /// <summary>
    /// 玩家新回合，监听事件函数
    /// </summary>
    public void NewTurn()
    {
        CurrentMana = maxMana;
    }

    public void UpdateMana(int cost)
    {
        CurrentMana -= cost;
        if (CurrentMana < 0)
        {
            CurrentMana = 0;
        }
    }

    public void NewGame()
    {
        CurrentHp = MaxHp;
        isDead = false;
        buffRound.currentValue = 0;
        NewTurn();
    }

    public void SetSleepAnimation()
    {
        animator.Play("death");
    }
    /// <summary>
    /// 加载的时候调用,保证玩家血量不更新
    /// </summary>
    public void SetLoadBool()
    {
        isLoadGame = true;
        print("我执行了SetLoadBool代码");
    }
    /// <summary>
    /// 新游戏的时候调用,保证玩家满血
    /// </summary>
    public void SetNewLoadBool()
    {
        isLoadGame = false;
        print("我执行了SetLoadBool代码");
    }

}
