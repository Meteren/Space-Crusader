using System;

public class PiercingEffect : EffectManager, IEffect<Bullet>
{
    public PiercingEffect(float time) : base(time)
    {
    }

    public int EffectLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public Type Type => throw new NotImplementedException();

    public event Action<IEffect<Bullet>> onComplete = (bullet) => throw new NotImplementedException();

    public void Apply(Bullet target)
    {
        throw new NotImplementedException();
    }

    public void Cancel(Bullet target)
    {
        //NO-OP
    }

    public void Cancel()
    {
        throw new NotImplementedException();
    }
}
