using System.Collections.Generic;
using System;

public class BoostDamageAmountEffect : EffectResolver, IEffect<Bullet>
{
    int boostAmount;

    public BoostDamageAmountEffect(Type target, List<Type> dependentEffects = null) : base(target, dependentEffects)
    {
    }

    public BoostDamageAmountEffect(int boostAmount = 2,int maxLevel = 3, List<Type> dependentEffects = null, Timer timer = null, Type targetType = null) : base(maxLevel, dependentEffects, timer, targetType)
    {
        this.boostAmount = boostAmount;
        if (timer != null)
            this.timer.onEnd += Cancel;
    }

    public int EffectLevel { get; set; }

    public event Action<IEffect<Bullet>> onComplete;

    public void Apply(Bullet target)
    {
        target.updatedData.damageAmount = target.baseData.damageAmount + EffectLevel * boostAmount;
    }

    public void Cancel(Bullet target)
    {
        //noop
    }

    public void Cancel()
    {
        onComplete?.Invoke(this);
        onComplete = null;
        EffectLevel = 0;
    }

    public IEffect<Bullet> CreateInstance()
    {
        return new BoostDamageAmountEffect();
    }

    public override string ToString()
    {
        return $"{TargetType.Name} Damage Boost";
    }
}
