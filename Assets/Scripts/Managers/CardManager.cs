using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[DefaultExecutionOrder(-100)]
public class CardManager : MonoBehaviour
{
    public PoolTool poolTool;               // 卡牌对象池
    public List<CardDataSO> cardDataList;   // 游戏中所有可能出现的卡牌

    [Header("卡牌库")]
    public CardLibrarySO newGameCardLibrary;    // 新游戏初始卡牌库
    public CardLibrarySO currentCardLibrary;    // 当前玩家卡牌库

    private int previousIndex;
    
    private void Awake()
    {
        InitCardDataList();

        // 依据需求初始化卡牌库
        foreach (var item in newGameCardLibrary.cardLibraryList)
        {
            currentCardLibrary.cardLibraryList.Add(item);
        }
    }

    private void OnDisable() {
        currentCardLibrary.cardLibraryList.Clear();
    }

    #region 获得项目卡牌      
    /// <summary>
    /// 初始化获得所有项目卡牌资源
    /// </summary>
    private void InitCardDataList()
    {
        Addressables.LoadAssetsAsync<CardDataSO>("CardData", null).Completed += OnCardDataLoaded;
    }

    /// <summary>
    /// 回调函数，获得卡牌
    /// </summary>
    /// <param name="handle"></param>
    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            cardDataList = new List<CardDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError("Failed to load card data list");
        }
    }
    #endregion

    /// <summary>
    /// 抽卡时调用，获得卡牌 GameObject
    /// </summary>
    /// <returns></returns>
    public GameObject GetCardObject()
    {
        var cardObj = poolTool.GetObjectFromPool();
        cardObj.transform.localScale = Vector3.zero;
        return cardObj;
    }

    /// <summary>
    /// 弃牌，将卡牌回收到对象池
    /// </summary>
    /// <param name="cardObject"></param>
    public void DiscardCard(GameObject cardObject)
    {
        poolTool.ReturnObjectToPool(cardObject);
    }

    /// <summary>
    /// 获取卡牌数据
    /// </summary>
    /// <returns></returns>
    public CardDataSO GetNewCardData()
    {
        // 确保相邻两张卡牌不一样
        int randomIndex = 0;
        do{
            randomIndex = Random.Range(0, cardDataList.Count);
        }while(previousIndex == randomIndex);
        
        previousIndex = randomIndex;
        return cardDataList[randomIndex];
    }

    public void UnlockCard(CardDataSO newCardData)
    {
        CardLibraryEntry newCard = new CardLibraryEntry
        {
            cardData = newCardData,
            amount = 1,
        };
        if(currentCardLibrary.cardLibraryList.Contains(newCard))
        {
            var target = currentCardLibrary.cardLibraryList.Find(t => t.cardData == newCardData);
            target.amount++;
        }
        else
        {
            currentCardLibrary.cardLibraryList.Add(newCard);
        }
    }
}
