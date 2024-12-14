using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// </summary>
public class MonoHandler : MonoBehaviour
{
    private event UnityAction UpdateEvent;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        UpdateEvent?.Invoke();
    }

    public static MonoHandler operator+ (MonoHandler mono, UnityAction action)
    {
        mono.AddUpdateEvent(action);
        return mono;
    }

    public static MonoHandler operator- (MonoHandler mono, UnityAction action)
    {
        mono.RemoveUpdateEvent(action);
        return mono;
    }

    /// <summary>
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateEvent(UnityAction action) => UpdateEvent += action;

    /// <summary>
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateEvent(UnityAction action) => UpdateEvent -= action;
}