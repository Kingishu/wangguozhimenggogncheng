using UnityEngine;
using UnityEngine.Events;

public class BaseEventSO<T> : ScriptableObject
{
    [TextArea]
    public string description;

    public UnityAction<T> OnEventRaised;
    public string lastSender;


    public void RaiseEvent(T value, object sender)
    {
        OnEventRaised?.Invoke(value);
        lastSender = sender.ToString(); // 记录最后一个发送者
    }
}