using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class ParticleHandler : MonoBehaviour
{
    ParticleSystem particle;

    IObjectPool<ParticleSystem> poolBelongTo;

    Vector2 baseScale;

    bool baseScaleInittied;

    public void Init(IObjectPool<ParticleSystem> poolBelongTo,Vector2 position,ParticleSystem particle)
    {
        if (!baseScaleInittied)
        {
            baseScale = transform.localScale;
            baseScaleInittied = true;
        }
           
        this.particle = particle;
        transform.position = position;
        this.poolBelongTo = poolBelongTo;
    }
    public void InitReturnToPoolProcess()
        =>
        StartCoroutine(ReturnToPoolRoutine());

    private IEnumerator ReturnToPoolRoutine()
    {
        yield return new WaitWhile(() => particle.IsAlive(true));
        Release();

    }

    public void SetScale(float scaleFactor)
        =>
        transform.localScale *= scaleFactor;

    private void Release()
    {
        particle.Clear();
        poolBelongTo.Release(particle);
        particle.transform.localScale = baseScale;
    }

}
