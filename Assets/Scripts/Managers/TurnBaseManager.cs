using UnityEngine;

public class TurnBaseManager : MonoBehaviour
{
    public GameObject playerObj;
    private bool isPlayerTurn = false;
    private bool isEnemyTurn = false;
    public bool battleEnd = true;

    private float timeCounter;
    public float playerTurnDuration;
    public float enemyTurnDuration;

    public bool canClick=false;
    
    public GamePlayPanel gamePlayPanel;
    
    [Header("广播事件")]
    public ObjectEventSO playerTurnBeginEvent;
    public ObjectEventSO enemyTurnBeginEvent;
    public ObjectEventSO enemyTurnEndEvent;
    
  

    private void Update()
    {
        if (battleEnd)
        {
            return;
        }

        if (isEnemyTurn)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= enemyTurnDuration)
            {
                timeCounter = 0f;
                // 敌人回合结束
                EnemyTurnEnd();
                // 玩家回合开始
                isPlayerTurn = true;
            }
        }

        if (isPlayerTurn)
        {
            timeCounter += Time.deltaTime;
            canClick = false;
            if (timeCounter >= playerTurnDuration)
            {
                timeCounter = 0f;
                // 玩家回合开始
                PlayerTurnBegin();
                isPlayerTurn = false;
            }
        }
    }

    [ContextMenu("Game Start")]
    public void GameStart()
    {
        isPlayerTurn = true;
        isEnemyTurn = false;
        battleEnd = false;
        timeCounter = 0f;
    }

    public void PlayerTurnBegin()
    {
        playerTurnBeginEvent.RaiseEvent(null, this);
        canClick = true;
    }
    

    public void EnemyTurnBegin()
    {
        isEnemyTurn = true;
        enemyTurnBeginEvent.RaiseEvent(null, this);
    }

    public void EnemyTurnEnd()
    {
        isEnemyTurn = false;
        enemyTurnEndEvent.RaiseEvent(null, this);
    }

    public void OnAfterRoomLoadedEvent(object obj)
    {
        Room room = obj as Room;
        switch (room.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                playerObj.SetActive(true);
                GameStart();
                break;
            case RoomType.Shop:
            case RoomType.Treasure:
                playerObj.SetActive(false);
                break;
            case RoomType.ResetRoom:
                playerObj.SetActive(true);
                playerObj.GetComponent<Player>().SetSleepAnimation();
                break;
        }
    }

    public void StopTurnBaseSystem(object obj)
    {
        battleEnd = true;
        playerObj.SetActive(false);
    }

    public void NewGame()
    {
        playerObj.GetComponent<Player>().NewGame();
    }

}
