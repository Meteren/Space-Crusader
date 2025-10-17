using System.Collections.Generic;
using System;

public class ExplosiveBulletActivationEffect : BulletActivationEffect
{
    public ExplosiveBulletActivationEffect(Type target,List<Type> dependentEffects = null) : base(target,dependentEffects)
    {
    }

    public ExplosiveBulletActivationEffect(Timer timer = null,List <Type> dependentEffects = null, int maxLevel = 3, Type type = null) 
        : base(type,dependentEffects,timer, maxLevel)
    {

    }

    public override IEffect<Bullet> CreateInstance()
    {
        return new ExplosiveBulletActivationEffect();//can be changed to make it time based in the code
    }

    public override string ToString()
    {
        return "Explosive Bullet";
    }
}


