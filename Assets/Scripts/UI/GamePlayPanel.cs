using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class GamePlayPanel : MonoBehaviour
{
    private VisualElement rootElement;      // panel 根节点
    private Label energyAmountLabel, drawAmountLabel, discardAmountLabel, turnLabel,coinlabel;
    public Button endTurnButton,settingButton;           // 结束回合按钮
    
    [Header("广播事件")]
    public ObjectEventSO playerTurnEndEvent;
    public ObjectEventSO settingButtonEvent;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;

        // 获取子元素
        energyAmountLabel = rootElement.Q<Label>("EnergyAmount");
        drawAmountLabel = rootElement.Q<Label>("DrawAmount");
        discardAmountLabel = rootElement.Q<Label>("DiscardAmount");
        turnLabel = rootElement.Q<Label>("TurnLabel");
        coinlabel = rootElement.Q<Label>("CoinAmount");
        endTurnButton = rootElement.Q<Button>("EndTurn");
        settingButton = rootElement.Q<Button>("Setting");

        endTurnButton.clicked += OnEndTurnButtonClicked;
        settingButton.clicked += OnSettingButtonClicked;

        // 初始化 UI
        drawAmountLabel.text = "0";
        discardAmountLabel.text = "0";
        energyAmountLabel.text = "0";
        coinlabel.text = "0";
        turnLabel.text = "游戏开始";
        
        
    }

    private void OnSettingButtonClicked()
    {
        settingButtonEvent.RaiseEvent(null,this);
    }

    private void Update()
    {
        coinlabel.text = CoinManager.coins.ToString();
    }

    // 结束回合按钮点击事件
    private void OnEndTurnButtonClicked()
    { 
        playerTurnEndEvent.RaiseEvent(null, this);
        
    }

    // 更新能量数字UI，事件函数
    public void UpdateEnergyAmount(int amount)
    {
        energyAmountLabel.text = amount.ToString();
    }

    // 更新抽牌数字 UI，事件函数
    public void UpdateDrawDeckAmount(int amount)
    {
        drawAmountLabel.text = amount.ToString();
    }

    // 更新弃牌数字 UI，事件函数
    public void UpdateDiscardDeckAmount(int amount)
    {
        discardAmountLabel.text = amount.ToString();
    }

    // 敌方回合开始，事件函数
    public void OnEnemyTurnBegin()
    {
        endTurnButton.SetEnabled(false);
        turnLabel.text = "敌方回合";
        turnLabel.style.color = Color.red;
    }

    // 玩家回合开始，事件函数
    public void OnPlayerTurnBegin()
    {
        endTurnButton.SetEnabled(true);
        turnLabel.text = "玩家回合";
        turnLabel.style.color = Color.white;
    }

    public void OnGameWinEvent()
    {
        endTurnButton.SetEnabled(false);
        
    }
    //在敌人死亡之后不能点击切换按钮
    public void OnCharacterDeathEvent()
    {
        endTurnButton.SetEnabled(false);
    }
    
    
  
    
}
