using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected float toNextPanelWaitTime;
    public float ToNextPanelWaitTime => toNextPanelWaitTime;
    /// <summary>
    /// </summary>
    public virtual void Show(Action onFinish = null, Action onBegin = null)
    {
        onBegin?.Invoke();
        
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            onFinish?.Invoke();
        }

    }
    /// <summary>
    /// </summary>
    public virtual void Hide(Action onFinish = null, Action onBegin = null)
    {
        onBegin?.Invoke();           
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            onFinish?.Invoke();
        }

    }
}
