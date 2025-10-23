using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectGrid : MonoBehaviour
{
    private EffectResolver effectInHold; //type can be checked if its a wanted type according to the
                                         //IEffect<TTarget> using etc. effectInHold is IEffect<Bullet>
    private BulletSpawner bSpawner;

    [Header("Grid Fields")]
    [SerializeField] private TextMeshProUGUI levelCount;
    [SerializeField] private TextMeshProUGUI effectName;
    [SerializeField] private Button button;

    IEffect<Bullet> effectInProgress;

    IEffect<Bullet> convertedEffectInHold => effectInHold is IEffect<Bullet> effect ? effect : null;

    EffectGridController effectGridController;

    public void InitGrid()
    {
        levelCount.text = "Blank";
        effectName.text = "Blank";
        button.enabled = false;
    }

    public void InitGrid(EffectGridController effectGridController, EffectResolver effectInHold, BulletSpawner bSpawner)
    {
        this.effectGridController = effectGridController;
        button.enabled = true;
        this.effectInHold = effectInHold;
        this.bSpawner = bSpawner;

        if (bSpawner.TryGetEffect(effectInHold.EffectType,effectInHold.TargetType, out IEffect<Bullet> effect))
        {
            levelCount.text = "LV " + (effect.EffectLevel + 1).ToString();
            effectName.text = effectInHold.ToString();
            effectInProgress = effect;

        }
        else
        {
            if(effectInHold is IResolveAsAbility<BulletSpawner>)
            {
                levelCount.text = effectInHold.ToString();
                effectName.text = "Ability";
                effectInProgress = null;
                return;
            }

            levelCount.text = "LV " + (convertedEffectInHold.EffectLevel + 1).ToString();
            effectName.text = effectInHold.ToString();
            effectInProgress = null;
        }
   
    }

    public void AddEffect() 
    {
        if(effectInProgress != null)
        {
            effectInProgress.EffectLevel++;
        }
        else
        {
            if(convertedEffectInHold != null)
            {
                IEffect<Bullet> instantiatedEffect = convertedEffectInHold.CreateInstance();
                EffectResolver resolver = instantiatedEffect as EffectResolver;
                resolver.SetDependents(effectInHold.dependentEffects);

                if (resolver != null)
                    resolver.SetTargetType(effectInHold.TargetType);

                Debug.Log(instantiatedEffect.GetType().Name + " " + resolver.TargetType.Name);

                //Check if it has an ability resolver
                if (instantiatedEffect != null)
                {
                    IResolveAsAbility<BulletSpawner> abilityResolver = null;
                    IDataProvider<BulletData> providedDependency = null;
                    if(instantiatedEffect is IResolveAsAbility<BulletSpawner> convertedResolver)
                    {
                        abilityResolver = convertedResolver;
                        abilityResolver.SendData(bSpawner);
                    }
                
                    instantiatedEffect.onComplete += bSpawner.RemoveEffect;
                    if(abilityResolver == null)
                        instantiatedEffect.EffectLevel++;
                    bSpawner.AddEffect(instantiatedEffect);

                    if (instantiatedEffect is IDataProvider<BulletData> convertedDependencyProvider)
                    {
                             
                        providedDependency = convertedDependencyProvider;

                        BulletData belongsTo = bSpawner.GetDataToBeEffected(instantiatedEffect);

                        providedDependency.Provide(belongsTo);
                    }


                }
            }
                        
        }

        effectGridController.ActiveMaxedOutEffectCount = effectGridController.GetMaxedEffects(effectGridController.GetAllBulletEffects()).Count;

        UIManager.instance.HandleSkillWindow();
            
    }
}
