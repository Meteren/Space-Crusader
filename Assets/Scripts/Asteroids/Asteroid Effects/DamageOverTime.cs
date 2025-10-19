
using System;

public class DamageOverTime<TSource> : EffectResolver, IEffect<IDamageable<TSource>>
{
    IDamageable<TSource> targetReference;
    TSource sourceReference;
    float timerStart;
    float intervalTick;

    public DamageOverTime() { }
    public DamageOverTime(TSource sourceReference,float timerStart, float intervalTick)
    {
        this.sourceReference = sourceReference;
        this.timerStart = timerStart;
        this.intervalTick = intervalTick;   
    }

    public int EffectLevel { get; set; }

    public event Action<IEffect<IDamageable<TSource>>> onComplete;

    public void Apply(IDamageable<TSource> target)
    {
        UnityEngine.Debug.Log("Applying damage over time effect");
        targetReference = target;
        timer = new Interval(intervalTick,timerStart ,GetType().Name);

        Interval interval = (Interval)timer;
        interval.onEnd += Cancel;
        interval.onInterval += OnInterval;
        timer.StartTimer();
   
    }

    public void Cancel(IDamageable<TSource> target)
    {
        
        //NOOP
    }

    public void Cancel()
    {
        onComplete?.Invoke(this);
        timer.StopTimer();
        timer = null;
        targetReference = null;
        sourceReference = default;
    }

    public IEffect<IDamageable<TSource>> CreateInstance()
    {
        return null;
    }

    private void OnInterval()
    {
        UnityEngine.Debug.Log("Inflict damage");
        targetReference.OnDamage(sourceReference);
    }


}