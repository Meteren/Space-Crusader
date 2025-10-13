using System;
using UnityEngine;

public class SpeedUpEffect : EffectManager, IEffect<Bullet>
{
    public int EffectLevel { get; set; } //increase logic, can be changed

    public int speedAddValue;

    public event Action<IEffect<Bullet>> onComplete;

    public SpeedUpEffect(float time, int speedAddValue) : base(time)
    {
        this.speedAddValue = speedAddValue;
        countDown.StartTimer();
        countDown.onEnd += Cancel;
    }

    public Type Type { get => GetType(); }

    public void Apply(Bullet target)
    {
        target.updatedData.speed = target.baseData.speed + new Vector2(0, speedAddValue * EffectLevel);
    }

    public void Cancel(Bullet target)
    {
        //NO-OP
    }

    public void Cancel()
    {
        onComplete?.Invoke(this);
    }
}
