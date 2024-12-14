using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// </summary>
public class MonoManager : SingletonBase<MonoManager>
{
    private MonoHandler mono;
    public MonoManager()
    {
        mono = new GameObject("MonoHolder").AddComponent<MonoHandler>();
    }
    /// <summary>
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateAction(UnityAction action)
    {
         mono?.AddUpdateEvent(action);
    }

    /// <summary>
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateAction(UnityAction action)
    {
          mono?.RemoveUpdateEvent(action);
    }

    /// <summary>
    /// </summary>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(string methodName)
    {        
          return mono?.StartCoroutine(methodName);
    }

    /// <summary>
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(IEnumerator routine)
    {
         return mono?.StartCoroutine(routine);
    }

    /// <summary>
    /// </summary>
    /// <param name="routine"></param>
    public void StopCoroutine(IEnumerator routine)
    {
          mono?.StopCoroutine(routine);
    }

   
}