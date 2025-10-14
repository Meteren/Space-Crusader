using System;
using System.Diagnostics;

public class IncreasePiercingEffect : EffectResolver, IEffect<Bullet>
{
    int increasePiercingAmount;
    public int EffectLevel { get; set; }

    public IncreasePiercingEffect() { }
    public IncreasePiercingEffect(float time, int increasePiercingAmount = 1) : base(time)
    {
        this.increasePiercingAmount = increasePiercingAmount;
        countDown.StartTimer();
        countDown.onEnd += Cancel;
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
        UnityEngine.Debug.Log("Canceled");
        onComplete = null;
    }

    public IEffect<Bullet> CreateInstance()
    {
        return new IncreasePiercingEffect(10);
    }

    public override string ToString()
    {
        return "Piercing";
    }
}
