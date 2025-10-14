using System;

using Random = UnityEngine.Random;

public abstract class EffectResolver : IComparable<EffectResolver>
{
    protected CountDown countDown;
    public CountDown CountDown { get { return countDown; } }

    public Type Type => GetType();

    protected int maxLevel;

    public int MaxLevel {  get { return maxLevel; } }

    public EffectResolver() { }
    public EffectResolver(float time, int maxLevel = 3)
    {
        countDown = new CountDown(time, ToString());
        this.maxLevel = maxLevel;
    }

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
