using System;

public class ExplosiveBulletActivationEffect : EffectResolver, IEffect<Bullet>, IResolveAsAbility<BulletSpawner>
{
    BulletSpawner bSpawnerReference;
    Bullet targetReference;
    public ExplosiveBulletActivationEffect(Type target) : base(target)
    {
    }

    public ExplosiveBulletActivationEffect(float time, int maxLevel = 3, Type type = null) : base(time,targetType:type)
    {
        countDown.onEnd += Cancel;
        countDown.StartTimer();
    }

    public int EffectLevel { get; set; }

    public event Action<IEffect<Bullet>> onComplete;

    public void Apply(Bullet target)
    {
        targetReference = target;//will be deleted prob
        
    }

    public void Cancel(Bullet target)
    {
        //NO-OP
    }

    public void Cancel()
    {
        onComplete?.Invoke(this);
        BulletData dataToBeCleaned = bSpawnerReference.GetDataToBeEffected(TargetType);

        foreach(var effect in dataToBeCleaned.effects)
            effect.Cancel();

        dataToBeCleaned.effects.Clear();
        bSpawnerReference.RemoveBulletFromSpawner(TargetType);
        onComplete = null;
        EffectLevel = 0;//no need but can stay     
    }

    public IEffect<Bullet> CreateInstance()
    {
        return new ExplosiveBulletActivationEffect(200f);
    }

    public void SendData(BulletSpawner dataToSend)
    {
        bSpawnerReference = dataToSend;
        bSpawnerReference.AddBulletToSpawner(TargetType);
    }

    public override string ToString()
    {
        return "Explosive Bullet";
    }
}
