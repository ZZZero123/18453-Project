using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTask 
{
    /// <summary>
    /// </summary>
    public float delayedTime;
    /// <summary>
    /// </summary>
    public float timer;
    /// <summary>
    /// </summary>
    public int repeatTimes;
    /// <summary>
    /// </summary>
    public int restRepeatTimes;
    /// <summary>
    /// </summary>
    public bool isPause;
    /// <summary>
    /// </summary>
    public Action callback;

    public TimerTask(float delayedTime, int repeatTimes, Action callback)
    {
        this.delayedTime = delayedTime;
        timer = 0;
        this.repeatTimes = repeatTimes;
        this.isPause = false;
        this.callback = callback;
        if(repeatTimes == -1)
        {
            restRepeatTimes = -1;
        }
        else
        {
            restRepeatTimes = repeatTimes;
        }
    }

}
