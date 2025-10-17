using System.Collections.Generic;
using System;

public class BulletCountToScatterBoosterEffect : EffectResolver, IEffect<Bullet>
{
    public BulletCountToScatterBoosterEffect(Type target, List<Type> dependentEffects = null) : base(target, dependentEffects)
    {
    }

    public BulletCountToScatterBoosterEffect(int maxLevel = 3, List<Type> dependentEffects = null, Timer timer = null, Type targetType = null) : base(maxLevel, dependentEffects, timer, targetType)
    {
        if (timer != null)
            this.timer.onEnd += Cancel;
    }

    public int EffectLevel { get; set; }

    public event Action<IEffect<Bullet>> onComplete;

    public void Apply(Bullet target)
    {
        target.updatedData.shotsToReflectCount = target.baseData.shotsToReflectCount + EffectLevel;
    }

    public void Cancel(Bullet target)
    {
        throw new NotImplementedException();
    }

    public void Cancel()
    {
        onComplete?.Invoke(this);
        onComplete = null;
        EffectLevel = 0;
    }

    public IEffect<Bullet> CreateInstance()
    {
        return new BulletCountToScatterBoosterEffect();
    }

    public override string ToString()
    {
        return "Scatter Count";
    }
}

