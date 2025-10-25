using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface IFactory<T,TConfig> where T : Component
{
    T Create(TConfig prefab, Vector2 position);


}

public interface IFactory<T> where T : Component
{
    T Create(T prefab, Vector2 position);
}


