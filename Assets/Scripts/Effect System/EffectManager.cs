using System;

using Random = UnityEngine.Random;

public abstract class EffectManager : IComparable<EffectManager>
{
    protected CountDown countDown;
    public CountDown CountDown { get { return countDown; } }

    public EffectManager(float time)
    {
        countDown = new CountDown(time,ToString());
    }

    public virtual int CalcProbability()
    {
        return Random.Range(0,10);
    }

    public int CompareTo(EffectManager other)
    {
        int generatedNumber = Random.Range(0, 10);

        if (generatedNumber > other.CalcProbability())
            return 1;
        else if (generatedNumber < other.CalcProbability())
            return -1;
        else return 0;
    }

    public override string ToString() => GetType().Name;

}
