using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public RoomDataSO roomData;
    public RoomState roomState;

    public int row;
    public int column;
    public List<Vector2Int> linkTo = new List<Vector2Int>();

    [Header("广播")]
    public ObjectEventSO loadRoomEvent;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        // Debug.Log("点击房间: " + roomData.roomType);
        if (roomState == RoomState.Attainable)
            loadRoomEvent.RaiseEvent(this, this);
    }

    /// <summary>
    /// 设置房间, 在创建时调用
    /// </summary>
    /// <param name="row">房间的列</param>
    /// <param name="col">房间的行</param>
    /// <param name="roomData">房间数据</param>
    public void SetupRoom(int column, int row, RoomDataSO roomData)
    {
        this.row = row;
        this.column = column;
        this.roomData = roomData;

        spriteRenderer.sprite = roomData.roomIcon;
        spriteRenderer.color = roomState switch
        {
            RoomState.Locked => Color.grey,
            RoomState.Visited => new Color(0.6f, 0.8f, 0.6f, 1f),
            RoomState.Attainable => Color.white,
            _ => throw new System.NotImplementedException(),
        };
    }
}
