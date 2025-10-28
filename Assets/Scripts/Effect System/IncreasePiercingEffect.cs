using System.Collections.Generic;
using System;

public class IncreasePiercingEffect : EffectResolver, IEffect<Bullet>
{
    int increasePiercingAmount;
    public int EffectLevel { get; set; }
    public IncreasePiercingEffect(Type target,List<Type> dependentEffects = null) : base(target, dependentEffects)
    {
    }
    public IncreasePiercingEffect(List<Type> dependentEffects = null,Timer timer = null, int increasePiercingAmount = 1, int maxLevel = 3,Type targetType = null) : base(maxLevel,dependentEffects,timer:timer, targetType:targetType)
    {

        this.increasePiercingAmount = increasePiercingAmount;
        if (this.timer != null)
            this.timer.onEnd += Cancel;
      
    }

    public event Action<IEffect<Bullet>> onComplete;

    public void Apply(Bullet target)
    {
        target.updatedData.pierceCount = target.baseData.pierceCount + increasePiercingAmount * EffectLevel;
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
        return new IncreasePiercingEffect();
    }

    public override string ToString()
    {
        return $"{TargetType.Name} Piercing";
    }
}
