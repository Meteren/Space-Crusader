using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
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

    public void InitGrid()
    {
        levelCount.text = "Default";
        effectName.text = "Default";
        button.enabled = false;
    }

    public void InitGrid(EffectResolver effectInHold, BulletSpawner bSpawner)
    {
        button.enabled = true;
        this.effectInHold = effectInHold;
        this.bSpawner = bSpawner;

        if (bSpawner.TryGetEffect(effectInHold.Type, out IEffect<Bullet> effect))
        {
            levelCount.text = (effect.EffectLevel + 1).ToString();
            effectName.text = effectInHold.ToString();
            effectInProgress = effect;

        }
        else
        {
            levelCount.text = (convertedEffectInHold.EffectLevel + 1).ToString();
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
                Debug.Log(instantiatedEffect.GetType().Name);

                if (instantiatedEffect != null)
                {
                    instantiatedEffect.onComplete += bSpawner.RemoveEffect;
                    instantiatedEffect.EffectLevel++;
                    bSpawner.AddEffect(instantiatedEffect);
                }
            }
                        
        }
        GetComponentInParent<EffectGridController>().gameObject.SetActive(false);
            
    }
}
