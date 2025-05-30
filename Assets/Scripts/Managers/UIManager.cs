using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI面板")]
    public GameObject gamePlayPanel;
    public GameObject gameWinPanel;
    public GameObject gameOverPanel;
    public GameObject pickCardPanel;
    public GameObject restRoomPanel;
    public GameObject shopPanel;
    public GameObject buyCardPanel;
    public GameObject settingPanel;

    public void OnAfterRoomLoadedEvent(object data)
    {
        Room currentRoom = (Room)data;

        switch (currentRoom.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                gamePlayPanel.SetActive(true);
                gamePlayPanel.GetComponent<GamePlayPanel>().endTurnButton.SetEnabled(false);
                
                break;
            case RoomType.Shop:
                shopPanel.SetActive(true);
                break;
            case RoomType.Treasure:
                break;
            case RoomType.ResetRoom:
                restRoomPanel.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// 关闭所有UI面板，  loadmap / load menu
    /// </summary>
    public void HideAllPanels()
    {
        gamePlayPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        restRoomPanel.SetActive(false);
        shopPanel.SetActive(false);
    }

    public void OnGameWinEvent()
    {
        gameWinPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
    }

    public void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
    }

    public void OnPickCardEvent()
    {
        pickCardPanel.SetActive(true);
    }

    public void OnFinighPickCardEvent()
    {
        pickCardPanel.SetActive(false);
        buyCardPanel.SetActive(false);
    }

    public void OnBuyCardEvent()
    {
        buyCardPanel.SetActive(true);
    }

    public void OnSettingButtonClicked()
    {
        settingPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void BackSettingEvent()
    {
        settingPanel.SetActive(false);
        Time.timeScale = 1;
    }

}
