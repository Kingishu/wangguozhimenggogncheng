using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreasureButton : MonoBehaviour, IPointerDownHandler
{
    public ObjectEventSO gameWinEvent;
    public int count = 0;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (count == 0)
        {
            gameWinEvent.RaiseEvent(null, this);
            count++;
        }
        
    }
    //加载地图时候清空clearCount
    public void ClearCount()
    {
        StartCoroutine(DelayClearCount());
    }

    IEnumerator DelayClearCount()
    {
        yield return new WaitForSeconds(1f);
        count = 0;
    }


}
