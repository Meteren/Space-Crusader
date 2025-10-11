using UnityEngine;

public interface IFactory<T,TConfig> where T : Component
{
    T Create(TConfig prefab, Vector2 position);
}


