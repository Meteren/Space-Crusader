using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Asteroid : MonoBehaviour, IDamageable<Bullet>
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

    protected virtual void Start()
    {
        transform.localScale /= CameraScaler.scaleFactor;
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

        if(effectReference != null && bullet is ExplosiveBullet exBullet)
        {
            float distance = Vector2.Distance(bullet.transform.position, transform.position);
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
        health -= bullet.DamageAmount;
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
        Color color = sr.color;
        Color damageColor = Color.red;
        sr.color = damageColor;

        yield return new WaitForSeconds(damageIndicationTime);

        sr.color = color;  
        damageCoroutine = null;
    }
    
    protected bool IsOutOfCamera()
    {
        float bottomY = cam.ScreenToWorldPoint(new Vector2(0,0)).y;

        return transform.position.y < bottomY - circleCollider.radius / 2;

    }

}
