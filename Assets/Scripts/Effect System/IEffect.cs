using System;

public interface IEffect<TTarget>
{
    int EffectLevel { get; set; }
    void Apply(TTarget target);
    void Cancel(TTarget target);
    void Cancel();

    event Action<IEffect<TTarget>> onComplete;

    IEffect<TTarget> CreateInstance();

}
