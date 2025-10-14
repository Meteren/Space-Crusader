using System;

public class IncreaseFireRateEffect : EffectResolver, IEffect<Bullet>
{
    float fireRateIncreaseAmount;

    public int EffectLevel { get; set; }

    public IncreaseFireRateEffect() { }
    public IncreaseFireRateEffect(float time, float fireRateIncreaseAmount = 0) : base(time)
    {
        this.fireRateIncreaseAmount = fireRateIncreaseAmount;
        countDown.StartTimer();
        countDown.onEnd += Cancel;
    }
      
    public event Action<IEffect<Bullet>> onComplete;

    public void Apply(Bullet target)
    {
        target.updatedData.generationTime = target.baseData.generationTime - (fireRateIncreaseAmount + EffectLevel);
    }

    public void Cancel(Bullet target)
    {
        //NO-OP
    }

    public void Cancel()
    {
        onComplete?.Invoke(this);
        onComplete = null;
    }

    public IEffect<Bullet> CreateInstance()
    {
        return new IncreaseFireRateEffect(5);
    }

    public override string ToString()
    {
        return "Fire Rate";
    }
}