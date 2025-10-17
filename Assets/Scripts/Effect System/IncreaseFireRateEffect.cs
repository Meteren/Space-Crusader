
using System;
using System.Collections.Generic;

public class IncreaseFireRateEffect : EffectResolver, IEffect<Bullet>
{
    float fireRateIncreaseAmount;

    public int EffectLevel { get; set; }

    private Bullet targetReference;

    public IncreaseFireRateEffect(Type target,List<Type> dependentEffects = null) : base(target,dependentEffects) { }
    public IncreaseFireRateEffect(List<Type> dependentEffects = null,Timer timer = null, float fireRateIncreaseAmount = 0.2f,int maxLevel = 3, Type targetType = null) : base(maxLevel, dependentEffects, timer:timer, targetType:targetType)
    {
        this.fireRateIncreaseAmount = fireRateIncreaseAmount;
        if(this.timer != null)
            this.timer.onEnd += Cancel;

    }
      
    public event Action<IEffect<Bullet>> onComplete;

    public void Apply(Bullet target)
    {
        targetReference = target;
        target.dataReference.generationTime = target.baseData.defaultGenTime - (fireRateIncreaseAmount * EffectLevel);
        target.dataReference.countDown = target.dataReference.generationTime;
    }

    public void Cancel(Bullet target)
    {
        if(targetReference != null)
            target.dataReference.generationTime = target.baseData.generationTime;
    }

    public void Cancel()
    {
        onComplete?.Invoke(this);
        Cancel(targetReference);
        onComplete = null;
        targetReference = null;
        EffectLevel = 0;
    }

    public IEffect<Bullet> CreateInstance()
    {
        return new IncreaseFireRateEffect();
    }

    public override string ToString()
    {
        return $"{TargetType.Name} Fire Rate";
    }
}