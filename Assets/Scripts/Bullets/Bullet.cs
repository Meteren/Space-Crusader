
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [Header("Damage Amount")]
    [SerializeField] private float damageAmount;

    public float DamageAmount {get {return damageAmount;}}

    public BulletData.DataFields updatedData;

    public BulletData.DataFields baseData;

    [HideInInspector] public BulletData dataReference; 

    protected IObjectPool<Bullet> bulletPoolBelonged;

    protected Rigidbody2D rb;

    protected bool initFirstTimeValues = false;

    protected PlayerController playerReference;

    protected Vector2 generationPoint;

    public IObjectPool<Bullet> BulletPoolBelonged { get => bulletPoolBelonged; }

    protected ParticleSpawner pSpawner;

    [Header("Particle Effects")]
    [SerializeField] protected ParticleSystem hitEffect;
    TrailRenderer trailRenderer;

    protected SpriteRenderer sr;

    protected void Start()
    {
        sr = GetComponent<SpriteRenderer>();    
        trailRenderer = GetComponent<TrailRenderer>();
        pSpawner = FindFirstObjectByType<ParticleSpawner>();
    }

    public void Init(BulletData data, IObjectPool<Bullet> bulletPoolBelonged,PlayerController pc,Vector2 position)
    {

        if (!initFirstTimeValues)
        {
            dataReference = data;
            baseData = data.CopyClassInstance();
            updatedData = baseData;
            initFirstTimeValues = true;
        }

        if (data.effects != null)
        {
            foreach (var effect in data.effects)
            {
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
        if(trailRenderer != null)
            trailRenderer.Clear();
        updatedData = baseData;
        if(gameObject.activeSelf)
            bulletPoolBelonged.Release(this);
        
    }

    public virtual void InflictDamage()
    {

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<IDamageable<Bullet>>(out IDamageable<Bullet> damagaeable))
        {
            damagaeable.OnDamage(this);
            MonoBehaviour monoB = damagaeable as MonoBehaviour;

            Vector2 direction = transform.position - monoB.transform.position;

            float rotationAngle = Mathf.Atan2(direction.y, direction.x);

            ParticleSystem spawnedParticle = pSpawner.Spawnparticle(hitEffect,collision.ClosestPoint(transform.position));

            spawnedParticle.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);

            ParticleHandler pHandler = spawnedParticle.GetComponent<ParticleHandler>();

            pHandler.InitReturnToPoolProcess();
     
        }

        if(!collision.GetComponent<PlayerController>() && !collision.GetComponent<Bullet>() && !collision.GetComponent<TargetLocator>())
            updatedData.pierceCount--;


        if (updatedData.pierceCount <= 0 )
        {
            if(gameObject.activeSelf)
                Release();
        }
    }


}
