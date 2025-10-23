using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Asteroid : MonoBehaviour, IDamageable<Bullet>, IDamageable<PiercerSkill>
{
    [Header("Health Segment")]
    public float health;
    [SerializeField] private TextMeshProUGUI healthIndicator;
    [SerializeField] private float damageIndicationTime;

    Rigidbody2D rb;
    SpriteRenderer sr;

    public bool isDestroyed;

    protected List<IEffect<IDamageable<Bullet>>> activeEffects = new();

    public List<IEffect<IDamageable<Bullet>>> ActiveEffects { get { return activeEffects; } }

    bool rotationRandomized;
    int rotationDir;

    Camera cam;

    CircleCollider2D circleCollider;

    Coroutine damageCoroutine;

    LevelController levelController;

    protected virtual void Start()
    {
        levelController = GameObject.Find("LevelGeneration").GetComponent<LevelController>();

        //transform.localScale *= CameraViewportHandler.Instance.scaleFactor;
        healthIndicator = healthIndicator.GetComponentInChildren<TextMeshProUGUI>();
        healthIndicator.text = health.ToString();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cam = Camera.main;
        circleCollider = GetComponent<CircleCollider2D>();
        //rb.linearVelocity = new Vector2(0, -2);
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
            Debug.Log("In Here");
        }

        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);

        damageCoroutine = StartCoroutine(DamageIndicator());
        health -= bullet.DamageAmount;
        healthIndicator.text = health.ToString();
        Debug.Log("On Damage");
        if (health <= 0)
        {
            //play a vfx in here such as particle effect or erosion like shader effect
            Destroy(gameObject);
        }

    }

    public void OnDamage(PiercerSkill damageSource, IEffect<IDamageable<PiercerSkill>> effectResolver = null)
    {

        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);

        damageCoroutine = StartCoroutine(DamageIndicator());
        health -= damageSource.damageAmount;
        healthIndicator.text = health.ToString();
        Debug.Log("On Damage");
        if (health <= 0)
        {
            //play a vfx in here such as particle effect or erosion like shader effect
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
            levelController.progressAmount += 0.25f * levelController.finalDecreaseVal
                / (exBullet.enemiesToBeEffected.Count != 0 ? exBullet.enemiesToBeEffected.Count : 1);
            return;
        }
        levelController.progressAmount += 0.25f * levelController.finalDecreaseVal;//can be changed
    }


}
