using System;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    //声明单例类
    public static CoinManager instance;
    //声明当前游戏的金币数量
    public static int coins;
    private void Awake()
    {
        if (instance !=null)
        {
            Destroy(gameObject);
        }
        instance = this;

        coins = 0;
    }
/// <summary>
/// 敌人死亡的时候执行的函数,增加金币
/// </summary>
/// <param name="amount"></param>
    public void AddCoin(object obj)
    {
      var character=  obj as CharacterBase;
      coins+=character.coins;
    }

    public void TryBuyCard(int amount)
    {
        //当前金币数大于卡牌所需要的金币数,可以购买
        if (coins >= amount)
        {
            //执行购买相关逻辑
        }
        else
        {
            //不能购买
        }
    }
}
