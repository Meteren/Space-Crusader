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
        UnityEngine.Debug.Log($"Provided data: {ProvidedData}");
        foreach (var effect in ProvidedData.effects.ToList())
        {
            EffectResolver resolver = effect as EffectResolver;
            UnityEngine.Debug.Log($"Dependency effect:{resolver.EffectType}");
            if (resolver.dependentEffects != null)
            {
                UnityEngine.Debug.Log("Dependency effects not null");
                foreach (var dependentType in resolver.dependentEffects)
                {
                    UnityEngine.Debug.Log($"Dependent type:{dependentType}");
                    if (dependentType == EffectType)
                        effect.Cancel();
                }
            }
            else
            {
                
                UnityEngine.Debug.Log("Dependency effects null");
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
