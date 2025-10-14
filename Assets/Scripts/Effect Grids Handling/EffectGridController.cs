using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EffectGridController : MonoBehaviour
{
    [SerializeField] private List<Grid> grids;
    private List<EffectResolver> effects = new();
    private BulletSpawner bSpawner;
    private void Start()
    {
        effects.Add(new IncreaseFireRateEffect(typeof(Bullet)));
        effects.Add(new IncreasePiercingEffect(typeof(Bullet)));
        effects.Add(new SpeedUpEffect(typeof(Bullet)));
        gameObject.SetActive(false);
        
    }

    private void OnEnable()
    {
        if(bSpawner == null)
            bSpawner = GameObject.FindFirstObjectByType<BulletSpawner>();
        if(bSpawner != null)
            ScatterEffects();
    }

    private void ScatterEffects()
    {

        List<EffectResolver> discriminatedEffects = effects
            .Where(x => bSpawner.bulletDataInstances
            .Any(y => (x.TargetType == y.prefab.GetType()) || x is IResolveAsAbility<BulletSpawner>)).ToList();

        List<IEffect<Bullet>> allBulletEffects = bSpawner.bulletDataInstances
        .SelectMany(x => x.effects)
        .ToList();

        List<EffectResolver> effectsMaxedOut = allBulletEffects.Select((x) => 
        {
            int currentLevel = x.EffectLevel;

            if(x is EffectResolver effectResolver 
               && effectResolver is IResolveAsAbility<BulletSpawner>
               && currentLevel >= effectResolver.MaxLevel )
                return effectResolver;
            else
                return null;

        }).ToList();


        if (effectsMaxedOut.Count > 0)
        {
            discriminatedEffects = discriminatedEffects
                .Where(x => !effectsMaxedOut.Any(y => y != null && x.EffectType == y.EffectType))
                .ToList();
        }

        discriminatedEffects.Sort((a, b) => a.CompareTo(b));
       
        for(int i = 0; i < grids.Count; i++)
        {
            if(i < discriminatedEffects.Count)
                grids[i].InitGrid(discriminatedEffects[i],bSpawner);
            else
                grids[i].InitGrid();
        }

    }
}
