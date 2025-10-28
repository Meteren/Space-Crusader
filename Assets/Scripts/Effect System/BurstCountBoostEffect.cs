using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;

public class BurstCountBoostEffect : EffectResolver, IEffect<Bullet>, IDataProvider<BulletData>
{
    int burstCountBoost;
    public BurstCountBoostEffect(Type target, List<Type> dependentEffects = null) : base(target, dependentEffects)
    {
    }

    public BurstCountBoostEffect(Timer timer = null, List<Type> dependentEffects = null, int burstCountBoost = 0,int maxLevel = 3, Type targetType = null) : base(maxLevel, dependentEffects, timer, targetType)
    {
        this.burstCountBoost = burstCountBoost;
        if(this.timer != null)
            this.timer.onEnd += Cancel;
    }

    public int EffectLevel { get; set; }
    public BulletData ProvidedData { get; set; }

    public event Action<IEffect<Bullet>> onComplete;

    public void Apply(Bullet target)
    {
        target.updatedData.burstCount = target.baseData.burstCount + EffectLevel + burstCountBoost; 
    }

    public void Cancel(Bullet target)
    {
        //NOOP
    }

    public void Cancel()
    {
        foreach (var effect in ProvidedData.effects.ToList())
        {
            EffectResolver resolver = effect as EffectResolver;
            if (resolver.dependentEffects != null)
            {
                foreach (var dependentType in resolver.dependentEffects)
                {
                    if (dependentType == EffectType)
                        effect.Cancel();
                }
            }


        }
        onComplete?.Invoke(this);
                                
        onComplete = null;
        EffectLevel = 0;
    }

    public IEffect<Bullet> CreateInstance()
    {
        return new BurstCountBoostEffect(/*new CountDown(15f,EffectType.Name)*/);
    }

    public void Provide(BulletData providedData)
    {
       
        ProvidedData = providedData;

    }

    public override string ToString()
    {
        return "Burst Count Boost";
    }
}
