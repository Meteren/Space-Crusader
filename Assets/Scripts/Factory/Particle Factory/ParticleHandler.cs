using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ParticleHandler : MonoBehaviour
{
    ParticleSystem particle;

    IObjectPool<ParticleSystem> poolBelongTo;

    public void Init(IObjectPool<ParticleSystem> poolBelongTo,Vector2 position,ParticleSystem particle)
    {
        this.particle = particle;
        transform.position = position;
        this.poolBelongTo = poolBelongTo;
    }
    public void InitReturnToPoolProcess()
        =>
        StartCoroutine(ReturnToPoolRoutine());

    private IEnumerator ReturnToPoolRoutine()
    { 
        yield return new WaitForSeconds(particle.main.duration);
        poolBelongTo.Release(particle);
    }
}
