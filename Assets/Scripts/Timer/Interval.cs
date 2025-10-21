using System;
using UnityEngine;

public class Interval : Timer
{
    private float valueAfterInterval;

    private float intervalTick;

    public float IntervalTick { get => intervalTick; }

    public event Action onInterval;

    bool startedAtZero;
    bool runOnFirstTick;

    public Interval(float intervalTick, float startPoint, bool runOnFirstTick, string source) : base(startPoint, source)
    {
        this.startPoint = startPoint;
        this.intervalTick = intervalTick;
        this.runOnFirstTick = runOnFirstTick;

        if (runOnFirstTick)
            valueAfterInterval = startPoint;
        else
            valueAfterInterval = startPoint - intervalTick;

        startedAtZero = Mathf.Approximately(startPoint, 0);
    }

    public override void OnTick()
    {
        if (startedAtZero)
            return;

        current -= Time.deltaTime;
        if (current <= valueAfterInterval )
        {
            valueAfterInterval -= intervalTick;

            onInterval?.Invoke();
            Debug.Log($"On Interval for {source}");
        }
    }

    public override void Reset()
    {
        base.Reset();
        valueAfterInterval = startPoint;
        startedAtZero = Mathf.Approximately(startPoint, 0);
    }

    public override void CleanAfterCompletion()
    {
        base.CleanAfterCompletion();
        onInterval = null;
    }
}
