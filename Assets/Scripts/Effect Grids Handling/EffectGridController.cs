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
        effects.Add(new IncreaseFireRateEffect());
        effects.Add(new IncreasePiercingEffect());
        effects.Add(new SpeedUpEffect());
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
        Debug.Log("Scatter called");
        List<IEffect<Bullet>> collectedEffects = new List<IEffect<Bullet>>(bSpawner.bulletEffects);
        List<EffectResolver> effectsMaxedOut = new List<EffectResolver>();


        effectsMaxedOut = collectedEffects.Select((x) => 
        {
            int currentLevel = x.EffectLevel;

            if(x is EffectResolver resolver && currentLevel >= resolver.MaxLevel)
                return resolver;
            else
                return null;

        }).ToList();

        Debug.Log($"Effects not maxed out count: {effectsMaxedOut}");

        List<EffectResolver> effectsCopy = new List<EffectResolver>(effects);

        if (effectsMaxedOut.Count > 0)
        {
            effectsCopy = effectsCopy
                .Where(x => !effectsMaxedOut.Any(y => y != null && x.Type == y.Type))
                .ToList();
        }

        Debug.Log($"Effects copy count:{effectsCopy.Count}");

        effectsCopy.Sort((a, b) => a.CompareTo(b));
       

        for(int i = 0; i < grids.Count; i++)
        {
            if(i < effectsCopy.Count)
                grids[i].InitGrid(effectsCopy[i],bSpawner);
            else
                grids[i].InitGrid();
        }

    }
}
