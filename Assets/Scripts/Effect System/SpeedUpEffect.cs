using System;
using UnityEngine;

public class SpeedUpEffect : EffectResolver, IEffect<Bullet>
{
    public int EffectLevel { get; set; } //increase logic, can be changed

    public int speedAddValue;

    public event Action<IEffect<Bullet>> onComplete;
    public SpeedUpEffect(Type type) : base(type) { }
    public SpeedUpEffect(float time, int speedAddValue = 1,Type targetType = null) : base(time,targetType:targetType)
    {
        this.speedAddValue = speedAddValue;
        countDown.StartTimer();
        countDown.onEnd += Cancel;
    }

    public void Apply(Bullet target)
    {
        target.updatedData.speed = target.baseData.speed + new Vector2(0, speedAddValue * EffectLevel);
        target.SetSpeed();
    }

    public void Cancel(Bullet target)
    {
        //NO-OP
    }

    public void Cancel()
    {
        onComplete?.Invoke(this);
        onComplete = null;
        EffectLevel = 0;
    }

    public IEffect<Bullet> CreateInstance()
    {
        return new SpeedUpEffect(60,3);   
    }

    public override string ToString()
    {
        return "Speed Up";
    }
}
