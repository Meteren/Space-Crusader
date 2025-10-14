using System;

using Random = UnityEngine.Random;

public abstract class EffectResolver : IComparable<EffectResolver>
{
    protected CountDown countDown;
    public CountDown CountDown { get { return countDown; } }

    public Type EffectType => GetType();

    public Type TargetType { get; set; }

    protected int maxLevel;

    public int MaxLevel {  get { return maxLevel; } }

    public EffectResolver(Type target) 
    {
        TargetType = target;
    }
    public EffectResolver(float time,int maxLevel = 3, Type targetType = null) : this(targetType)
    {
        countDown = new CountDown(time, GetType().Name);
        this.maxLevel = maxLevel;
    }

    public void SetTargetType(Type targetType) => TargetType = targetType;

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
