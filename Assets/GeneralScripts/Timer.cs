using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float targetTime;

    public void SetTimer(float duration)
    {
        targetTime = Time.time + duration;
    }

    public bool TimeIsUp => Time.time >= targetTime;
    public float TimeLeft => targetTime - Time.time;
}
