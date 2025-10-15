
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public BulletData.DataFields updatedData;

    public BulletData.DataFields baseData;

    [HideInInspector] public BulletData dataReference; 

    protected IObjectPool<Bullet> bulletPoolBelonged;

    protected Rigidbody2D rb;

    protected bool initFirstTimeValues = false;

    protected PlayerController playerReference;

    protected Vector2 generationPoint;

    protected List<IEffect<Bullet>> storedEffects;

    public IObjectPool<Bullet> BulletPoolBelonged { get => bulletPoolBelonged; }

    protected void Start()
    {
        transform.localScale /= CameraScaler.scaleFactor;
    }
    private void Update()
    {
        Debug.Log($"Speed value:{rb.linearVelocity.y}");

    }

    public void Init(BulletData data, IObjectPool<Bullet> bulletPoolBelonged,PlayerController pc,Vector2 position,List<IEffect<Bullet>> effects = null)
    {
        if (!initFirstTimeValues)
        {
            dataReference = data;
            baseData = data.CopyClassInstance();
            updatedData = baseData;
            initFirstTimeValues = true;
        }

        storedEffects = new List<IEffect<Bullet>>(effects);

        if (effects != null)
        {
            foreach (var effect in effects)
            {
                Debug.Log(effect.GetType().Name);
                effect.Apply(this);
            }
        }
        this.bulletPoolBelonged = bulletPoolBelonged;
        playerReference = pc;     
        generationPoint = position;
    }

    public virtual void ApplyModeSpecification()
    {
        SetSpeed();
        SetPosition();
    }

    public virtual void SetPosition()
    {
        transform.position = generationPoint + new Vector2(0, playerReference.boundarySize.y / 2);
    }

    public virtual void SetSpeed()
    {
        if(!rb)
            rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = updatedData.speed;
    }

    public virtual void Release()
    {
        updatedData = baseData;
        bulletPoolBelonged.Release(this);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.GetComponent<PlayerController>() && !collision.GetComponent<Bullet>())
            updatedData.pierceCount--;

        if (updatedData.pierceCount <= 0 )
        {
            if(gameObject.activeSelf)
                Release();
        }
    }

}
