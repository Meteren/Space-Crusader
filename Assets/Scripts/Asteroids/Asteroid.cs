using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Asteroid : MonoBehaviour, IDamageable<Bullet>
{
    [Header("Health Segment")]
    [SerializeField] private float health;
    [SerializeField] private TextMeshProUGUI healthIndicator;
    [SerializeField] private float damageIndicationTime;

    Rigidbody2D rb;
    SpriteRenderer sr;

    public bool isDestroyed;


    protected List<IEffect<IDamageable<Bullet>>> activeEffects = new();

    protected void Start()
    {
        transform.localScale /= CameraScaler.scaleFactor;
        healthIndicator = healthIndicator.GetComponentInChildren<TextMeshProUGUI>();
        healthIndicator.text = health.ToString();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        //rb.linearVelocity = new Vector2(0, -2);
    }
    public virtual void MoveLogic()
    {
        return;
    }

    public void OnDamage(Bullet bullet)
    {
        StartCoroutine(DamageIndicator());
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

    private void OnDisable()
    {
        ClearEffects();
    }

    private void ClearEffects()
    {
        foreach (var effect in activeEffects.ToList())
        {
            effect.Cancel();
        }
    }

    private IEnumerator DamageIndicator()
    {
        Color color = sr.color;
        Color damageColor = Color.red;
        sr.color = damageColor;

        yield return new WaitForSeconds(damageIndicationTime);

        sr.color = color;   
    }

}
