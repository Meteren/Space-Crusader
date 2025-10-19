using System.Collections.Generic;
using System;

using Random = UnityEngine.Random;

public abstract class EffectResolver : IComparable<EffectResolver>
{
    protected Timer timer;
    public Timer Timer { get { return timer; } }

    public Type EffectType => GetType();

    public Type TargetType { get; set; }

    public List<Type> dependentEffects;

    protected int maxLevel;

    public int MaxLevel {  get { return maxLevel; } }

    public EffectResolver() { }

    public EffectResolver(Type target,List<Type> dependentEffects) 
    {
        TargetType = target;
        this.dependentEffects = dependentEffects;
    }
    public EffectResolver(int maxLevel,List<Type> dependentEffects,Timer timer = null,Type targetType = null) : this(targetType,dependentEffects)
    {
        this.dependentEffects = dependentEffects;
        this.timer = timer;
        this.maxLevel = maxLevel;
        if (this.timer != null)
            this.timer.StartTimer();

    }

    public void SetTargetType(Type targetType) => TargetType = targetType;
    public void SetDependents(List<Type> dependentEffects) => this.dependentEffects = dependentEffects;

    public virtual int CalcProbability()
    {
        return Random.Range(0,10);
    }

    public int CompareTo(EffectResolver other)
    {
        int generatedNumber = CalcProbability();

        if (generatedNumber > other.CalcProbability())
            return 1;
        else if (generatedNumber < other.CalcProbability())
            return -1;
        else return 0;
    }


}
