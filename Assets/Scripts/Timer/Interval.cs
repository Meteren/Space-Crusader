using System;
using UnityEngine;

public class Interval : Timer
{
    private float valueAfterInterval;

    private float intervalTick;

    public event Action onInterval;

    public Interval(float intervalTick, float startPoint) : base(startPoint)
    {
        this.intervalTick = intervalTick;
        valueAfterInterval = startPoint;
    }

    public override void OnTick()
    {
        current -= Time.deltaTime;  
        if(current <= valueAfterInterval)
        {
            valueAfterInterval -= intervalTick;

            onInterval?.Invoke();
        }        
    }

    public override void Reset()
    {
        base.Reset();
        valueAfterInterval = startPoint;
    }
}
