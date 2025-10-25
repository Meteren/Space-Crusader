using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ParticleFactory : IFactory<ParticleSystem>
{
    Dictionary<ParticleSystem, IObjectPool<ParticleSystem>> particlePools = new();
    Transform parent;
    public ParticleFactory(Transform parent = null)
    {
        this.parent = parent;
    }
    public ParticleSystem Create(ParticleSystem prefab, Vector2 position)
    {
        var particlePool = particlePools.TryGetValue(prefab,out IObjectPool<ParticleSystem> pool) 
            ? pool
            : particlePools[prefab] = new ObjectPool<ParticleSystem>(
                () => GameObject.Instantiate(prefab).GetComponent<ParticleSystem>(),
                p => p.gameObject.SetActive(true),
                p => p.gameObject.SetActive(false),
                p => GameObject.Destroy(p.gameObject),
                true,
                10,
                100);
        ParticleSystem particle = particlePool.Get();

        particle.transform.SetParent(parent);

        ParticleHandler pHandler = particle.gameObject.GetComponent<ParticleHandler>();

        pHandler.Init(particlePool, position, particle);

        return particle;

    }
}
