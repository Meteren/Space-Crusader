using System;

public class IncreaseFireRateEffect : EffectResolver, IEffect<Bullet>
{
    float fireRateIncreaseAmount;

    public int EffectLevel { get; set; }

    private Bullet targetReference;

    public IncreaseFireRateEffect(Type target) : base(target) { }
    public IncreaseFireRateEffect(float time, float fireRateIncreaseAmount = 0.2f, Type targetType = null) : base(time,targetType:targetType)
    {
        this.fireRateIncreaseAmount = fireRateIncreaseAmount;
        countDown.onEnd += Cancel;
        countDown.StartTimer();

    }
      
    public event Action<IEffect<Bullet>> onComplete;

    public void Apply(Bullet target)
    {
        targetReference = target;
        target.dataReference.generationTime = target.baseData.generationTime - (fireRateIncreaseAmount * EffectLevel);
    }

    public void Cancel(Bullet target)
    {
        if(targetReference != null)
            target.dataReference.generationTime = target.baseData.generationTime;
    }

    public void Cancel()
    {
        onComplete?.Invoke(this);
        Cancel(targetReference);
        onComplete = null;
        targetReference = null;
        EffectLevel = 0;
    }

    public IEffect<Bullet> CreateInstance()
    {
        return new IncreaseFireRateEffect(30);
    }

    public override string ToString()
    {
        return "Fire Rate";
    }
}