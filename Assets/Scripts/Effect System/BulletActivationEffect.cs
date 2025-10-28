using System.Collections.Generic;
using System;
using System.Linq;

public abstract class BulletActivationEffect : EffectResolver, IEffect<Bullet>, IResolveAsAbility<BulletSpawner>
{
    BulletSpawner bSpawnerReference;
    public BulletActivationEffect(Type target,List<Type> dependentEffects) : base(target, dependentEffects)
    {
    }
    public BulletActivationEffect(Type type, List<Type> dependentEffects,Timer timer = null, int maxLevel = 3) : base(maxLevel,dependentEffects,timer,targetType:type)
    {
        if(this.timer!=null)
            this.timer.onEnd += Cancel;
    }

    public int EffectLevel { get; set; }

    public event Action<IEffect<Bullet>> onComplete;

    public void Apply(Bullet target)
    {
        //NOOP   
    }

    public void Cancel(Bullet target)
    {
        //NOOP
    }

    public void Cancel()
    {
        onComplete?.Invoke(this);
        BulletData dataToBeCleaned = bSpawnerReference.GetDataToBeEffected(TargetType);

        foreach(var effect in dataToBeCleaned.effects.ToList())
            effect.Cancel();

        dataToBeCleaned.effects.Clear();
        bSpawnerReference.RemoveBulletFromSpawner(TargetType);
        onComplete = null;
        EffectLevel = 0;//no need but can stay     
    }

    public virtual IEffect<Bullet> CreateInstance()
    {
        return null;
    }


    public void SendData(BulletSpawner dataToSend)
    {
        bSpawnerReference = dataToSend;
        //UnityEngine.Debug.Log(TargetType);
        bSpawnerReference.AddBulletToSpawner(TargetType);
    }

}

