using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

public class ParticleSpawner : MonoBehaviour
{

    ParticleFactory particleFactory;

    public ParticleFactory ParticleFactory {  get { return particleFactory; } }


    private void Awake()
    {
        particleFactory = new ParticleFactory(transform);
    }

    public ParticleSystem Spawnparticle(ParticleSystem prefab,Vector2 position)
        => particleFactory.Create(prefab,position);

}
