using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EffectGridController : MonoBehaviour
{
    [SerializeField] private List<EffectGrid> grids;
    private List<EffectResolver> effects = new();
    private BulletSpawner bSpawner;

    public int EffectCount {  get { return effects.Count; } }

    public int ActiveMaxedOutEffectCount { get; set; }
    private void Start()
    {
        effects.Add(new IncreaseFireRateEffect(typeof(Bullet)));
        effects.Add(new IncreasePiercingEffect(typeof(Bullet)));
        effects.Add(new SpeedUpEffect(typeof(Bullet)));
        effects.Add(new BoostDamageAmountEffect(typeof(Bullet)));
        effects.Add(new ExplosiveBulletActivationEffect(typeof(ExplosiveBullet)));
        effects.Add(new BurstCountBoostEffect(typeof(ExplosiveBullet)));
        effects.Add(new SpeedUpEffect(typeof(ExplosiveBullet)));
        effects.Add(new TimeBetweenBurstBoosterEffect(typeof(ExplosiveBullet), new List<Type>() { typeof(BurstCountBoostEffect)}));
        effects.Add(new IncreaseFireRateEffect(typeof(ExplosiveBullet)));
        effects.Add(new ScatteredBulletActivationEffect(typeof(ScatteredBullet)));
        effects.Add(new IncreaseFireRateEffect(typeof(ScatteredBullet)));
        effects.Add(new SpeedUpEffect(typeof(ScatteredBullet)));
        effects.Add(new IncreasePiercingEffect(typeof(ScatteredBullet)));
        effects.Add(new BulletCountToScatterBoosterEffect(typeof(ScatteredBullet)));

        gameObject.SetActive(false);

    }

    private void OnEnable()
    {
        if (bSpawner == null)
            bSpawner = GameObject.FindFirstObjectByType<BulletSpawner>();
        if (bSpawner != null)
            ScatterEffects();
    }

    private void ScatterEffects()
    {

        List<EffectResolver> discriminatedEffects = effects
        .Where(x => bSpawner.bulletDataInstances
            .Any(y =>
            {
                bool hasDependent = false;

                List<Type> foundTypes = new List<Type>();

                if (x.dependentEffects != null)
                {
                    if (x.TargetType == y.bulletType || x is IResolveAsAbility<BulletSpawner>)
                    {
                        foreach (var effect in y.effects)
                        {
                            foreach (var type in x.dependentEffects)
                            {
                                if (effect.GetType() == type)
                                    foundTypes.Add(type);
                            }
                        }
                        if (foundTypes.Count == x.dependentEffects.Count)
                            hasDependent = true;
                    }

                    return hasDependent;
                }
                else
                    return x.TargetType == y.bulletType || x is IResolveAsAbility<BulletSpawner>;

            }))
        .ToList();

        if (discriminatedEffects != null)
            Debug.Log($"Discriminated effect count: {discriminatedEffects.Count}");

        List<IEffect<Bullet>> allBulletEffects = GetAllBulletEffects();

        
        Debug.Log($"All bullet effects:{allBulletEffects.Count}");

        List<EffectResolver> effectsMaxedOut = GetMaxedEffects(allBulletEffects);

        if(effectsMaxedOut != null)
                    Debug.Log($"Effects maxed out count:{effectsMaxedOut.Count}");

        if (effectsMaxedOut.Count > 0)
        {
            discriminatedEffects = discriminatedEffects
            .Where(x => !effectsMaxedOut
            .Any(y => x.EffectType == y.EffectType && x.TargetType == y.TargetType))
            .ToList();

        }

        //discriminatedEffects.Sort((a, b) => a.CompareTo(b));

        //discriminatedEffects = discriminatedEffects.OrderBy(x => UnityEngine.Random.value).ToList();

        foreach(var effect in discriminatedEffects)
        {
            effect.CalcProbability();
        }

        discriminatedEffects.Sort((a, b) => a.CompareTo(b));
       
        for(int i = 0; i < grids.Count; i++)
        {
            if(i < discriminatedEffects.Count)
                grids[i].InitGrid(this,discriminatedEffects[i],bSpawner);
            else
                grids[i].InitGrid();
        }

    }


    public List<EffectResolver> GetMaxedEffects(List<IEffect<Bullet>> allBulletEffects)
    {
        return allBulletEffects.Select((x) =>
        {
            int currentLevel = x.EffectLevel;

            if (x is EffectResolver effectResolver)
            {
                if ((currentLevel >= effectResolver.MaxLevel) || effectResolver is IResolveAsAbility<BulletSpawner>)
                    return effectResolver;
                return null;
            }
            else
                return null;

        }).Where(x => x != null).ToList();
    }


    public List<IEffect<Bullet>> GetAllBulletEffects()
    {
        return bSpawner.bulletDataInstances
       .SelectMany(x => x.effects)
       .ToList();
    }

    public void HandleSkillWindow()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        GameManager.instance.PauseOrContinueGame();

    }

}
