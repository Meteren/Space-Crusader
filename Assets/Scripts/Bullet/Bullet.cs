
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class Bullet : MonoBehaviour
{
    public BulletData.DataFields updatedData;

    public BulletData.DataFields baseData;

    protected IObjectPool<Bullet> bulletPoolBelonged;

    protected Rigidbody2D rb;

    protected bool initFirstTimeValues = false;

    public IObjectPool<Bullet> BulletPoolBelonged { get => bulletPoolBelonged; }

    private void Start()
    {
        transform.localScale /= CameraScaler.scaleFactor;

    }

    public void Init(BulletData data, IObjectPool<Bullet> bulletPoolBelonged,PlayerController pc,Vector2 position,List<IEffect<Bullet>> effects)
    {
        if (!initFirstTimeValues)
        {
            baseData = data.CopyClassInstance();
            updatedData = baseData;
            initFirstTimeValues = true;
        }
        foreach(var effect in effects)
        {
            effect.Apply(this);
        }
        this.bulletPoolBelonged = bulletPoolBelonged;
        transform.position = position + new Vector2(0, pc.boundarySize.y / 2);
        SetSpeed();
    }
    
    private void SetSpeed()
    {
        if(!rb)
            rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = updatedData.speed;
    }

    public void Release()
    {
        updatedData = baseData;
        bulletPoolBelonged.Release(this);
    }

}
