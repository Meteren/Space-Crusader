using System.Collections.Generic;
using System;


public class TimeBetweenBurstBoosterEffect : EffectResolver, IEffect<Bullet>
{
    float timeBetweenBurstIncreaseVal;
    public TimeBetweenBurstBoosterEffect(Type target,List<Type> dependentEffects = null) : base(target,dependentEffects)
    {
    }

    public TimeBetweenBurstBoosterEffect(List<Type> dependentEffects = null,Timer timer = null,float timeBetweenBurstIncreaseVal = 0.03f, int maxLevel = 3 , Type targetType = null) : base(maxLevel,dependentEffects, timer, targetType)
    {
        this.timeBetweenBurstIncreaseVal = timeBetweenBurstIncreaseVal;
        if (this.timer != null)
            this.timer.onEnd += Cancel;
    }

    public int EffectLevel { get; set; }

    public event Action<IEffect<Bullet>> onComplete;

    public void Apply(Bullet target)
    {
        target.updatedData.timeBetweenBurstShots = target.baseData.timeBetweenBurstShots - timeBetweenBurstIncreaseVal * EffectLevel;
    }

    public void Cancel(Bullet target)
    {
        //NOOP
    }

    public void Cancel()
    {
        onComplete?.Invoke(this);
        onComplete = null;
        EffectLevel = 0;
    }

    public IEffect<Bullet> CreateInstance()
    {
        return new TimeBetweenBurstBoosterEffect();
    }

    public override string ToString()
    {
        return "Time Between Burst";
    }
}
