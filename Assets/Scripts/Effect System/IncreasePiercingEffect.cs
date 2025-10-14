using System;

public class IncreasePiercingEffect : EffectResolver, IEffect<Bullet>
{
    int increasePiercingAmount;
    public int EffectLevel { get; set; }
    public IncreasePiercingEffect(Type target) : base(target)
    {
    }
    public IncreasePiercingEffect(float time, int increasePiercingAmount = 1,Type targetType = null) : base(time,targetType:targetType)
    {
        this.increasePiercingAmount = increasePiercingAmount;
        countDown.onEnd += Cancel;
        countDown.StartTimer();

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
        EffectLevel = 0;
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
