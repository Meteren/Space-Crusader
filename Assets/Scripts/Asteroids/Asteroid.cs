using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Asteroid : MonoBehaviour, IDamageable<Bullet>, IDamageable<PiercerSkill>
{
    [Header("Health Segment")]
    [HideInInspector] public float health;
    [SerializeField] private TextMeshProUGUI healthIndicator;
    [SerializeField] private float damageIndicationTime;

    [HideInInspector]public float prevHealth;
    SpriteRenderer sr;

    public bool isDestroyed;

    protected List<IEffect<IDamageable<Bullet>>> activeEffects = new();

    public List<IEffect<IDamageable<Bullet>>> ActiveEffects { get { return activeEffects; } }

    bool rotationRandomized;
    int rotationDir;

    Camera cam;

    CircleCollider2D circleCollider;

    Coroutine damageCoroutine;

    LevelController levelController => GameManager.instance.levelController;


    [Header("Effects")]
    [SerializeField] protected ParticleSystem explosionEffect;

    ParticleSpawner pSpawner => GameManager.instance.pSpawner;

    float referenceScale = 0.2352913f;

    protected virtual void Start()
    {
       

        healthIndicator = GetComponentInChildren<TextMeshProUGUI>();
        healthIndicator.text = health.ToString();
        sr = GetComponent<SpriteRenderer>();

        cam = Camera.main;
        circleCollider = GetComponent<CircleCollider2D>();

    }
    protected virtual void Update()
    {

        if (IsOutOfCamera())
            Destroy(gameObject);


        if (!rotationRandomized)
        {
            rotationDir = Random.Range(0, 2);
            rotationDir = rotationDir == 0 ? rotationDir - 1 : rotationDir;
            rotationRandomized = true;  
        }
        transform.Rotate(0, 0, 10 * rotationDir * Time.deltaTime);
    }

    public virtual void MoveLogic()
    {
        return;
    }

    public void OnDamage(Bullet bullet,IEffect<IDamageable<Bullet>> effectReference)
    {
        IncreaseSkillWindowActivationValue(bullet);
        if(effectReference != null && bullet is ExplosiveBullet exBullet)
        {
            float distance = Vector2.Distance(bullet.transform.position, transform.position) - circleCollider.radius;
            if(distance >= exBullet.ImpactRadius)
            {
                effectReference.Cancel();
                exBullet.enemiesToBeEffected.Remove(this);
                return;
            }
        }

        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);

        damageCoroutine = StartCoroutine(DamageIndicator());

        health -= bullet.updatedData.damageAmount;

        GameManager.instance.scoreInALevel += prevHealth - health;

        prevHealth = health;

        healthIndicator.text = health.ToString();
        //Debug.Log("On Damage");

        if (health <= 0)
        {
            CreateEplosionParticle();
            Destroy(gameObject);

        }

    }

    public void OnDamage(PiercerSkill damageSource, IEffect<IDamageable<PiercerSkill>> effectResolver = null)
    {

        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);

        damageCoroutine = StartCoroutine(DamageIndicator());
        health -= damageSource.damageAmount;

        GameManager.instance.scoreInALevel += prevHealth - (health < 0 ? 0 : health);

        healthIndicator.text = health.ToString();

        if (health <= 0)
        {
            CreateEplosionParticle();
            Destroy(gameObject);

        }
    }

    public void AddEffect(IEffect<IDamageable<Bullet>> effect)
    {
        activeEffects.Add(effect);
        effect.onComplete += RemoveEffect;
        effect.Apply(this);
    }

    public void RemoveEffect(IEffect<IDamageable<Bullet>> effect)
    {
        effect.onComplete -= RemoveEffect;
        activeEffects.Remove(effect);
    }

    protected void OnDestroy()
    {
        isDestroyed = true;
        ClearEffects();
    }

    private void ClearEffects()
    {
        foreach (var effect in activeEffects.ToList())
        {
            effect.Cancel();
        }
        activeEffects.Clear();
    }

    private IEnumerator DamageIndicator()
    {
        sr.color = Color.red;

        yield return new WaitForSeconds(damageIndicationTime);

        sr.color = Color.white;  
        damageCoroutine = null;
    }
    
    protected bool IsOutOfCamera()
    {
        float bottomY = cam.ScreenToWorldPoint(new Vector2(0,0)).y;

        return transform.position.y < bottomY - circleCollider.radius / 2;

    }

    protected void IncreaseSkillWindowActivationValue(Bullet bullet)
    {
        if(bullet is ExplosiveBullet exBullet)
        {
            levelController.progressAmount += (0.25f * levelController.finalDecreaseVal) //???
                / (exBullet.enemiesToBeEffected.Count != 0 ? exBullet.enemiesToBeEffected.Count + 3 : 1);
            return;
        }

        if(bullet is ScatteredBullet sBullet)
        {
            levelController.progressAmount += (0.25f * levelController.finalDecreaseVal) //???
               / (sBullet.updatedData.shotsToReflectCount == 1 ? sBullet.updatedData.shotsToReflectCount : sBullet.updatedData.shotsToReflectCount / 2);
            return;
        }

        levelController.progressAmount += 0.25f * levelController.finalDecreaseVal;//can be changed
    }

    private void CreateEplosionParticle()
    {

        ParticleSystem instantiatedParticle = pSpawner.ParticleFactory.Create(explosionEffect, transform.position);

        ParticleHandler pHandler = instantiatedParticle.GetComponent<ParticleHandler>();

        float spriteWidth = sr.size.x;

        float scaleFactor = circleCollider.radius / referenceScale;
    
        pHandler.SetScale(scaleFactor);

        instantiatedParticle.Play();

        pHandler.InitReturnToPoolProcess();
    }


}
