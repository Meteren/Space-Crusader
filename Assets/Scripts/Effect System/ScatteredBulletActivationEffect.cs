using System;
using System.Collections.Generic;

public class ScatteredBulletActivationEffect : BulletActivationEffect
{
    public ScatteredBulletActivationEffect(Type target,List<Type> dependentEffects = null) : base(target,dependentEffects)
    {
    }

    public ScatteredBulletActivationEffect(List<Type> dependentEffects = null,Timer timer = null, int maxLevel = 3, Type type = null) : base(type,dependentEffects,timer, maxLevel)
    {
    }

    public override IEffect<Bullet> CreateInstance()
    {
        return new ScatteredBulletActivationEffect();
    }

    public override string ToString()
    {
        return "Scattered Bullet";
    }
}

