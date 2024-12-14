using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// </summary>
public class EventCenter : SingletonBase<EventCenter>
{
    private Dictionary<EventName, List<Delegate>> eventDic = new Dictionary<EventName, List<Delegate>>();

    /// <summary>
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void AddListenerBase(EventName eventName, Delegate callback)
    {
        if (eventDic.ContainsKey(eventName))
        {
            eventDic[eventName].Add(callback);
        }
        else
        {
            eventDic.Add(eventName, new List<Delegate>() { callback});
        }
    }
    /// <summary>
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void AddListener(EventName eventName, Action callback)
    {
        AddListenerBase(eventName, callback);
    }
    /// <summary>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddListener<T>(EventName eventName, Action<T> callback)
    {
        AddListenerBase(eventName, callback);
    }
    /// <summary>
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void AddListener<T1, T2>(EventName eventName, Action<T1, T2> callback)
    {
        AddListenerBase(eventName, callback);
    }
    /// <summary>
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void AddListener<T1, T2, T3>(EventName eventName, Action<T1, T2, T3> callback)
    {
        AddListenerBase(eventName, callback);
    }
    /// <summary>
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void AddListener<T1, T2, T3, T4>(EventName eventName, Action<T1, T2, T3, T4> callback)
    {
        AddListenerBase(eventName, callback);
    }
    public void AddListenerWithReturnValue<R>(EventName eventName, Func<R> callback)
    {
        AddListenerBase(eventName, callback);
    }
    public void AddListenerWithReturnValue<T1, R>(EventName eventName, Func<T1, R> callback)
    {
        AddListenerBase(eventName, callback);
    }
    public void AddListenerWithReturnValue<T1,T2, R>(EventName eventName, Func<T1,T2, R> callback)
    {
        AddListenerBase(eventName, callback);
    }
    public void AddListenerWithReturnValue<T1, T2, T3, R>(EventName eventName, Func<T1, T2, T3, R> callback)
    {
        AddListenerBase(eventName, callback);
    }
    public void AddListenerWithReturnValue<T1, T2, T3, T4, R>(EventName eventName, Func<T1, T2, T3, T4, R> callback)
    {
        AddListenerBase(eventName, callback);
    }
    /// <summary>
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void RemoveListenerBase(EventName eventName, Delegate callback)
    {
        if (eventDic.TryGetValue(eventName, out List<Delegate> eventList))
        {
            eventList.Remove(callback);
            if (eventList.Count == 0)
            {
                eventDic.Remove(eventName);
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void RemoveListener(EventName eventName, Action callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    /// <summary>
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void RemoveListener<T>(EventName eventName, Action<T> callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    /// <summary>
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void RemoveListener<T1, T2>(EventName eventName, Action<T1, T2> callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    /// <summary>
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void RemoveListener<T1, T2, T3>(EventName eventName, Action<T1, T2, T3> callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    /// <summary>
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void RemoveListener<T1, T2, T3, T4>(EventName eventName, Action<T1, T2, T3, T4> callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    public void RemoveListenerWithReturnValue<R>(EventName eventName, Func<R> callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    public void RemoveListenerWithReturnValue<T1, R>(EventName eventName, Func<T1, R> callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    public void RemoveListenerWithReturnValue<T1, T2, R>(EventName eventName, Func<T1, T2, R> callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    public void RemoveListenerWithReturnValue<T1, T2, T3, R>(EventName eventName, Func<T1, T2, T3, R> callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    public void RemoveListenerWithReturnValue<T1, T2, T3, T4, R>(EventName eventName, Func<T1, T2, T3, T4, R> callback)
    {
        RemoveListenerBase(eventName, callback);
    }
    /// <summary>
    /// </summary>
    /// <param name="eventName"></param>
    public void TriggerEvent(EventName eventName)
    {
        if (eventDic.ContainsKey(eventName))
        {
            foreach (Delegate callback in eventDic[eventName])
            {
                (callback as Action)?.Invoke();
            }
        }
    }
    /// <summary>
    /// </summary>
    /// <param name="eventName"></param>
    public void TriggerEvent<T>(EventName eventName, T info)
    {
        if (eventDic.ContainsKey(eventName))
        {            
            foreach (Delegate callback in eventDic[eventName])
            {
                (callback as Action<T>)?.Invoke(info); 
            }
        }
    }
    /// <summary>
    /// </summary>
    /// <param name="eventName"></param>
    public void TriggerEvent<T1, T2>(EventName eventName, T1 info1, T2 info2)
    {
        if (eventDic.ContainsKey(eventName))
        {
            foreach (Delegate callback in eventDic[eventName])
            {
                (callback as Action<T1, T2>)?.Invoke(info1, info2);
            }
        }
    }
    /// <summary>
    /// </summary>
    /// <param name="eventName"></param>
    public void TriggerEvent<T1, T2, T3>(EventName eventName, T1 info1, T2 info2, T3 info3)
    {
        if (eventDic.ContainsKey(eventName))
        {
            foreach (Delegate callback in eventDic[eventName])
            {
                (callback as Action<T1, T2, T3>)?.Invoke(info1, info2, info3);
            }
        }
    }
    /// <summary>
    /// </summary>
    /// <param name="eventName"></param>
    public void TriggerEvent<T1, T2, T3, T4>(EventName eventName, T1 info1, T2 info2, T3 info3, T4 info4)
    {
        if (eventDic.ContainsKey(eventName))
        {
            foreach (Delegate callback in eventDic[eventName])
            {
                (callback as Action<T1, T2, T3, T4>)?.Invoke(info1, info2, info3, info4);
            }
        }
    }
    public R TriggerEventWithReturnValue<R>(EventName eventName)
    {
        R value = default(R);
        if (eventDic.ContainsKey(eventName))
        {
            foreach (Delegate callback in eventDic[eventName])
            {
                if (callback is Func<R> funcCallback)
                {
                    value = funcCallback.Invoke();
                }
            }
            return value;
        }
        return value;
    }
    public R TriggerEventWithReturnValue<T1, R>(EventName eventName, T1 info1)
    {
        R value = default(R);
        if (eventDic.ContainsKey(eventName))
        {
            foreach (Delegate callback in eventDic[eventName])
            {
                if (callback is Func<T1, R> funcCallback)
                {
                    value = funcCallback.Invoke(info1);
                }
            }
            return value;
        }
        return value;
    }
    public R TriggerEventWithReturnValue<T1, T2, R>(EventName eventName, T1 info1, T2 info2)
    {
        R value = default(R);
        if (eventDic.ContainsKey(eventName))
        {
            foreach (Delegate callback in eventDic[eventName])
            {
                if (callback is Func<T1,T2, R> funcCallback)
                {
                    value = funcCallback.Invoke(info1, info2);
                }
            }
            return value;
        }
        return value;
    }
    public R TriggerEventWithReturnValue<T1, T2, T3, R>(EventName eventName, T1 info1, T2 info2, T3 info3)
    {
        R value = default(R);
        if (eventDic.ContainsKey(eventName))
        {
            foreach (Delegate callback in eventDic[eventName])
            {
                if (callback is Func<T1, T2, T3, R> funcCallback)
                {
                    value = funcCallback.Invoke(info1, info2, info3);
                }
            }
            return value;
        }
        return value;
    }
    public R TriggerEventWithReturnValue<T1, T2, T3, T4, R>(EventName eventName, T1 info1, T2 info2, T3 info3, T4 info4)
    {
        R value = default(R);
        if (eventDic.ContainsKey(eventName))
        {
            foreach (Delegate callback in eventDic[eventName])
            {
                if (callback is Func<T1, T2, T3, T4, R> funcCallback)
                {
                    value = funcCallback.Invoke(info1, info2, info3, info4);
                }
            }
            return value;
        }
        return value;
    }
    /// <summary>
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}
