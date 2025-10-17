using System.Collections.Generic;
using System;
using UnityEngine;

public class SpeedUpEffect : EffectResolver, IEffect<Bullet>
{
    public int EffectLevel { get; set; } //increase logic, can be changed

    public int speedAddValue;

    public event Action<IEffect<Bullet>> onComplete;
    public SpeedUpEffect(Type type,List<Type> dependentEffects = null) : base(type, dependentEffects) { }
    public SpeedUpEffect(List<Type> dependentEffects = null,Timer timer = null, int speedAddValue = 1,int maxLevel = 3,Type targetType = null) : base(maxLevel,dependentEffects, timer,targetType:targetType)
    {
        this.speedAddValue = speedAddValue;
        if(timer != null)
            this.timer.onEnd += Cancel;
    }

    public void Apply(Bullet target)
    {
        target.updatedData.speed = target.baseData.speed + 
            new Vector2(target.baseData.speed.x > 0 ? speedAddValue * EffectLevel : 0, target.baseData.speed.y > 0 ? speedAddValue * EffectLevel : 0);
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
        return new SpeedUpEffect();   
    }

    public override string ToString()
    {
        return $"{TargetType.Name} Speed Up";
    }
}
