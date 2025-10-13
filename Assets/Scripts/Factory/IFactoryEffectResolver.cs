using System.Collections.Generic;

public interface IFactoryEffectResolver<T>
{
    List<IEffect<T>> Effects { get; set; }
    void SetEffects(List<IEffect<T>> effects);
}


